using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
public class demo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        NetworkManager.Singleton.OnClientDisconnectCallback  += NetworkManager_OnClientDisconnectCallback;
        Hide();
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId){

        if(clientId == NetworkManager.Singleton.LocalClientId){
            //ui.SetActive(true);
            Debug.Log("CLIENT_ID - "+clientId);
            Debug.Log("Local CLIENT_ID - "+ NetworkManager.Singleton.LocalClientId);
            Debug.Log("Server CLIENT_ID - "+ NetworkManager.ServerClientId);
            Debug.Log("gameobject- "+ gameObject);
            Show();
        }else if(clientId == NetworkManager.ServerClientId){
            Show();
            //ui.SetActive(true);
        }
        
    }

    private void Hide(){
        gameObject.SetActive(false);
    
    }

    private void Show(){
        gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
