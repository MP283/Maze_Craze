using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.Netcode;

using TMPro;

public class CharacterSelectPlayer : MonoBehaviour
{
    
    [SerializeField] private int playerIndex;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private GameObject readyGameObject;
    [SerializeField] private TMP_Text UsernameText;
    
    [SerializeField] private GameObject kickButton;

    private void Awake(){
        MultiplayerMainMenu.Instance.OnPlayerDataNetworkListChanged += MultiplayerMainMenu_OnPlayerDataNetworkListChanged;
        //CharacterSelectUI.Instance.OnReadyChanged += CharacterSelectUI_OnReadyChanged;
        //MultiplayerMainMenu_OnPlayerDataNetworkListChanged(this, EventArgs.Empty);
        
        
    }

    public void KickPlayerButton(){
        PlayerData playerData = MultiplayerMainMenu.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
        LobbyManager.Instance.KickPlayer(playerData.playerId.ToString());
        MultiplayerMainMenu.Instance.KickPlayer(playerData.clientId);
    }

    private void Start() {
        UpdatePlayer();
        MultiplayerMainMenu.Instance.OnPlayerDataNetworkListChanged += MultiplayerMainMenu_OnPlayerDataNetworkListChanged;
        CharacterSelectUI.Instance.OnReadyChanged += CharacterSelectUI_OnReadyChanged;

        kickButton.SetActive(NetworkManager.Singleton.IsServer);
        
    }

   
    private void CharacterSelectUI_OnReadyChanged(object sender, System.EventArgs e){
        UpdatePlayer();
    }


    private void MultiplayerMainMenu_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e) {
        UpdatePlayer();
        
        
    }

    private async void UpdatePlayer() {
        //Hide();
        if (MultiplayerMainMenu.Instance.IsPlayerIndexConnected(playerIndex)) {
            Show();
            PlayerData playerData = MultiplayerMainMenu.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            readyGameObject.SetActive(CharacterSelectUI.Instance.IsPlayerReady(playerData.clientId));

            playerVisual.SetPlayerColor(MultiplayerMainMenu.Instance.GetPlayerColor(playerData.colorId));

            //UsernameText.text = "gay";

            UsernameText.text = playerData.playerUserName.ToString();;

            /*
            PlayerData playerData = MultiplayerMainMenu.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            
            readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));

            playerNameText.text = playerData.playerName.ToString();

            */
        } else {

            Debug.Log($"Player Index: {playerIndex}");

            Hide();
        }
    }

    private void onDestroy(){
		
        MultiplayerMainMenu.Instance.OnPlayerDataNetworkListChanged -= MultiplayerMainMenu_OnPlayerDataNetworkListChanged;
	}

    private void Show(){
        gameObject.SetActive(true);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }
}
