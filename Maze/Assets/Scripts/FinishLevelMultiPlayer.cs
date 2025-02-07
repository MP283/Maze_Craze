using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.CloudSave;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using System;

public class FinishLevelMultiPlayer : NetworkBehaviour
{
    public AudioSource finishSound;
    public Animator anim;

    public static bool checkpointReached = false;

    public static FinishLevelMultiPlayer Instance { get; private set; }

    public event EventHandler OnGameFinished;

    public void Awake(){
        Instance = this;
    }

    public async void Start()
    {
        if(!IsOwner){
            return;
        }
        checkpointReached = false;
        FinishLevel.checkpointReached = false;
        //finishSound = GetComponent<AudioSource>();

        //await UnityServices.InitializeAsync();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsOwner){
            return;
        }
        if(collision.gameObject.name == "CheckPoint" && !checkpointReached){
            
            anim.SetBool("HasReachedCheckPoint",true);
            GameFinishedServerRpc(NetworkManager.Singleton.LocalClientId);
            

        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void GameFinishedServerRpc(ulong clientId) {
        GameFinishedClientRpc(clientId);
        
    }
    
    [ClientRpc]
    private void GameFinishedClientRpc(ulong clientId) {
        checkpointReached = true;
        finishSound.Play();
        GameManager.Instance.GameOverUI.SetActive(true);
        PlayerData playerdata = MultiplayerMainMenu.Instance.GetPlayerDataFromClientId(clientId);
        GameManager.Instance.messageText.text = playerdata.playerUserName + " WON!!";
        if(IsServer){

            //NetworkManager.Singleton.SceneManager.LoadScene("Multiplayer_Level", LoadSceneMode.Single);
        }
    }
    

}
