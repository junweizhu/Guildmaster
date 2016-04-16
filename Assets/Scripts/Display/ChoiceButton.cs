using UnityEngine;
using System.Collections;

public class ChoiceButton : MonoBehaviour {
	public int value;

	public void Select(){
		Database.game.choiceScreen.SelectSearch (value);
	}
}
