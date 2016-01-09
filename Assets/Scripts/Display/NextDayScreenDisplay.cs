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
	// Use this for initialization
	void Start () {
		prefabList.Add (GameObject.Instantiate (prefab) as GameObject);
		prefabList [0].transform.SetParent (scrollList);
		ResetTransform (prefabList [0]);
	}
	
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
	public void UpdateText(int day, List<Task> tasks, List<string> activity)
	{
		title.text="Day "+day.ToString();
		if (tasks==null)
		{
			tasks=new List<Task>();
		}
		List<string> information=new List<string>();
		if (tasks.Count>0)
		for (int i=0; i<tasks.Count; i++) {
			information.Add(tasks[i].Details());
		}
		if (activity.Count>0)
		for (int i=0; i<activity.Count;i++){
			information.Add (activity[i]);
		}
		for (int i=0; i<information.Count; i++) {
			prefabList.GeneratePrefab(i,prefab,"Log",scrollList);
			prefabList [i].GetComponent<Text>().text=information[i];
		}
		if (information.Count < prefabList.Count) {
			for (int i=information.Count; i<prefabList.Count; i++) {
				if (i>0){
					prefabList [i].SetActive (false);
			}
				else
					prefabList [i].GetComponent<Text>().text=GameObject.FindObjectOfType<StringDatabase>().GetString("NoTask");
			}
		}
		int count=1;
		if (information.Count>1){
			count=information.Count;
		}
		scrollList.SetSize(scrollRect,count, 80);
	}

	void CreatePrefabs(){



	}
	void ResetTransform(GameObject slot)
	{
		slot.transform.localPosition=new Vector3(0,0,0);
		slot.transform.localScale=new Vector3(1,1,1);
	}
}
