using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;
    public AudioSource ButtonSoundAudioSource;


    private void Awake() {
        closeButton.onClick.AddListener(closeButtonSound);
    }

    private void Start() {
        //KitchenGameMultiplayer.Instance.OnFailedToJoinGame += KitchenGameMultiplayer_OnFailedToJoinGame;
        LobbyManager.Instance.OnCreateLobbyStarted += LobbyManager_OnCreateLobbyStarted;
        LobbyManager.Instance.OnCreateLobbyFailed += LobbyManager_OnCreateLobbyFailed;
        LobbyManager.Instance.OnJoinStarted += LobbyManager_OnJoinStarted;
        LobbyManager.Instance.OnJoinFailed += LobbyManager_OnJoinFailed;
        LobbyManager.Instance.OnQuickJoinFailed += LobbyManager_OnQuickJoinFailed;

        Hide();
    }

    private void LobbyManager_OnQuickJoinFailed(object sender, System.EventArgs e) {
        ShowMessage("Could not find a Lobby to Quick Join!");
    }

    private void LobbyManager_OnJoinFailed(object sender, System.EventArgs e) {
        ShowMessage("Failed to join Lobby!");
    }

    private void LobbyManager_OnJoinStarted(object sender, System.EventArgs e) {
        ShowMessage("Joining Lobby...");
    }

    private void LobbyManager_OnCreateLobbyFailed(object sender, System.EventArgs e) {
        ShowMessage("Failed to create Lobby!");
    }

    private void LobbyManager_OnCreateLobbyStarted(object sender, System.EventArgs e) {
        ShowMessage("Creating Lobby...");
    }

    /*
    private void KitchenGameMultiplayer_OnFailedToJoinGame(object sender, System.EventArgs e) {
        if (NetworkManager.Singleton.DisconnectReason == "") {
            ShowMessage("Failed to connect");
        } else {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
    }*/

    private void ShowMessage(string message) {
        Show();
        messageText.text = message;
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void closeButtonSound(){
        ButtonSoundAudioSource.Play();
        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        //KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= KitchenGameMultiplayer_OnFailedToJoinGame;
        LobbyManager.Instance.OnCreateLobbyStarted -= LobbyManager_OnCreateLobbyStarted;
        LobbyManager.Instance.OnCreateLobbyFailed -= LobbyManager_OnCreateLobbyFailed;
        LobbyManager.Instance.OnJoinStarted -= LobbyManager_OnJoinStarted;
        LobbyManager.Instance.OnJoinFailed -= LobbyManager_OnJoinFailed;
        LobbyManager.Instance.OnQuickJoinFailed -= LobbyManager_OnQuickJoinFailed;
    }

}