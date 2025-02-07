using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour{

	
    [SerializeField] private SpriteRenderer playerSprite;

	private Material material;

	
	private void Awake(){
		material = new Material(playerSprite.material);
		//material = new Material(Shader.Find("Sprites/Default"));
		playerSprite.material = material;
	}

	
	private void Start(){
		material = new Material(playerSprite.material);
		playerSprite.material = material;
		//SetPlayerColor(MultiplayerMainMenu.Instance.GetPlayerColor(MultiplayerMainMenu.Instance.GetFirstUnusedColorId()));
	}

	public void SetPlayerColor(Color Color){
		material.color = Color;
		Debug.Log("Player color set to: " + material.color);
	}
}
