using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MemberSelectScreenDisplay : MonoBehaviour
{

	public GameObject selectPrefab;
	public List<GameObject> prefabList = new List<GameObject> ();
	public Text dialogue;
	public Transform selectList;
	public ScrollRect scrollRect;
	public bool show = false;
	public int maxSelection;
	public List<SlotInfo> selectedMembers = new List<SlotInfo> ();
	public Button selectButton;


	
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
		if (selectedMembers.Count > 0) {
			selectButton.interactable = true;
		} else {
			selectButton.interactable = false;
		}
	}

	public void UpdateText (List<Member> availablemembers, string description, int maxselection, int id=0)
	{
		dialogue.text = description;
		selectedMembers.Clear ();
		for (int i=0; i<availablemembers.Count; i++) {
			prefabList.GeneratePrefab(i,selectPrefab,"Member",selectList);
			prefabList [i].GetComponent<SlotInfo> ().FillSlotWithMember (availablemembers [i]);
			prefabList [i].GetComponent<SlotInfo> ().ResetSelection ();
		}
		if (availablemembers.Count < prefabList.Count) {
			for (int i=availablemembers.Count; i<prefabList.Count; i++) {
				prefabList [i].SetActive (false);
			}
		}
		selectList.SetSize (scrollRect, availablemembers.Count, 60);
		maxSelection = maxselection;
	}
	
	public void SelectMember (SlotInfo slot)
	{
		if (selectedMembers.Contains (slot)) {
			slot.Select ();
			selectedMembers.Remove (slot);
		} else if (selectedMembers.Count < maxSelection) {
			slot.Select ();
			selectedMembers.Add (slot);
		}
	}

	public int ShowFirstBuyer ()
	{
		for (int i=0; i<prefabList.Count; i++) {
			if (prefabList [i].GetComponent<Image> ().IsActive ()) {
				return prefabList [i].GetComponent<SlotInfo> ().id;
			}
		}
		return 999;
	}
}
