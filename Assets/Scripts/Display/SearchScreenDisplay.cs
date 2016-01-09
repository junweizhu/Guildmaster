using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SearchScreenDisplay : MonoBehaviour {
	public GameObject selectPrefab;
	public Transform selectList;
	public ScrollRect scrollRect;
	public Text title;
	private List<GameObject> prefabList=new List<GameObject>();
	public List<SlotInfo> recruitChoices= new List<SlotInfo>();
	public List<SlotInfo> questChoices= new List<SlotInfo>();
	public List<SlotInfo> adventureChoices= new List<SlotInfo>();
	public SlotInfo searchType;
	public Text dialogue;
	public Button searchButton;
	public List<SlotInfo> selectedMembers=new List<SlotInfo>();
	public int maxSelection;
	public bool show = false;
	public GameObject searchRecruitList;
	public GameObject searchQuestList;
	public GameObject AdventureList;
	public ScrollRect optionRect;

	void Update()
	{
		if (selectedMembers.Count>0)
		{
			searchButton.interactable=true;
		}
		else{
			searchButton.interactable=false;
		}
		if (show) {
			GetComponent<CanvasGroup> ().alpha = 1;
			GetComponent<CanvasGroup> ().blocksRaycasts = true;
		} else {
			GetComponent<CanvasGroup> ().alpha = 0;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
	}
	
	public void UpdateText(List<Member> availablemembers,string title, string description,int maxselection,string search)
	{
		this.title.text=title;
		dialogue.text=description;
		selectedMembers.Clear();
		for (int i=0; i<availablemembers.Count; i++) {
			prefabList.GeneratePrefab(i,selectPrefab,"Member",selectList);
			prefabList [i].GetComponent<SlotInfo> ().FillSlotWithMember (availablemembers [i]);
			prefabList [i].GetComponent<SlotInfo> ().ResetSelection();
		}
		if (availablemembers.Count < prefabList.Count) {
			for (int i=availablemembers.Count; i<prefabList.Count; i++) {
				prefabList [i].SetActive (false);
			}
		}
		selectList.SetSize(scrollRect,availablemembers.Count, 48);
		maxSelection=maxselection;
		EnableSearchList(searchRecruitList,recruitChoices[0],search=="SearchRecruit");
		EnableSearchList(searchQuestList,questChoices[0],search=="SearchQuest");
		EnableSearchList(AdventureList,adventureChoices[0],search=="Adventure");
	}

	public void SelectSearch(SlotInfo slot)
	{
		if (searchType!=slot)
		{
			searchType.Select();
			slot.Select();
			searchType=slot;
		}

	}
	public void SelectMember (SlotInfo slot)
	{
		if(selectedMembers.Contains(slot))
		{
			slot.Select ();
			selectedMembers.Remove (slot);
		}
		else if(selectedMembers.Count<maxSelection)
		{
			slot.Select ();
			selectedMembers.Add(slot);
		}
	}
	public void EnableSearchList(GameObject searchlist,SlotInfo firstChoice,bool statement)
	{
		if (statement){
			optionRect.content=searchlist.GetComponent<RectTransform>();
			searchlist.GetComponent<RectTransform> ().offsetMax=new Vector2(searchlist.GetComponent<RectTransform> ().offsetMax.x, 0);
			if (searchType==null)
			{
				searchType=firstChoice;
				searchType.Select();
			}
			else
			{
				SelectSearch(firstChoice);
			}
		}
		searchlist.SetActive(statement);

	}
}
