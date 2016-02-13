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
			prefabList [i].GetComponent<SlotInfo> ().FillSlotWithArea(Database.areas.FindArea(guild.knownAreas[i]));
			prefabList [i].GetComponent<SlotInfo> ().ResetSelection();
		}
		areaList.SetSize(guild.knownAreas.Count,64);
		FillStats (null);
		if (lastSelected != null) {
			lastSelected = null;
		}
		travelButton.interactable=false;
		this.guild=guild;
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
			int selectedarea=areaslot.id;
			FillStats (Database.areas.FindArea(selectedarea),guild.successfulVisits[selectedarea],guild.foundGatheringPoints[selectedarea],guild.foundHuntingGrounds[selectedarea]);
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
