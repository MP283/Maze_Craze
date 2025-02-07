using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Unity.Services.CloudSave;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Services.Core;

public class Main_menu : MonoBehaviour
{
	public TMP_Text FirstNameText,LastNameText,UserNameText;

	public AudioMixer AudioMix;

	public string FIRSTNAME,LASTNAME,USERNAME;
	public string[] KEYARRAY = {"FirstName", "LastName", "UserName"};

    public void GoToLevel(int lvl){
        SceneManager.LoadSceneAsync(lvl);
    }

	public void GoToSinglePlayer(){
        SceneManager.LoadSceneAsync("Levels");
    }

	public void GoToMultiPlayer(){
        SceneManager.LoadSceneAsync("Multiplayer_menu");
    }

	public void InvokeQuitGame(){
		Invoke("QuitGame", 2.0f);
	}

    public void QuitGame(){
        Application.Quit();
    }

	public void Start(){

		LoadData();
	}


    public async void LoadData(){
		
		await UnityServices.InitializeAsync();

		Dictionary<string,string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> {"FirstName","LastName","UserName"});

		if(serverData.ContainsKey("FirstName")){

			FirstNameText.text = serverData["FirstName"];
			LastNameText.text =  serverData["LastName"];
			UserNameText.text = serverData["UserName"];
		}
	}
}
