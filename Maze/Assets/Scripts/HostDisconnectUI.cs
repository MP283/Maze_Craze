using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class HostDisconnectUI : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HostDisconnectUI script is being created.");
        NetworkManager.Singleton.OnServerStopped  += NetworkManager_OnHostDisconnectCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback  += NetworkManager_OnClientDisconnectCallback;
        Hide();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId){

        if(clientId == NetworkManager.Singleton.LocalClientId){
            Debug.Log("CLIENT_ID - "+clientId);
            Debug.Log("Local CLIENT_ID - "+ NetworkManager.Singleton.LocalClientId);
            Debug.Log("Server CLIENT_ID - "+ NetworkManager.ServerClientId);
            Debug.Log("gameobject- "+ gameObject);
            Show();
        }else if(clientId == NetworkManager.ServerClientId){
            Show();
        }
        
    }

    private void NetworkManager_OnHostDisconnectCallback(bool serverStatus){
       
        Debug.Log("ON SERVER STOPPED"+serverStatus);
        Show();
        
        
    }

    public void MainMenuButton(){
        NetworkManager.Singleton.Shutdown();
        
		LobbyManager.Instance.LeaveLobby();
        SceneManager.LoadScene("Main_menu");
    }

    private void Hide(){
        gameObject.SetActive(false);
    
    }

    private void Show(){
        gameObject.SetActive(true);

    }

    private void OnDestroy()
    {
        Debug.Log("HostDisconnectUI script is being destroyed.");
        // Clear any ongoing operations or event subscriptions
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStopped -= NetworkManager_OnHostDisconnectCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
