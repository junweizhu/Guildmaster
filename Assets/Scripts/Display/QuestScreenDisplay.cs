using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestScreenDisplay : MonoBehaviour
{
	public GameObject questslotPrefab;
	public Transform questslotList;
	public List<GameObject> slotPrefabList = new List<GameObject> ();
	public int questCount;
	public Text slotSize;
	private bool refresh;
	private CanvasGroup canvasGroup;
	void Start(){
		canvasGroup=GetComponent<CanvasGroup>();
	}
	void Update(){
		if(canvasGroup.alpha!=1){
			refresh=true;
		} else if(refresh){
			refresh=false;
			UpdateText();
		}
	}

	public void UpdateText ()
	{

		Guild guild = Database.myGuild;

		List<Quest> questlist = guild.questBoard.questList;
		if (Database.game.screenDisplay == "Request") {
			questlist = Database.quests.AvailableQuests ();
		}

		questCount = questlist.Count;
		for (int i=0; i<questCount; i++) {
			if (questlist [i].name == null) {
				if (slotPrefabList.Count > (i + 1)) {
					slotPrefabList [i].SetActive (false);
				}
				questCount = i;
				break;
			}
			slotPrefabList.GeneratePrefab (i, questslotPrefab, questslotPrefab.name, questslotList);
			slotPrefabList [i].GetComponent<SlotInfo> ().FillSlotWithQuest (i + 1, questlist [i]);
			if (Database.game.screenDisplay == "Request") {

				slotPrefabList[i].GetComponent<SlotInfo>().recruitRequestButton.interactable=guild.questBoard.AcceptedQuestCount()<Database.upgrades.GetUpgrade(2).MaxSize(Database.myGuild.upgradelist[2]);
			}
		}
		if (questCount < slotPrefabList.Count) {
			for (int i=questCount; i<slotPrefabList.Count; i++) {
				slotPrefabList [i].SetActive (false);
			}
		}
		questslotList.transform.SetSize (questCount, 64);
		slotSize.text = questCount.ToString () + "/" + Database.upgrades.GetUpgrade (2).MaxSize (guild.upgradelist [2]);
	}
}
