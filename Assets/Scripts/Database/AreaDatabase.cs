using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaDatabase : MonoBehaviour {
	public List<Area> areaList=new List<Area>();
	public List<GatheringPoint> gatheringList=new List<GatheringPoint>();
	public Dictionary<string,List<string>> areaGatheringType=new Dictionary<string,List<string>>();
	// Use this for initialization
	void Start () {
		areaGatheringType["Forest"]=new List<string>(){"Flower","Herb","Fruit","Wood"};
		GenerateGatheringPoints();
		GenerateAreas();
	}
	
	void GenerateGatheringPoints()
	{
		gatheringList.Add(new GatheringPoint(0,"Flower",GetComponent<ItemDatabase>().FindItems("Flower"),5));

	}

	void GenerateAreas()
	{
		areaList.Add(new Area(0,"Green Forest","Forest",1,1,0,GetTypeGatheringPoint("Forest"),15));

	}

	List<GatheringPoint> GetTypeGatheringPoint(string areatype)
	{


		List<GatheringPoint>gatheringpoints=new List<GatheringPoint>();
		foreach (GatheringPoint point in gatheringList) {
			if (areaGatheringType[areatype].Contains(point.type))
				gatheringpoints.Add(point);
		}
		return gatheringpoints;

	}

	public Area FindArea(int id)
	{
		foreach(Area area in areaList)
		{
			if (area.id==id)
			{
				return area;
			}
		}
		return null;
	}
}
