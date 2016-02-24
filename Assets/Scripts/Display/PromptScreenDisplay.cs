using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PromptScreenDisplay : MonoBehaviour
{

	public Text description;
	public bool show;
	public string action;

	void Update ()
	{
		GetComponent<CanvasGroup> ().SetShow (show);
	}

	public void Prompt (string action)
	{

		this.action = action;
		show = true;
		description.text = Database.strings.GetString ("Prompt");
	}

	public void Confirm ()
	{
		if (action == "StartQuest") {
			GameObject.FindObjectOfType<GameManager> ().StartQuest ();
		} else if (action == "Adventure") {
			GameObject.FindObjectOfType<GameManager> ().GoOnAdventure ();
		} else if (action == "Shop") {
			GameObject.FindObjectOfType<GameManager> ().BuyOrSell ();
		} else if (action == "Socialize") {
			GameObject.FindObjectOfType<GameManager> ().GoToTavern ();
		}
		show = false;
	}
	public void Close(){
		show=false;
	}
}
