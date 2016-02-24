using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class QuestStatScreenDisplay : MonoBehaviour
{
	public Text questNumber;
	public Text questName;
	public Text questshortDescription;
	public Text questStatus;
	public Text questDifficulty;
	public Text questMaxParticipants;
	public Text questDuration;
	public CanvasGroup detailsList;
	public Text questDetails;
	public Transform rewardList;
	public GameObject itemPrefab;
	public GameObject textPrefab;
	public List<GameObject> rewardprefList = new List<GameObject> ();
	public Transform characterList;
	public GameObject characterPrefab;
	public List<GameObject> characterprefList = new List<GameObject> ();
	public bool show;
	public string showSub;
	public Quest quest;
	public bool refresh;
	public Button startButton;
	public Button finishButton;
	public Button characterButton;
	public Button acceptButton;
	public Button addCharacterButton;
	public QuestBoard questBoard;
	public int firstId;
	public List<Character> participants;
	public Guild myGuild;
	// Update is called once per frame
	void Update ()
	{

		GetComponent<CanvasGroup>().SetShow(show);
		if (showSub == "reward" && rewardList.GetComponent<CanvasGroup> ().alpha == 0) {
			rewardList.GetComponent<CanvasGroup> ().alpha = 1;
			rewardList.GetComponent<CanvasGroup> ().blocksRaycasts = true;
			rewardList.GetComponent<ScrollRect> ().normalizedPosition = new Vector2 (0, 1);
		} else if (showSub != "reward" && rewardList.GetComponent<CanvasGroup> ().alpha == 1) {
			rewardList.GetComponent<CanvasGroup> ().alpha = 0;
			rewardList.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
		if (showSub == "details" && detailsList.GetComponent<CanvasGroup> ().alpha == 0) {
			detailsList.alpha = 1;
			detailsList.blocksRaycasts = true;
			detailsList.GetComponent<ScrollRect> ().normalizedPosition = new Vector2 (0, 1);
		} else if (showSub != "details" && detailsList.GetComponent<CanvasGroup> ().alpha == 1) {
			detailsList.alpha = 0;
			detailsList.blocksRaycasts = false;
		}
		if (showSub == "character" && characterList.GetComponent<CanvasGroup> ().alpha == 0) {
			characterList.GetComponent<CanvasGroup> ().alpha = 1;
			characterList.GetComponent<CanvasGroup> ().blocksRaycasts = true;
			characterList.GetComponent<ScrollRect> ().normalizedPosition = new Vector2 (0, 1);
		} else if (showSub != "character" && characterList.GetComponent<CanvasGroup> ().alpha == 1) {
			characterList.GetComponent<CanvasGroup> ().alpha = 0;
			characterList.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
		if (refresh) {
			refresh = false;
			showSub = "details";
			questName.text = quest.name;
			questshortDescription.text = quest.shortDescription;
			Debug.Log (quest.longDescription);
			questDetails.text = quest.longDescription;
			questStatus.text = Database.strings.GetString (quest.status);
			questMaxParticipants.text = quest.maxParticipants.ToString ();
			FillReward ();
			FillParticipants (quest.participants);
			if (!quest.accepted) {
				acceptButton.gameObject.SetActive (true);
				finishButton.gameObject.SetActive (false);
				startButton.gameObject.SetActive (false);
				characterButton.interactable = false;
			} else {
				acceptButton.gameObject.SetActive (false);
				if (quest.maxParticipants > 0) {
					startButton.gameObject.SetActive (true);
					finishButton.gameObject.SetActive (false);
				} else {
					startButton.gameObject.SetActive (false);
					finishButton.gameObject.SetActive (true);
				}
				if (participants.Count > 0) {
					characterButton.interactable = true;
				} else {
					characterButton.interactable = false;
				}
			}
			questDuration.text = string.Format (Database.strings.GetString ("Duration"), quest.duration);
			if (quest.duration > 1)
				questDuration.text += "s";
			if (quest.status == "Ongoing" || !quest.accepted) {
				addCharacterButton.interactable = false;
				finishButton.interactable = false;
				startButton.interactable = false;

			} else {
				if (myGuild.CanFinishQuest (int.Parse (questNumber.text) - 1)) {
					finishButton.interactable = true;
				} else {
					finishButton.interactable = false;
				}
				if (quest.maxParticipants > 0) {
					addCharacterButton.interactable = true;
				} else {
					addCharacterButton.interactable = false;
				}
			}
		}
	}

	void FillReward ()
	{
		if (rewardprefList.Count > 0) {
			foreach (GameObject i in rewardprefList) {
				DestroyObject (i);

			}
			rewardprefList = new List<GameObject> ();
		}
		int rewardCount = 0;
		if (quest.expReward.Count > 0) {
			foreach (KeyValuePair<int,int> exp in quest.expReward) {
				string skillname = "";
				if (exp.Key == 99) {
					skillname = "";
				} else {
					skillname = Database.skills.GetSkill (exp.Key).name;
				}
				rewardprefList.Add (GameObject.Instantiate (textPrefab) as GameObject);
				rewardprefList.Last ().GetComponent <Text> ().text = string.Format (Database.strings.GetString ("Experience"), exp.Value.ToString (), skillname);
				rewardprefList.Last ().transform.SetParent (rewardList);
				rewardprefList.Last ().transform.localScale = new Vector3 (1, 1, 1);
				rewardCount++;
			}
		}
		if (quest.moneyReward > 0) {
			rewardprefList.Add (GameObject.Instantiate (textPrefab) as GameObject);
			rewardprefList.Last ().GetComponent <Text> ().text = quest.moneyReward.ToString () + " Gold";
			rewardprefList.Last ().transform.SetParent (rewardList);
			rewardprefList.Last ().transform.localScale = new Vector3 (1, 1, 1);
			rewardCount++;
		}
		if (quest.itemRewards != null) {
			foreach (KeyValuePair<int,int> reward in quest.itemRewards) {
				rewardprefList.Add (GameObject.Instantiate (itemPrefab));
				rewardprefList.Last ().transform.SetParent (rewardList);
				rewardprefList.Last ().transform.localScale = new Vector3 (1, 1, 1);
				rewardprefList.Last ().GetComponent<SlotInfo> ().FillSlotWithReward (Database.items.FindItem (reward.Key), reward.Value);
				rewardCount++;
			}
		}
		if (rewardprefList.Count == 0) {
			rewardprefList.Add (GameObject.Instantiate (textPrefab) as GameObject);
			rewardprefList.Last ().GetComponent <Text> ().text = Database.strings.GetString ("NoRewards");
			rewardprefList.Last ().transform.SetParent (rewardList);
			rewardprefList.Last ().transform.localScale = new Vector3 (1, 1, 1);
			rewardCount++;
		}
		rewardList.SetSize (rewardCount, 64);
	}

	public void FillParticipants (List<Character> list)
	{
		if (quest.maxParticipants > 0) {
			participants = list;
			int count= participants.Count;
			if (participants.Count<characterprefList.Count){
				count=characterprefList.Count;
			}
			for (int i=0; i<count; i++) {
				characterprefList.GeneratePrefab(i,characterPrefab,"Member",characterList);
				if (i<participants.Count){
					characterprefList[i].GetComponent<SlotInfo> ().FillSlotWithCharacter (participants [i]);
				} else{
					characterprefList[i].SetActive(false);
				}
			}
			if (participants.Count > 0) {
				characterButton.interactable = true;
				startButton.interactable = true;
			} else {
				characterButton.interactable = false;
				startButton.interactable = false;
			}
			characterList.SetSize (characterprefList.Count, 64);
		}
	}

	public void SetShow (string display)
	{
		showSub = display;
	}

	public void CloseScreen ()
	{
		show = false;
	}

	public void FinishQuest ()
	{
		GameObject.FindObjectOfType<GameManager> ().FinishQuest (int.Parse (questNumber.text) - 1);
	}
}
