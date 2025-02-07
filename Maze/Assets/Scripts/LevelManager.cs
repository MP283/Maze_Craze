using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.CloudSave;
using UnityEngine.UI;
using Unity.Services.Core;

public class LevelManager : MonoBehaviour
{

    private string levelToLoad;
	public int num = 0;

    public Button Level_1,Level_2,Level_3,Level_4,Level_5;

    private async void Start(){
        
		await UnityServices.InitializeAsync();
    }
    private async void FixedUpdate(){

		Dictionary<string,string> serverData;
		if(num == 0){
			serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> {"Level"});

			if(serverData.ContainsKey("Level") && num == 0){
                string PlayerLevel = serverData["Level"]; 
				
                switch(PlayerLevel){
                    case "2":
                        Level_2.interactable = true;
                        break;
                    case "3":
                        Level_2.interactable = true;
                        Level_3.interactable = true;
                        break;
                    case "4":
                        Level_2.interactable = true;
                        Level_3.interactable = true;
                        Level_4.interactable = true;
                        break;
                    case "5":
                        Level_2.interactable = true;
                        Level_3.interactable = true;
                        Level_4.interactable = true;
                        Level_5.interactable = true;
                        break;
                    case "6":
                        Level_2.interactable = true;
                        Level_3.interactable = true;
                        Level_4.interactable = true;
                        Level_5.interactable = true;
                        break;
                    default:
                        Debug.Log("ERROR");
                        break;
                }

			}
			else{
				Debug.Log("YOU ARE NOT SIGNED IN");
			}
            num =1;
            
		}
    
    }

    public void PlayGame(string level){

        levelToLoad = level;
        Invoke("InvokePlayGame", 1.0f);
    }

    private void InvokePlayGame(){
        SceneManager.LoadSceneAsync(levelToLoad);
    }
}
