using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using UnityEngine.UI;

public class MazeAssist : NetworkBehaviour
{

    public GameObject MazeAssistOriginal,Player;
    //public GameObject MazeAssistButton;
    public int num;
    public TMP_Text MazeAssistCounter;
    // Start is called before the first frame update
    void Start()
    {     
        GameObject.Find("GameAssistButton").GetComponent<Button>().onClick.AddListener(() => {
            GameAssist();
        });

        GameObject.Find("GameAssistCounter").GetComponent<TMP_Text>().text = "Game Assist: "+ num.ToString();   
        
        if(!IsOwner){
            //Destroy(Player);
            return;
        }
        
    }

    public void GameAssist(){
        
        if(num > 0){

            GameObject MazeAssistClone = Instantiate(MazeAssistOriginal);
            
            MazeAssistClone.transform.position = Player.transform.position;
        
            num = num - 1;
            GameObject.Find("GameAssistCounter").GetComponent<TMP_Text>().text = "Game Assist: "+ num.ToString();
        }
    }
}

/*
if(!IsOwner){
            return;
        }
if(Player == null){
                MazeAssistClone.transform.position = playerMovement.Instance.GetPlayerPosition();
            }else{
            }
*/
