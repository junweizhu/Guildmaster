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

	// Update is called once per frame
	void Update ()
	{
		GetComponent<CanvasGroup> ().SetShow (show);
	}

	public void ShowDialogue (GameEvent gameevent, List<int> names)
	{
		Debug.Log (gameevent.dialogue);
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

	public void UpdateText ()
	{
		Debug.Log ("Updating text");
		if (dialogueDisplay.Count > 0) {
			Debug.Log ("it has Text"+dialogueIndex);
			foreach (Dialogue dialogueText in dialogueDisplay) {
				if (dialogueText.order == dialogueIndex) {
					characterName.text = dialogueText.GetSpeakerName (names);
					dialogue.text = dialogueText.GetText (names);

					return;
				}
			}
		} else {
			show = false;
		}
	}

	public void Press ()
	{
		if (dialogueDisplay.Count > dialogueIndex + 1) {
			dialogueIndex++;
			UpdateText ();
		} else {
			Database.events.GetTrigger (eventTrigger).Activate ();
			Database.events.GetEvent (eventId).finished = true;
			show = false;
		}
	}
}
