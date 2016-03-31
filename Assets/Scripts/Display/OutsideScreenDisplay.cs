using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class OutsideScreenDisplay : MonoBehaviour {
	public GameObject areaPrefab;
	public List<GameObject> prefabList=new List<GameObject>();
	public Transform areaList;
	public Text areaName;
	public Text areaType;
	public Text areaDifficulty;
	public Text numberVisits;
	public Text numberGathering;
	public Button travelButton;
	private SlotInfo lastSelected;
	private Guild guild;
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

	public void UpdateText(){
		guild=Database.myGuild;
		List<int> arealist=guild.knownAreas;

		for (int i=0;i<arealist.Count;i++)
		{
			prefabList.GeneratePrefab(i,areaPrefab,"Area",areaList);
			prefabList [i].GetComponent<SlotInfo> ().FillSlotWithArea(Database.areas.FindArea(arealist[i]));
			prefabList [i].GetComponent<SlotInfo> ().ResetSelection();
		}
		areaList.SetSize(arealist.Count,64);
		FillStats (null);
		if (lastSelected != null) {
			lastSelected = null;
		}
		travelButton.interactable=false;
	}

	public void FillStats(Area area,int visit=0,int gatheringpoints=0)
	{
		if (area!=null){
			areaName.text=area.name;
			areaType.text=area.type;
			areaDifficulty.text=area.difficulty.ToString();
		}
		else
		{
			areaName.text="";
			areaType.text="";
			areaDifficulty.text="";
		}
		numberVisits.text=visit.ToString();
		numberGathering.text=gatheringpoints.ToString();
	}

	public void DisplayAreaStats (SlotInfo areaslot)
	{
		if(lastSelected!=areaslot) {
			areaslot.Select ();
			int selectedarea=areaslot.id;
			FillStats (Database.areas.FindArea(selectedarea),guild.successfulVisits[selectedarea],guild.foundGatheringPoints[selectedarea]);
			if (lastSelected != null) {
				lastSelected.Select ();
			}
			lastSelected = areaslot;
			travelButton.interactable=true;
		}
	}
	public Area GetSelectedArea(){
		return Database.areas.FindArea(lastSelected.id);
	}
}
