using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using UnityEngine.UI;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Core;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class CloudSaveScript : MonoBehaviour
{
	public TMP_Text statusTxt;
	public TMP_InputField FirstName, LastName, UserName;
	public bool HasLoggedInBool = false;
	public int num = 0;
	public GameObject Menu;


	public static CloudSaveScript Instance { get; private set; }

	private void Awake(){
    
        Instance = this;
	}
	

	public async void SaveData(){

		var data = new Dictionary<string, object>{ {"HasLoggedIn", true},	{"FirstName", FirstName.text},	{"LastName", LastName.text},  {"UserName", UserName.text},{"Level","1"}}; 
		await CloudSaveService.Instance.Data.ForceSaveAsync(data);
						
		SceneManager.LoadSceneAsync("Main_Menu");

	}

	public async void Start(){
			await UnityServices.InitializeAsync();
		
		//LoadData();
	
	}

	/*
	public async void FixedUpdate(){
		string key = "HasLoggedIn";
		Dictionary<string,string> serverData;
		if(UnityServices.State == ServicesInitializationState.Initialized){
			if(num == 0){
				serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> {"HasLoggedIn"});

				if(serverData.ContainsKey("HasLoggedIn") && num == 0){
					if(serverData[key] == "true"){
						SceneManager.LoadSceneAsync(1);
						num = 1;
					}
					else{
						Menu.SetActive(true);
						num = 1;
					}
				}
				else{
					statusTxt.text = "YOU ARE NOT LOOGED IN!";
				}
			}
		}
		
	}
	*/

	public async Task<string> LoadData(string key){

		Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { key });
		string value = null;

		if (serverData.ContainsKey(key))
		{
			value = serverData[key];
		}
		else
		{
			statusTxt.text = "KEY NOT FOUND!";
		}

		return value ?? "Default value"; // Return a default value if key is not found
	}

   
}
