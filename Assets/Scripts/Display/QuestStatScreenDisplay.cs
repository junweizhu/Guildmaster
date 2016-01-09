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
	public Transform memberList;
	public GameObject memberPrefab;
	public List<GameObject> memberprefList = new List<GameObject> ();
	public bool show;
	public string showSub;
	public Quest quest;
	public bool refresh;
	public ItemDatabase db;
	public Button startButton;
	public Button finishButton;
	public Button memberButton;
	public Button addMemberButton;
	public QuestBoard questBoard;
	public int firstId;
	public List<Member> participants;
	public Guild myGuild;
	// Update is called once per frame
	void Update ()
	{

		if (show) {
			GetComponent<CanvasGroup> ().alpha = 1;
			GetComponent<CanvasGroup> ().blocksRaycasts = true;
		} else {
			GetComponent<CanvasGroup> ().alpha = 0;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
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
		if (showSub == "member" && memberList.GetComponent<CanvasGroup> ().alpha == 0) {
			memberList.GetComponent<CanvasGroup> ().alpha = 1;
			memberList.GetComponent<CanvasGroup> ().blocksRaycasts = true;
			memberList.GetComponent<ScrollRect> ().normalizedPosition = new Vector2 (0, 1);
		} else if (showSub != "member" && memberList.GetComponent<CanvasGroup> ().alpha == 1) {
			memberList.GetComponent<CanvasGroup> ().alpha = 0;
			memberList.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
		if (refresh) {
			refresh = false;
			showSub = "details";
			questName.text = quest.name;
			questshortDescription.text = quest.shortDescription;
			questDetails.text = quest.longDescription;
			questStatus.text=questBoard.questStatus [int.Parse (questNumber.text) - 1];
			questMaxParticipants.text = quest.maxParticipants.ToString ();
			FillReward ();
			FillParticipants (questBoard.questParticipants [int.Parse (questNumber.text) - 1]);
			if (quest.maxParticipants>0){
				startButton.gameObject.SetActive(true);
				finishButton.gameObject.SetActive(false);
			} else{
				startButton.gameObject.SetActive(false);
				finishButton.gameObject.SetActive(true);
			}
			if (participants.Count> 0) {
				memberButton.interactable = true;
			} else {
				memberButton.interactable = false;

			}
			if (questBoard.questStatus [int.Parse (questNumber.text) - 1]=="Ongoing") {
				questDuration.text = questBoard.questDays [int.Parse (questNumber.text) - 1].ToString ()+ " Day";
				if (questBoard.questDays [int.Parse (questNumber.text) - 1]>1)
					questDuration.text +="s";
				addMemberButton.interactable = false;
				finishButton.interactable = false;
				startButton.interactable = false;
			} else {
				questDuration.text = quest.duration.ToString ()+ " Day";
				if (quest.duration>1){
					questDuration.text +="s";
				}
				if (myGuild.CanFinishQuest(int.Parse(questNumber.text)-1)){
					finishButton.interactable= true;
				}
				else{
					finishButton.interactable=false;
				}
				if (quest.maxParticipants > 0) {
					addMemberButton.interactable = true;
				} else {
					addMemberButton.interactable = false;
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
		int rewardCount=0;
		if (quest.expReward.Count > 0) {
			foreach (KeyValuePair<int,int> exp in quest.expReward)
			{
				string skillname="";
				if (exp.Key==99){
					skillname=" Experience";
				}
				else{
					skillname=GameObject.FindObjectOfType<SkillDatabase>().GetSkill(exp.Key).name+" Experience";
				}
			rewardprefList.Add (GameObject.Instantiate (textPrefab) as GameObject);
			rewardprefList.Last ().GetComponent <Text> ().text = exp.Value.ToString () +" "+skillname;
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
		db = GameObject.Find ("Database").GetComponent<ItemDatabase> ();
		if (quest.itemRewards != null) {
			foreach (KeyValuePair<int,int> reward in quest.itemRewards) {
				rewardprefList.Add (GameObject.Instantiate (itemPrefab));
				rewardprefList.Last ().transform.SetParent (rewardList);
				rewardprefList.Last ().transform.localScale = new Vector3 (1, 1, 1);
				rewardprefList.Last ().GetComponent<SlotInfo> ().FillSlotWithReward (db.FindItem (reward.Key), reward.Value);
				rewardCount++;
			}
		}
		if (rewardprefList.Count == 0) {
			rewardprefList.Add (GameObject.Instantiate (textPrefab) as GameObject);
			rewardprefList.Last ().GetComponent <Text> ().text = "There are no rewards given in this quest.";
			rewardprefList.Last ().transform.SetParent (rewardList);
			rewardprefList.Last ().transform.localScale = new Vector3 (1, 1, 1);
			rewardCount++;
		}
		rewardList.SetSize(rewardCount,60);
	}

	public void FillParticipants (List<Member> list)
	{
		if (memberprefList.Count > 0) {
			foreach (GameObject i in memberprefList) {
				DestroyObject (i);
			}
			memberprefList = new List<GameObject> ();
		}
		if (quest.maxParticipants >0) {
			participants = list;
			for (int i=0; i<participants.Count; i++) {
				memberprefList.Add (GameObject.Instantiate (memberPrefab));
				memberprefList.Last ().transform.SetParent (memberList);
				memberprefList.Last ().transform.localScale = new Vector3 (1, 1, 1);
				memberprefList.Last ().GetComponent<SlotInfo> ().FillSlotWithMember (participants [i]);
			}
			if (participants.Count>0){
				memberButton.interactable = true;
				startButton.interactable = true;
			}
			else {
				memberButton.interactable = false;
				startButton.interactable = false;
			}
			memberList.SetSize(memberprefList.Count,60);
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
	public void FinishQuest()
	{
		GameObject.FindObjectOfType<GameManager>().FinishQuest(int.Parse(questNumber.text)-1);
	}
}
