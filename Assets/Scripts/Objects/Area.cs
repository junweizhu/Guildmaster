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


	public Area (int id, string name, string type, int level, int difficulty, int traveltime, int size, int maxpoints = 0, Dictionary<int,int> links = default(Dictionary<int,int>))
	{
		this.id = id;
		this.type = type;
		this.name = name;
		travelTime = traveltime;
		this.level = level;
		this.difficulty = difficulty;
		linkedAreas = links;
		maxGatheringPoints = maxpoints;
		this.size = size;
	}

	public GatheringPoint FindRandomGatheringPoint ()
	{
		GatheringPoint selected = null;
		int lowestChance = 101;
		int RNG = 0;
		RNG = Mathf.RoundToInt (Random.Range (1, 1000) / 10);
		List<KeyValuePair<GatheringPoint,int>> gatheringpoints = Database.areas.GetTypeGatheringPoint (type);
		for (int i = 0; i < gatheringpoints.Count; i++) {
			if (gatheringpoints [i].Value >= RNG && gatheringpoints [i].Value < lowestChance) {
				selected = gatheringpoints [i].Key;
				lowestChance = gatheringpoints [i].Value;
			}
		}
		return selected;
	}
}
