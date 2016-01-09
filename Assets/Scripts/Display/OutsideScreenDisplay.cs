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
	public Text numberHunting;
	public Button travelButton;
	private SlotInfo lastSelected;
	private Guild guild;
	private AreaDatabase adb;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void UpdateText(Guild guild){

		for (int i=0;i<guild.knownAreas.Count;i++)
		{
			prefabList.GeneratePrefab(i,areaPrefab,"Area",areaList);
			prefabList [i].GetComponent<SlotInfo> ().FillSlotWithArea(guild.knownAreas[i]);
			prefabList [i].GetComponent<SlotInfo> ().ResetSelection();
		}
		areaList.SetSize(guild.knownAreas.Count,48);
		FillStats (null);
		if (lastSelected != null) {
			lastSelected = null;
		}
		travelButton.interactable=false;
		this.guild=guild;
		adb=GameObject.FindObjectOfType<AreaDatabase>();
	}

	public void FillStats(Area area,int visit=0,int gatheringpoints=0, int huntinggrounds=0)
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
		numberHunting.text=huntinggrounds.ToString();
	}

	public void DisplayAreaStats (SlotInfo areaslot)
	{
		if(lastSelected!=areaslot) {
			areaslot.Select ();
			Area selectedarea=adb.FindArea(areaslot.id);
			FillStats (selectedarea,guild.successfulVisits[selectedarea],guild.foundGatheringPoints[selectedarea],guild.foundHuntingGrounds[selectedarea]);
			if (lastSelected != null) {
				lastSelected.Select ();
			}
			lastSelected = areaslot;
			travelButton.interactable=true;
		}
	}
	public Area GetSelectedArea(){
		return adb.FindArea(lastSelected.id);
	}
}
