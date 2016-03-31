using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaDatabase {
	private List<Area> areaList=new List<Area>();
	private List<GatheringPoint> gatheringList=new List<GatheringPoint>();
	private Dictionary<string,List<string>> areaGatheringType=new Dictionary<string,List<string>>();
	// Use this for initialization

	public AreaDatabase () {
		areaGatheringType["Forest"]=new List<string>(){"Flower","Herb","Fruit","Wood"};
		areaGatheringType["Plains"]=new List<string>(){"Flower","Herb","Fruit"};
		GenerateGatheringPoints();
		GenerateAreas();
	}
	
	public List<Area> GetArea(){
		return areaList;
	}

	void GenerateGatheringPoints()
	{
		gatheringList.Add(new GatheringPoint(0,"Flower",Database.items.FindGatheringItems("Flower"),5));
		gatheringList.Add(new GatheringPoint(1,"Wood",Database.items.FindGatheringItems("Wood"),5));
	}

	void GenerateAreas()
	{
		areaList.Add(new Area(0,"Green Plains","Plains",0,1,0,40,15));
		areaList.Add(new Area(1,"Green Forest","Forest",0,1,0,50,15));

	}

	public List<GatheringPoint> GetTypeGatheringPoint(string areatype)
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
	public int RandomArea(int level){
		List<int> arealist=new List<int>();
		foreach (Area area in areaList){
			if (area.level<level+5 &&area.level>level-5){
				arealist.Add (area.id);
			}
		}
		if (arealist.Count>0){
		return arealist[Random.Range(0,arealist.Count)];
		} else{
			return 0;
		}
	}
}
