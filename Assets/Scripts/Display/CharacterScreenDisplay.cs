using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterScreenDisplay : MonoBehaviour {
	public GameObject memberslotPrefab;
	public Transform memberslotList;
	private List<GameObject> slotPrefabList=new List<GameObject>();
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

	public void UpdateText()
	{

		Guild guild=Database.myGuild;
		List<Character> memberlist=guild.characterlist;
		if (Database.game.screenDisplay=="Recruit"){
			memberlist=Database.characters.GetRecruitables();
		}
		int count=memberlist.Count;
		if (memberlist.Count<slotPrefabList.Count){
			count=slotPrefabList.Count;
		}
		for(int i=0; i<count;i++)
		{
			slotPrefabList.GeneratePrefab(i,memberslotPrefab,memberslotPrefab.name,memberslotList);
			if (i<memberlist.Count){
				slotPrefabList[i].GetComponent<SlotInfo>().FillSlotWithCharacter(memberlist[i]);
				if (Database.game.screenDisplay=="Recruit"){
					slotPrefabList[i].GetComponent<SlotInfo>().recruitRequestButton.interactable=Database.myGuild.characterlist.Count<Database.upgrades.GetUpgrade(0).MaxSize(Database.myGuild.upgradelist[0]);
				}
			} else{
				slotPrefabList[i].SetActive(false);
			}
		}
		memberslotList.SetSize(memberlist.Count,64);
		slotSize.text=memberlist.Count.ToString()+"/"+Database.upgrades.GetUpgrade(0).MaxSize(guild.upgradelist[0]);
	}
}
