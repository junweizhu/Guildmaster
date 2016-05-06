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
			description.text = string.Format (Database.strings.GetString ("Recruit"),Database.game.textInputScreen.textInput.text);
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
			if (character.nickname == "" || character.nickname == null) {
				textInput.text = character.name;
			} else {
				textInput.text = character.nickname;
			}
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
			Database.game.StartQuest ();
		} else if (action == "Adventure") {
			Database.game.GoOnAdventure ();
		} else if (action == "Shop") {
			Database.game.BuyOrSell ();
		} else if (action == "Socialize") {
			Database.game.GoToTavern ();
		} else if (action=="Recruit"||action=="Dialogue"){
			if (textInput!=null){
				Database.game.promptScreen.Prompt(action);
				show=true;
			} else{
				Database.game.textInputScreen.Close();
				if (action=="Recruit"){
					Database.game.RecruitCharacter();
				} else{
					Database.game.dialogueScreen.Press();
				}
			}
		} else if(action=="DeleteItem"){
			Database.game.warehouseScreen.DeleteItem();
		}
	}
	public void Close(){
		show=false;
	}
}
