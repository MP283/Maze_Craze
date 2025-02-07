using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;


public class PauseMenu : MonoBehaviour
{
    public bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    public void MenuButton(){
        SceneManager.LoadSceneAsync(1);
    }

    public void MainMenuButton(){
        
        /*
        if(NetworkManager.Singleton.LocalClientId == NetworkManager.ServerClientId){
            PlayerData playerData = MultiplayerMainMenu.Instance.GetPlayerDataFromPlayerIndex(1);
            MultiplayerMainMenu.Instance.KickPlayer(playerData.clientId);
            //LobbyManager.Instance.KickPlayer(playerData.playerId.ToString());
            //MultiplayerMainMenu.Instance.KickPlayer(NetworkManager.Singleton.LocalClientId);
        }*/
        Time.timeScale = 1f;
        gameIsPaused = false;
		NetworkManager.Singleton.Shutdown();
		LobbyManager.Instance.LeaveLobby();
        SceneManager.LoadSceneAsync("Main_menu");
    }

    public void Resume(){

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;

    }

    public void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
