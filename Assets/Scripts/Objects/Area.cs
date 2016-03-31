using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Area
{
	
	public string name;
	public int id;
	public string type;
	public int level;
	public int difficulty;
	public int travelTime;
	public int size;
	public Dictionary<int,int> linkedAreas;
	public int maxGatheringPoints;
	//public Dictionary<int,Dictionary<int,int>>map=new Dictionary<int,Dictionary<int,int>>();
	//public Dictionary<int,Dictionary<int,string>>eventMap=new Dictionary<int,Dictionary<int,string>>();
	//public Vector2 startingPoint;


	public Area (int id, string name, string type, int level, int difficulty, int traveltime,int size, int maxpoints=0, Dictionary<int,int> links=default(Dictionary<int,int>))
	{
		this.id = id;
		this.type = type;
		this.name = name;
		travelTime = traveltime;
		this.level = level;
		this.difficulty = difficulty;
		linkedAreas = links;
		maxGatheringPoints = maxpoints;
		this.size=size;
	}
/*
	public void GenerateMap(){
		for(int x=0;x<10;x++){
			map[x]=new Dictionary<int,int>();
			eventMap[x]=new Dictionary<int,string>();
			for (int y=0;y<10;y++){
				map[x][y]=0;
				eventMap[x][y]="";
			}
		}
		int startX=Random.Range(0,9);
		int startY=Random.Range(0,9);
		if (startX!=0&&startX!=9&&startY!=0&&startY!=9){
			startY=Random.Range(0,1)*9;
		}
		startingPoint=new Vector2(startX,startY);
	}*/

	public GatheringPoint FindRandomGatheringPoint ()
	{
		GatheringPoint selected = null;
		int lastRNG = 0;
		int currentRNG = 0;
		foreach (GatheringPoint point in Database.areas.GetTypeGatheringPoint(type)) {
			if (point.gatherableItems.Count > 0) {
				currentRNG = Random.Range (1, 1000);
				if (currentRNG > lastRNG) {
					selected = point;
					lastRNG = currentRNG;
				}
			}
		}
		return selected;
	}
}
