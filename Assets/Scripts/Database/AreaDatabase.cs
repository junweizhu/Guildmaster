using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaDatabase {
	private List<Area> areaList=new List<Area>();
	private List<GatheringPoint> gatheringList=new List<GatheringPoint>();
	private Dictionary<string,Dictionary<string,int>> areaGatheringType=new Dictionary<string,Dictionary<string,int>>();
	// Use this for initialization

	public AreaDatabase () {
		areaGatheringType["Plains"]=new Dictionary<string,int>(){{"Flowerspot",100},{"Animal",40},{"Livestock",10}};
		//areaGatheringType["Forest"]=new List<string>(){{"Flower","Herb","Fruit","Wood"};

		GenerateGatheringPoints();
		GenerateAreas();
	}
	
	public List<Area> GetArea(){
		return areaList;
	}

	void GenerateGatheringPoints()
	{
		int i = Database.items.firstMaterialId;
		gatheringList.Add(new GatheringPoint(0,"Flowerspot",new List<int>(){21+i,22+i,23+i},5));
		gatheringList.Add(new GatheringPoint(1,"Bird",new List<int>(){42+i,49+i,50+i,56+i,63+i},4));
		gatheringList.Add(new GatheringPoint(2,"Animal",new List<int>(){43+i,51+i,52+i,61+i,62+i},2));
		gatheringList.Add(new GatheringPoint(3,"Livestock",new List<int>(){44+i,45+i,53+i,69+i},2));
	}

	void GenerateAreas()
	{
		areaList.Add(new Area(0,"Green Plains","Plains",0,1,0,40,15));
		areaList.Add(new Area(1,"Green Forest","Forest",0,1,0,50,15));

	}

	public List<KeyValuePair<GatheringPoint,int>> GetTypeGatheringPoint(string areatype)
	{
		List<KeyValuePair<GatheringPoint,int>>gatheringpoints=new List<KeyValuePair<GatheringPoint,int>>();
		foreach (GatheringPoint point in gatheringList) {
			if (areaGatheringType[areatype].ContainsKey(point.type))
				gatheringpoints.Add(new KeyValuePair<GatheringPoint, int>(point,areaGatheringType[areatype][point.type]));
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
