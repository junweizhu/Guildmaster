using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BuyerScreenDisplay : MonoBehaviour
{

	public GameObject buyerPrefab;
	public List<GameObject> prefabList = new List<GameObject> ();
	public Transform buyerList;
	public ScrollRect scrollRect;
	public int buyerId;
	public SlotInfo lastSelected = null;
	public bool show = false;

	
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
	}

	public void UpdateText (List<Member> availablemembers, Skill skill, int id)
	{

		for (int i=0; i<availablemembers.Count; i++) {
				if (i + 1 > prefabList.Count) {
					prefabList.Add (GameObject.Instantiate (buyerPrefab) as GameObject);
					prefabList [i].transform.SetParent (buyerList);
					prefabList [i].GetComponent<SlotInfo> ().ResetTransform ();
				} else if (prefabList [i].activeSelf == false) {
					prefabList [i].SetActive (true);
				}
			prefabList [i].GetComponent<SlotInfo> ().FillSlotWithMember (availablemembers [i], skill);
			if (availablemembers [i].memberId == id) {
					SelectMember (prefabList [id].GetComponent<SlotInfo> ());
				}

		}
		if (availablemembers.Count < prefabList.Count) {
			for (int i=availablemembers.Count; i<prefabList.Count; i++) {
				prefabList [i].SetActive (false);
			}
		}
		buyerList.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, availablemembers.Count * 60);
		if (availablemembers.Count > 4)
			scrollRect.vertical = true;
		else
			scrollRect.vertical = false;
	}

	public void SelectMember (SlotInfo slot)
	{
		if (lastSelected == null) {
			slot.Select ();
			buyerId = slot.id;
			lastSelected = slot;
		}
		if (lastSelected.id != slot.id) {
			lastSelected.Select ();
			buyerId = slot.id;
			slot.Select ();
			lastSelected = slot;
		}
	}

}
