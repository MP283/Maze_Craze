using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

public class MultiplayerMainMenu : NetworkBehaviour
{

    private NetworkList<PlayerData> playerDataNetworkList;

    
    [SerializeField] private List<Color> playerColorList;

    //OnPlayerDataNetworkListChanged
    public event EventHandler OnPlayerDataNetworkListChanged;

    public static MultiplayerMainMenu Instance { get; private set; }

    private void Awake(){
    
        Instance = this;
        if (playerDataNetworkList == null) {
        playerDataNetworkList = new NetworkList<PlayerData>();
        }
        playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
        
        DontDestroyOnLoad(gameObject);
    
    }

    private void OnNetworkSpawn()
    {
        if (IsServer)
        {
            playerDataNetworkList = new NetworkList<PlayerData>();
            playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
        }
    }

    
    public void StartHost(){
    Debug.Log("StartHost");

        if (NetworkManager.Singleton != null) {
            // Ensure the NetworkManager persists across scenes
            DontDestroyOnLoad(NetworkManager.Singleton.gameObject);

            // Register the callback before starting the host

            if (playerDataNetworkList == null) {
                playerDataNetworkList = new NetworkList<PlayerData>();
                Debug.Log("Initialized playerDataNetworkList");
            }
            
            

            // Start the host
            NetworkManager.Singleton.StartHost();

            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
            AddPlayerDataServerRpc(NetworkManager.Singleton.LocalClientId);
            NetworkManager_Client_OnClientConnectedCallback(NetworkManager.Singleton.LocalClientId);
            
            SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
            // Load the CharacterSelect scene
            NetworkManager.Singleton.SceneManager.LoadScene("CharacterSelect", LoadSceneMode.Single);
        } else {
            Debug.LogError("NetworkManager.Singleton is null");
        }
    }

    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId) {
        for (int i = 0; i < playerDataNetworkList.Count; i++) {
            PlayerData playerData = playerDataNetworkList[i];
            if (playerData.clientId == clientId) {
                // Disconnected!
                playerDataNetworkList.RemoveAt(i);
            }
        }
    }

    

    [ServerRpc(RequireOwnership = false)]
    private void AddPlayerDataServerRpc(ulong clientId) {
        playerDataNetworkList.Add(new PlayerData { 
            clientId = clientId,
            colorId = GetFirstUnusedColorId()});

        Debug.Log("AddPlayerDataServerRpc");
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId) {
        AddPlayerDataServerRpc(clientId);
        Debug.Log("NetworkManager_OnClientConnectedCallback");
    }

    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent) {
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
        
        Debug.Log("PlayerDataNetworkList_OnListChanged");
    }

    public bool IsPlayerIndexConnected(int playerIndex) {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            
            Debug.Log(clientId);
        }
        return playerIndex < playerDataNetworkList.Count;
    }

    public void StartClient(){
            
        Debug.Log("StartClient");
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;
        NetworkManager.Singleton.StartClient(); 
        //SceneManager.LoadSceneAsync("Multiplayer_Level");

    }

    public async void NetworkManager_Client_OnClientConnectedCallback(ulong clientId){
        SetPlayerNameServerRpc(await CloudSaveScript.Instance.LoadData("UserName"));
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameServerRpc(string UserName, ServerRpcParams serverRpcParams = default) {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.playerUserName = UserName;

        playerDataNetworkList[playerDataIndex] = playerData;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default) {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.playerId = playerId;

        playerDataNetworkList[playerDataIndex] = playerData;
    }

    private void OnDestroy() {
        if (playerDataNetworkList != null) {
            playerDataNetworkList.Dispose();
        }
    }

    public PlayerData GetPlayerDataFromPlayerIndex(int playerIndex){
        return playerDataNetworkList[playerIndex];
    }

    public PlayerData GetPlayerDataFromClientId(ulong clientId){

        foreach(PlayerData playerData in playerDataNetworkList){
            if(playerData.clientId == clientId){
                return playerData;
            }
        }return default;

    }

    public PlayerData GetPlayerData(){
        return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }

    public Color GetPlayerColor(int colorId){

        return playerColorList[colorId]; 
    }

    public void ChangePlayerColor(int colorId){
        ChangePlayerColorServerRpc(colorId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangePlayerColorServerRpc(int colorId, ServerRpcParams serverRpcParams = default) {
        if (!IsColorAvailable(colorId)) {
            // Color not available
            return;
        }

        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.colorId = colorId;

        playerDataNetworkList[playerDataIndex] = playerData;
    }

    public int GetPlayerDataIndexFromClientId(ulong clientId) {
        for (int i=0; i< playerDataNetworkList.Count; i++) {
            if (playerDataNetworkList[i].clientId == clientId) {
                return i;
            }
        }
        return -1;
    }

    private bool IsColorAvailable(int colorId) {
        foreach (PlayerData playerData in playerDataNetworkList) {
            if (playerData.colorId == colorId) {
                // Already in use
                return false;
            }
        }
        return true;
    }

    public int GetFirstUnusedColorId() {
        for (int i = 0; i<playerColorList.Count; i++) {
            if (IsColorAvailable(i)) {
                return i;
            }
        }
        return -1;
    }

    public void KickPlayer(ulong clientId) {
        NetworkManager.Singleton.DisconnectClient(clientId);
        NetworkManager_Server_OnClientDisconnectCallback(clientId);
        if(clientId == NetworkManager.ServerClientId){
		    NetworkManager.Singleton.Shutdown();
        }
    }


    public void GoToMainMenu(){
        
        SceneManager.LoadSceneAsync("Main_menu");
    }
    
}