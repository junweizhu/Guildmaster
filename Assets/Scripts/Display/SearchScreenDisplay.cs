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
	public List<SlotInfo> selectedCharacters=new List<SlotInfo>();
	public int maxSelection;
	public bool show = false;
	public GameObject searchRecruitList;
	public GameObject searchQuestList;
	public GameObject AdventureList;
	public ScrollRect optionRect;

	void Update()
	{
		if (selectedCharacters.Count>0)
		{
			searchButton.interactable=true;
		}
		else{
			searchButton.interactable=false;
		}
		GetComponent<CanvasGroup>().SetShow(show);
	}
	
	public void UpdateText(List<Character> availablecharacters,string title, string description,int maxselection,string search)
	{
		this.title.text=title;
		dialogue.text=description;
		selectedCharacters.Clear();
		int count=availablecharacters.Count;
		if (availablecharacters.Count < prefabList.Count) {
			count=prefabList.Count;
		}
		for (int i=0; i<count; i++) {
			prefabList.GeneratePrefab(i,selectPrefab,"Character",selectList);
			if (i<availablecharacters.Count){
				prefabList [i].GetComponent<SlotInfo> ().FillSlotWithCharacter (availablecharacters [i]);
				prefabList [i].GetComponent<SlotInfo> ().ResetSelection();
			} else{
				prefabList [i].SetActive (false);
			}
		}
		selectList.SetSize(scrollRect,availablecharacters.Count, 64);
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
	public void SelectCharacter (SlotInfo slot)
	{
		if(selectedCharacters.Contains(slot))
		{
			slot.Select ();
			selectedCharacters.Remove (slot);
		}
		else if(selectedCharacters.Count<maxSelection)
		{
			slot.Select ();
			selectedCharacters.Add(slot);
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
