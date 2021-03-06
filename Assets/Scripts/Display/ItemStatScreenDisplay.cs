﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemStatScreenDisplay : MonoBehaviour
{
	public Text itemName;
	public Text itemValue;
	public Text itemDescription;
	public List<Text> statlist = new List<Text> ();
	public Dictionary<string,Text>stats = new Dictionary<string,Text > (); 

	// Use this for initialization
	void Start ()
	{
		foreach (Text stat in statlist) {
			stats [stat.name] = stat;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void FillSlot (Item item)
	{
		foreach (KeyValuePair<string,Text> stat in stats) {
			stat.Value.text = " ";
		}
		if (item != null) {
			itemName.text = item.name;
			itemValue.text = item.sellValue.ToString () + " G";
			itemDescription.text = item.description;
			if (itemDescription.text!="")
				itemDescription.text+=" ";
			if (item.stats!=null) {
				if (item.subType=="Heal"){
					itemDescription.text+=string.Format(Database.strings.GetString(item.subType),item.GetStatString());
				}
				foreach (KeyValuePair<string,int> stat in item.stats) {
					if (stats.ContainsKey (stat.Key)) {
						if (stat.Value != 0) {
							stats [stat.Key].text += stat.Value.ToString ();
						}
					}
				}
				stats["Weight"].text=item.weight.ToString();
			}
		} else {
			itemName.text = "";
			itemValue.text = "";
			itemDescription.text = "";
		}
	}
}
