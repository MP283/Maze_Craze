using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class CharacterSelectUI : NetworkBehaviour
{

	private Dictionary<ulong, bool> playerReadyDictionary;

    
	[SerializeField] private TMP_Text lobbyNameText;
	[SerializeField] private TMP_Text lobbyCodeText;
    
    public event EventHandler OnReadyChanged;

    public static CharacterSelectUI Instance { get; private set; }

	private void Awake(){
			
        Instance = this;
		playerReadyDictionary = new Dictionary<ulong, bool>();

	}

    private void Start(){
        Lobby lobby = LobbyManager.Instance.GetLobby();
        Debug.Log(lobby.LobbyCode);
        lobbyNameText.text = "Lobby Name : " + lobby.Name;
        lobbyCodeText.text = "Lobby Code : " + lobby.LobbyCode;
    }

	public void backToMultiplayerMenu(){
		NetworkManager.Singleton.Shutdown();
		LobbyManager.Instance.LeaveLobby();
        SceneManager.LoadSceneAsync("Main_menu");

	} 

	public void readyButton(){

		SetPlayerReadyServerRpc();

	} 

	[ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default) {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId]) {
                // This player is NOT ready
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady) {
            LobbyManager.Instance.DeleteLobby();
            NetworkManager.Singleton.SceneManager.LoadScene("Multiplayer_Level", LoadSceneMode.Single);
        }
    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientId){
        playerReadyDictionary[clientId] = true;
        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerReady(ulong clientId){

        return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId]; 
    }
}
