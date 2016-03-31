using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueScreenDisplay : MonoBehaviour
{
	public List<Dialogue> dialogueDisplay = new List<Dialogue> ();
	public int eventId;
	public int dialogueIndex;
	public Text characterName;
	public Text dialogue;
	public int eventTrigger;
	public List<string> names;
	public bool show;
	public string stringToDialogue = "";
	public WaitForSeconds timePerLetter = new WaitForSeconds (0.05f);

	// Update is called once per frame
	void Update ()
	{
		GetComponent<CanvasGroup> ().SetShow (show);
	}

	public void ShowDialogue (GameEvent gameevent, List<int> names)
	{
		//Debug.Log (gameevent.dialogue);
		eventId = gameevent.id;
		dialogueDisplay = Database.strings.GetDialogue (gameevent.dialogue);
		eventTrigger = gameevent.eventTrigger;
		dialogueIndex = 0;
		UpdateText ();
		if (names != null) {
			this.names = Database.characters.GetCharacterNames (names);
		}
		show = true;
	}

	public void ShowDialogue (int guildexpGain,int money, Dictionary<int,int> items=null, Dictionary<int,int>memberexpGain=null,Dictionary<int,int> quest=null,List<int>area=null,List<int>newmember=null)
	{
		int count = 0;
		if (items != null) {
			foreach (KeyValuePair<int,int> item in items) {
				dialogueDisplay.Add (new Dialogue ("Gain", count, Database.strings.GetString ("GainedItem"), new List<int> (){0,item.Value,item.Key}, new List<string> (){"","Number","Item"}));
				count++;
			}
		}
		if (memberexpGain != null) {
			foreach (KeyValuePair<int,int> member in memberexpGain) {
				dialogueDisplay.Add (new Dialogue ("Gain", count, Database.strings.GetString ("GainedExp"), new List<int> (){0,member.Key,member.Value}, new List<string> (){"","Character","Number"}));
				count++;
			}
		}
		if (guildexpGain > 0) {
			dialogueDisplay.Add (new Dialogue ("Gain", count, Database.strings.GetString ("GuildGainedExp"), new List<int> (){0,guildexpGain}, new List<string> (){"","Number"}));
			count++;
		}
		if (money > 0) {
			dialogueDisplay.Add (new Dialogue ("Gain", count, Database.strings.GetString ("GuildGainedMoney"), new List<int> (){0,money}, new List<string> (){"","Number"}));
			count++;
		}
		if (quest != null) {
			foreach (KeyValuePair<int,int> newquest in quest) {
				dialogueDisplay.Add (new Dialogue ("Gain", count, Database.strings.GetString ("GuildGainedQuest"), new List<int> (){0,newquest.Key}, new List<string> (){"","Quest"}));
				count++;
			}
		}
		if (area!=null){
			for (int i=0; i<area.Count;i++){
				dialogueDisplay.Add (new Dialogue ("Gain", count, Database.strings.GetString ("GuildGainedArea"), new List<int> (){0,area[i]}, new List<string> (){"","Area"}));
				count++;
			}
		}
		if (newmember!=null){
			for (int i=0; i<newmember.Count;i++){
				dialogueDisplay.Add (new Dialogue ("Gain", count, Database.strings.GetString ("GuildGainedRecruit"), new List<int> (){0,newmember[i]}, new List<string> (){"","Character"}));
				count++;
			}
		}
		dialogueIndex = 0;
		UpdateText ();
		show = true;
		Debug.Log("NewText");
	}

	public void UpdateText ()
	{
		if (dialogueDisplay.Count > 0) {
			foreach (Dialogue dialogueText in dialogueDisplay) {
				if (dialogueText.order == dialogueIndex) {
					characterName.text = dialogueText.GetSpeakerName (names);
					stringToDialogue = dialogueText.GetText (names);
					StartCoroutine (AnimateText (stringToDialogue));
					return;
				}
			}
		} else {
			show = false;
		}
	}

	public void Press ()
	{
		if (stringToDialogue != dialogue.text) {
			StopAllCoroutines ();
			dialogue.text = stringToDialogue;	
			if (dialogueDisplay.Count == dialogueIndex + 1) {
				if (Database.events.GetEvent (eventId).enterName && dialogueDisplay.Count > dialogueIndex) {
					ShowTextInputScreen ();
				}
			}
		} else if (dialogueDisplay.Count > dialogueIndex + 1) {
			dialogueIndex++;
			UpdateText ();
		} else {
			GameEvent gameevent = Database.events.GetEvent (eventId);
			if (Database.events.GetEvent (eventId).enterName) {
				if (gameevent.changeNameType == "Guild") {
					Database.guilds.FindGuild (gameevent.changeNameId).name = Database.game.textInputScreen.textInput.text;
				} else {
					Database.characters.GetCharacter (gameevent.changeNameId).name = Database.game.textInputScreen.textInput.text;
				}
			}
			Database.events.GetTrigger (eventTrigger).Activate ();
			Database.game.DisplayScreen(Database.game.screenDisplay,Database.game.displayScreen[Database.game.screenDisplay]);
			dialogueDisplay.Clear ();
			dialogue.text="";
			stringToDialogue="";
			show = false;
			if (!gameevent.finished){
				gameevent.FinishEvent();
			}
		}
	}

	public void ShowTextInputScreen ()
	{
		dialogueIndex++;
		GameEvent gameevent = Database.events.GetEvent (eventId);
		GameObject.FindObjectOfType<GameManager> ().InputText ("Dialogue", gameevent.changeNameId, gameevent.changeNameType);
	}

	IEnumerator AnimateText (string strComplete)
	{
		int i = 0;
		dialogue.text = "";
		while (i < strComplete.Length) {
			dialogue.text += strComplete [i++];
			yield return timePerLetter;
		}
		if (dialogueDisplay.Count == dialogueIndex + 1) {
			if (Database.events.GetEvent (eventId).enterName && dialogueDisplay.Count > dialogueIndex) {
				ShowTextInputScreen ();
			}
		}
	}
}
