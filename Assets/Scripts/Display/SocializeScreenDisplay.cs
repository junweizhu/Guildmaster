using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SocializeScreenDisplay : MonoBehaviour {

	public GameObject talkslotPrefab;
	public Transform talkslotList;
	public List<GameObject> slotPrefabList = new List<GameObject> ();
	public int eventCount;
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

		List<GameEvent> eventlist = Database.events.activeSocializeEvents;

		eventCount = eventlist.Count;
		for (int i=0; i<eventCount; i++) {
			if (eventlist [i].name == null) {
				if (slotPrefabList.Count > (i + 1)) {
					slotPrefabList [i].SetActive (false);
				}
				eventCount = i;
				break;
			}
			slotPrefabList.GeneratePrefab (i, talkslotPrefab, talkslotPrefab.name, talkslotList);
			slotPrefabList [i].GetComponent<SlotInfo> ().FillSlotWithTopic (eventlist [i]);
		}
		if (eventCount < slotPrefabList.Count) {
			for (int i=eventCount; i<slotPrefabList.Count; i++) {
				slotPrefabList [i].SetActive (false);
			}
		}
		talkslotList.transform.SetSize (eventCount, 64);
	}

	public void Refresh(){
		refresh = true;
	}
}
