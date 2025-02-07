using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// [RequireComponent(typeof(Rigidbody 2D), typeof(SphereCollider))]

public class playerMovement : NetworkBehaviour
{

    public static playerMovement Instance { get; private set; }

    

    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private FixedJoystick fixedJoystick;
    [SerializeField] private float moveSpeed;
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject MazeAssistUI;
    private bool JoystickConfirm = false;
    [SerializeField] private PlayerVisual playerVisual;

    public int MAP_NUMBER;

    //FinishLevel finishLevel = new FinishLevel(); // or get it from somewhere

    
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        
    }

    private void Start(){
        
		MAP_NUMBER = LobbyManager.Map_number;

        PlayerData playerData = MultiplayerMainMenu.Instance.GetPlayerDataFromClientId(OwnerClientId);
        playerVisual.SetPlayerColor(MultiplayerMainMenu.Instance.GetPlayerColor(playerData.colorId));
        if(!IsOwner){
            Destroy(mainCamera);
            Destroy(MazeAssistUI);
            return;
        }
    }

    public Vector3 GetPlayerPosition(){
        if(!IsOwner){
            return default;
        }

        return this.transform.position; 
    }

    private void FixedUpdate(){

        if(!JoystickConfirm){
            GameObject joystickObject = GameObject.Find("Fixed Joystick");
            fixedJoystick = joystickObject.GetComponent<FixedJoystick>();
            if(fixedJoystick != null){
                JoystickConfirm = true;

                if(MAP_NUMBER == 1){

                    if(IsHost){
                        this.transform.position = new Vector3(42.8f, 7.9f, -1f);
                    }else{
                        this.transform.position = new Vector3(-11.0f, -19.6f, -1f);
                    }

                }else if(MAP_NUMBER == 2){
                    if(IsHost){
                        this.transform.position = new Vector3(14.65f, 21.5f, -1f);
                    }else{
                        this.transform.position = new Vector3(14.65f, -22.51f, -1f);
                    }
                }
                
            }
        }
        
        if(!IsOwner){
            Destroy(mainCamera);
            Destroy(MazeAssistUI);
            return;
        }
        
        bool checkpoint = FinishLevel.checkpointReached;
        bool checkpoint2 = FinishLevelMultiPlayer.checkpointReached;
        //Debug.Log(""+ checkpoint);
        if(!checkpoint && !checkpoint2){
            rigidbody.velocity = new Vector2(fixedJoystick.Horizontal * moveSpeed, fixedJoystick.Vertical * moveSpeed);
        }else{
            rigidbody.bodyType = RigidbodyType2D.Static;
        }
        

    }
}
