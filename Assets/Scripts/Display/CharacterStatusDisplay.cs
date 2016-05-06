using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterStatusDisplay : MonoBehaviour {

	public Character character;
	public Text characterName;
	public Text characterHealth;
	public Text characterMana;
	public Transform activeParent;
	public Transform inactiveParent;
	public bool show;
	private Character blankChar=new Character();
	public Button button;
	public Image buttonImage;

	void Update () {
		if (show) {
			characterHealth.text = character.totalStats ["CurrentHealth"].ToString () + "/" + character.totalStats ["MaxHealth"].ToString ();
			characterMana.text = character.totalStats ["CurrentMana"].ToString () + "/" + character.totalStats ["MaxMana"].ToString ();
			if (character.isEnemy && Database.game.adventureScreen.phase == "Choose Enemy" || !character.isEnemy && Database.game.adventureScreen.phase == "Choose Ally") {
				button.interactable = true;
				buttonImage.enabled = true;
				activeParent.GetComponent<CanvasGroup> ().blocksRaycasts = true;
				activeParent.GetComponent<CanvasGroup> ().interactable = true;
			} else {
				button.interactable = false;
				buttonImage.enabled = false;
				activeParent.GetComponent<CanvasGroup> ().blocksRaycasts = false;
				activeParent.GetComponent<CanvasGroup> ().interactable = false;
			}
		} else if (transform.parent == activeParent) {
			transform.SetParent (inactiveParent);
		}

		
	}
	public void SetCharacter(Character character){
		if (transform.parent == inactiveParent) {
			transform.SetParent (activeParent);
			characterName.text = character.nickname;
		}
		this.character = character;
		show = true;
		UpdateCharacter ();
	}

	public void UpdateCharacter(){
		if (show &&character.totalStats ["CurrentHealth"] <= 0) {
			show=false;
			character = blankChar;
		}
	}
	public void SelectCharacter(){
		Database.game.adventureScreen.SelectCharacter (this);
	}
}
