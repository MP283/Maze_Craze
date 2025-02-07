using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private Transform playerPrefab;
    public GameObject GameOverUI;
    public TextMeshProUGUI messageText;

	public GameObject Level_1,Level_2;
	
	public int MAP_NUMBER;
   
    public static GameManager Instance { get; private set; }
	public void Awake(){
        Instance = this;
    }

	private void Start(){
		MAP_NUMBER = LobbyManager.Map_number;

		Debug.Log("THIS IS MAP NUMBER: " + MAP_NUMBER);
		if(MAP_NUMBER == 1){
			Level_1.SetActive(true);
			Level_2.SetActive(false);
		}else if(MAP_NUMBER == 2){
			
			Level_2.SetActive(true);
			Level_1.SetActive(false);
		}
	}
	public override void OnNetworkSpawn(){

		if(IsServer){
			NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
		}
		
		
	}

	private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut) {
		foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
			Transform playerTransform = Instantiate(playerPrefab);
			
			
			playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
			
		}

	}

	public void MainMenuButton(){
        NetworkManager.Singleton.Shutdown();
        
		LobbyManager.Instance.LeaveLobby();
        SceneManager.LoadScene("Main_menu");
    }
}
