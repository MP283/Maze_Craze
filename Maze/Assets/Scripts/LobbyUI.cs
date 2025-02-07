using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class LobbyUI : MonoBehaviour
{
    
    
	[SerializeField] private TMP_InputField lobbyNameInputField;
	[SerializeField] private TMP_InputField lobbyCodeInputField;
	[SerializeField] private Transform lobbyContainer;
	[SerializeField] private Transform lobbyTemplate;

    private void Start() {
        
        lobbyTemplate.gameObject.SetActive(false);
        LobbyManager.Instance.OnLobbyListChanged += LobbyManager_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    public void CreatePublicButton(){
        LobbyManager.Instance.CreateLobby(lobbyNameInputField.text, false);
    }
    
    public void CreatePrivateButton(){
        LobbyManager.Instance.CreateLobby(lobbyNameInputField.text, true);
    }

    public void JoinWithCodeButton(){
        LobbyManager.Instance.JoinWithCode(lobbyCodeInputField.text);
    }

    private void LobbyManager_OnLobbyListChanged(object sender, LobbyManager.OnLobbyListChangedEventArgs e) {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList) {
        foreach (Transform child in lobbyContainer) {
            if (child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList) {
            Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }

    private void OnDestroy() {
        LobbyManager.Instance.OnLobbyListChanged -= LobbyManager_OnLobbyListChanged;
    }
}
