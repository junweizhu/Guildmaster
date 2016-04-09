using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterStatusDisplay : MonoBehaviour {

	private Character character;
	public Text characterName;
	public Text characterHealth;
	public Text characterMana;
	public Transform activeParent;
	public Transform inactiveParent;

	void Update () {
		if (character != null) {
			characterHealth.text = character.totalStats ["CurrentHealth"].ToString () + "/" + character.totalStats ["MaxHealth"].ToString ();
			characterMana.text = character.totalStats ["CurrentMana"].ToString () + "/" + character.totalStats ["MaxMana"].ToString ();
		} else if (transform.parent == activeParent) {
			transform.SetParent (inactiveParent);
		}
	}
	public void SetCharacter(Character character){
		if (transform.parent == inactiveParent) {
			transform.SetParent (activeParent);
			characterName.text = character.name;
		}
	}
}
