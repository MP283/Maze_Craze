 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.CloudSave;
using System.Threading.Tasks;

public class FinishLevel : MonoBehaviour
{
    public AudioSource finishSound;
    public Animator anim;

    public static bool checkpointReached = false;
    public int LevelAssign;

    // Start is called before the first frame update
    public async void Start()
    {
        FinishLevelMultiPlayer.checkpointReached = false;
        checkpointReached = false;
        //finishSound = GetComponent<AudioSource>();

        await UnityServices.InitializeAsync();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!checkpointReached){
            finishSound.Play();
            anim.SetBool("HasReachedCheckPoint",true);
            checkpointReached = true;
            changePlayerLevelInCloud();

            Invoke("NextLevel", 2.0f);
        }
    }

    public async void changePlayerLevelInCloud(){

        string CloudLevel = await CloudSaveScript.Instance.LoadData("Level");
        if(LevelAssign.ToString() == CloudLevel){
            var data = new Dictionary<string, object>{{"Level", LevelAssign + 1}}; 
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }

        

        /*
        Dictionary<string,string> serverData;
        // Load data asynchronously
        serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "Level" });

        // Check if "Level" exists in the dictionary
        if (serverData.ContainsKey("Level"))
        {
            try
            {
                // Parse the level value and increment it
                int playerLevel = int.Parse(serverData["Level"]) + 1;
        
                // Update the dictionary
                serverData["Level"] = playerLevel.ToString();

                
                var data = new Dictionary<string, object>{{"Level",serverData["Level"]}}; 
		        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        
            }
            catch (RequestFailedException ex)
            {
                // Handle parsing errors (e.g., log the error)
                Debug.Log($"Error parsing level: {ex.Message}");
            }
        }
        else
        {
            // Handle the case where "Level" is not present in the dictionary
            Debug.Log("Key 'Level' not found in server data.");
        }*/
  
    }

    private void NextLevel(){
        if(SceneManager.GetActiveScene().buildIndex == 10){
            SceneManager.LoadScene("Main_menu");
        }else{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    
}
