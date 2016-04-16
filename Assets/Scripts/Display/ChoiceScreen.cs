using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChoiceScreen : MonoBehaviour {
	public int choice;
	public GameObject selectPrefab;
	public Transform selectList;
	public ScrollRect scrollRect;
	private List<GameObject> prefabList=new List<GameObject>();
	public bool show=false;
	public string choiceOrigin;
	GameObject myEventSystem;

	void Start(){
		myEventSystem = GameObject.Find("EventSystem");
	}
	void Update () {
		GetComponent<CanvasGroup>().SetShow(show);
	}

	public void PresentChoice(List<string> choices,string origin)
	{
		myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
		int count=choices.Count;
		choiceOrigin = origin;
		int prefabcount = prefabList.Count;
		if (choices.Count < prefabcount) {
			count=prefabList.Count;
		}
		int index=0;
		for (int i=0; i<count; i++) {
			prefabList.GeneratePrefab(i,selectPrefab,"",selectList);
			index = i;
			prefabList [i].GetComponent<ChoiceButton> ().value = i;
			if (i<choices.Count){
				prefabList [i].GetComponentInChildren<Text> ().text=choices[i];
			} else{
				prefabList [i].SetActive (false);
			}
		}
		selectList.SetSize (scrollRect, choices.Count, 64);
		show = true;
	}

	public void SelectSearch(int id)
	{
		choice = id;
		if (choiceOrigin=="Adventure") {
			Database.game.adventureScreen.Continue (choice);
		}
		show=false;
	}
}
