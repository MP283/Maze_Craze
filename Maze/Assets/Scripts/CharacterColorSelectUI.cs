using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CharacterColorSelectUI : MonoBehaviour
{
	[SerializeField] private int colorId;
	[SerializeField] private Image image;
	[SerializeField] private GameObject selectedGameObject;

	private void Start(){
		image.color = MultiplayerMainMenu.Instance.GetPlayerColor(colorId);
		MultiplayerMainMenu.Instance.OnPlayerDataNetworkListChanged += MultiplayerMainMenu_OnPlayerDataNetworkListChanged;
		UpdateIsSelected();
	}
	
	private void MultiplayerMainMenu_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e) {
        UpdateIsSelected();
    }

	private void UpdateIsSelected(){
		if(MultiplayerMainMenu.Instance.GetPlayerData().colorId == colorId){
			 selectedGameObject.SetActive(true);
		}else{
			 selectedGameObject.SetActive(false);
		}
	}

	public void ChangePlayerColorButton(){
		MultiplayerMainMenu.Instance.ChangePlayerColor(colorId);
	}

	private void onDestroy(){
		
		MultiplayerMainMenu.Instance.OnPlayerDataNetworkListChanged -= MultiplayerMainMenu_OnPlayerDataNetworkListChanged;
	}
}
