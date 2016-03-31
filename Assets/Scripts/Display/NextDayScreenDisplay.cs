using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NextDayScreenDisplay : MonoBehaviour {
	public Transform scrollList;
	public GameObject prefab;
	public ScrollRect scrollRect;
	public List<GameObject> prefabList;
	public Text title;
	public bool show;
	public Text longDescription;
	public SlotInfo lastSelected;

	// Update is called once per frame
	void Update () {
		if (show) {
			GetComponent<CanvasGroup> ().alpha = 1;
			GetComponent<CanvasGroup> ().blocksRaycasts = true;
		} else {
			GetComponent<CanvasGroup> ().alpha = 0;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
	}
	public void UpdateText(int day,int month, int year, List<Task> tasks, List<Character> characters)
	{
		longDescription.text="";
		title.text=string.Format(Database.strings.GetString("Date"),day.ToString(),Database.strings.monthNames[month],year.ToString());
		if (tasks==null)
		{
			tasks=new List<Task>();
		}
		int count=1;
		if (tasks.Count+characters.Count>1){
			count=tasks.Count+characters.Count;
		}
		if((Database.myGuild.paidMaintenance||Database.myGuild.levelUp)&&tasks.Count+characters.Count>0){
			count +=1;
		}
		for (int i=0; i<count; i++) {
			prefabList.GeneratePrefab(i,prefab,"Log",scrollList);
			if(i<tasks.Count){
				prefabList [i].GetComponent<SlotInfo>().TaskLog(tasks[i]);
			} else if (i-tasks.Count<characters.Count){
				prefabList [i].GetComponent<SlotInfo>().CharacterLog(characters[i-tasks.Count]);

			} else if(Database.myGuild.paidMaintenance||Database.myGuild.levelUp){
				prefabList [i].GetComponent<SlotInfo>().GuildLog(Database.myGuild);
			} else {
				prefabList [i].GetComponent<SlotInfo>().TaskLog(null);
			}
		}
		if (count < prefabList.Count) {
			for (int i=count+characters.Count; i<prefabList.Count; i++) {
				prefabList [i].SetActive (false);
			}
		}
		SelectSlot(null);
		scrollList.SetSize(scrollRect,count, 128);
	}
	
	void ResetTransform(GameObject slot)
	{
		slot.transform.localPosition=new Vector3(0,0,0);
		slot.transform.localScale=new Vector3(1,1,1);
	}

	public void SelectSlot(SlotInfo slot){
		if (lastSelected!=null){
			lastSelected.Select();
			if (lastSelected!=slot)
			{
				if (slot!=null){
					slot.Select();
				}
				lastSelected=slot;
			} else{
				lastSelected=null;
			}

		} else {
			if (slot!=null){
				slot.Select();
			}
			lastSelected=slot;
		}
		if (lastSelected!=null){
			longDescription.text=lastSelected.longDescription;
		} else{
			longDescription.text="";
		}
		longDescription.GetComponent<RectTransform> ().offsetMax = new Vector2 (longDescription.GetComponent<RectTransform> ().offsetMax.x, 0);
	}
}
