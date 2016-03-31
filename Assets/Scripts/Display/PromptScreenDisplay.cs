using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PromptScreenDisplay : MonoBehaviour
{

	public Text description;
	public bool show;
	public string action;
	public InputField textInput;
	public Character character;
	public Guild guild;
	public GameObject cancelButton;



	void Update ()
	{
		GetComponent<CanvasGroup> ().SetShow (show);
	}

	public void Prompt (string action)
	{

		this.action = action;
		if (action=="Recruit"||action=="Dialogue"){
			description.text = string.Format (Database.strings.GetString ("Recruit"),GameObject.FindObjectOfType<GameManager> ().textInputScreen.textInput.text);
		} else{
			description.text = Database.strings.GetString ("Prompt");
		}
		show = true;
	}

	public void InputText(string action,int id,string type="Character", bool forced=false){
		this.action = action;
		description.text = Database.strings.GetString ("Prompt");

		textInput.text="";
		if (type=="Character"){
			character=Database.characters.GetCharacter(id);
			textInput.text=character.name;
		} else if (type=="Guild"){
			guild=Database.guilds.FindGuild(id);
			textInput.text=guild.name;
		}
		if (forced){
			cancelButton.SetActive(false);
		} else{
			cancelButton.SetActive(false);
		}

		show = true;
	}

	public void Confirm ()
	{
		Close ();
		if (action == "StartQuest") {
			GameObject.FindObjectOfType<GameManager> ().StartQuest ();
		} else if (action == "Adventure") {
			GameObject.FindObjectOfType<GameManager> ().GoOnAdventure ();
		} else if (action == "Shop") {
			GameObject.FindObjectOfType<GameManager> ().BuyOrSell ();
		} else if (action == "Socialize") {
			GameObject.FindObjectOfType<GameManager> ().GoToTavern ();
		} else if (action=="Recruit"||action=="Dialogue"){
			if (textInput!=null){
				GameObject.FindObjectOfType<GameManager> ().promptScreen.Prompt(action);
				show=true;
			} else{
				GameObject.FindObjectOfType<GameManager> ().textInputScreen.Close();
				if (action=="Recruit"){
					GameObject.FindObjectOfType<GameManager> ().RecruitCharacter();
				} else{
					GameObject.FindObjectOfType<GameManager> ().dialogueScreen.Press();
				}
			}
		} else if(action=="DeleteItem"){
			GameObject.FindObjectOfType<WarehouseScreenDisplay>().DeleteItem();
		}
	}
	public void Close(){
		show=false;
	}
}
