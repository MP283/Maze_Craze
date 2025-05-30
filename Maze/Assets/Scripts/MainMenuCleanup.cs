using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainMenuCleanup : MonoBehaviour
{
    private void Awake() {
        if (NetworkManager.Singleton != null) {
            Destroy(NetworkManager.Singleton.gameObject);
        }
        
        if (MultiplayerMainMenu.Instance != null) {
            Destroy(MultiplayerMainMenu.Instance.gameObject);
        }
        
        if (LobbyManager.Instance != null) {
            Destroy(LobbyManager.Instance.gameObject);
        }
    }
}
