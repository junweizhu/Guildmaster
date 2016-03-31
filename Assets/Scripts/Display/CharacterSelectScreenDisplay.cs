using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectScreenDisplay : MonoBehaviour
{

	public GameObject selectPrefab;
	public List<GameObject> prefabList = new List<GameObject> ();
	public Text dialogue;
	public Transform selectList;
	public ScrollRect scrollRect;
	public bool show = false;
	public int maxSelection;
	public List<SlotInfo> selectedCharacters = new List<SlotInfo> ();
	public Button selectButton;


	
	// Update is called once per frame
	void Update ()
	{
		GetComponent<CanvasGroup>().SetShow(show);
		if (selectedCharacters.Count > 0) {
			selectButton.interactable = true;
		} else {
			selectButton.interactable = false;
		}

	}

	public void UpdateText (List<Character> availablecharacters, string description, int maxselection)
	{
		dialogue.text = description;
		selectedCharacters.Clear ();
		int count=availablecharacters.Count;
		if (availablecharacters.Count<prefabList.Count){
			count=prefabList.Count;
		}
		for (int i=0; i<count; i++) {
			prefabList.GeneratePrefab(i,selectPrefab,"Character",selectList);
			if (i<availablecharacters.Count){
				prefabList [i].GetComponent<SlotInfo> ().FillSlotWithCharacter (availablecharacters [i]);
				prefabList [i].GetComponent<SlotInfo> ().ResetSelection ();
			} else{
				prefabList [i].SetActive (false);
			}
		}
		selectList.SetSize (scrollRect, availablecharacters.Count, 64);
		maxSelection = maxselection;
	}
	
	public void SelectCharacter (SlotInfo slot)
	{
		if (selectedCharacters.Contains (slot)) {
			slot.Select ();
			selectedCharacters.Remove (slot);
		} else if (selectedCharacters.Count < maxSelection) {
			slot.Select ();
			selectedCharacters.Add (slot);
		}
	}
}
