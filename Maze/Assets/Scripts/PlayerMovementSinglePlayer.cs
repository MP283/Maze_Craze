using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSinglePlayer : MonoBehaviour
{


    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private FixedJoystick fixedJoystick;
    [SerializeField] private float moveSpeed;
    private bool JoystickConfirm = false;

    private void FixedUpdate(){

        if(!JoystickConfirm){
            GameObject joystickObject = GameObject.Find("Fixed Joystick");
            fixedJoystick = joystickObject.GetComponent<FixedJoystick>();
            if(fixedJoystick != null){
                JoystickConfirm = true;
                //this.transform.position = new Vector3(42.8f, 7.9f, -1f);
                
            }
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
