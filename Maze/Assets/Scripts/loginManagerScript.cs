using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.CloudSave;


public class loginManagerScript : MonoBehaviour
{
    public TMP_Text statustxt;
    public CloudSaveScript CloudSaveManager;
    private bool runOnce = false;

    public static loginManagerScript Instance { get; private set; }

    

    async void Start(){

        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
            
        if(UnityServices.State == ServicesInitializationState.Initialized && !runOnce){
            
            await CheckLoginStatus();
            Invoke("CloudSaveHasLoggedInBool", 0);
            
            
            
        }
        
        
        
    }


    public async Task CheckLoginStatus(){

        string result = await LoadData("HasLoggedIn");
        Debug.Log("" + result);
    
        if(result == "true")
        {
            CloudSaveManager.HasLoggedInBool = true;
        }
    }


    private async void CloudSaveHasLoggedInBool(){
        if(CloudSaveManager.HasLoggedInBool){
            await SignInAnonymusoly();
            
        }else{
            CloudSaveManager.Menu.SetActive(true);
        }
    }

    public async Task<string> LoadData(string key){

		Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { key });
		string value = null;

		if (serverData.ContainsKey(key))
		{
			value = serverData[key];
		}
		else
		{
			Debug.Log("gay");
		}

		return value ?? "Default value"; // Return a default value if key is not found
	}

    public async void SignIn(){
        await SignInAnonymusoly();
    }

    async Task SignInAnonymusoly(){

        try
        {
            
            Debug.Log("Sign in anonymously succeeded!");
            CloudSaveManager.HasLoggedInBool = true;
            SceneManager.LoadSceneAsync("Main_menu");
            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); 

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);

            if(AuthenticationService.Instance.IsSignedIn){
                SceneManager.LoadSceneAsync("Main_menu");
            }
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
         }

    }

    

}


