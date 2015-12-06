using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item {
	public int itemId;
	public string itemName;
	public string itemType;
	public int itemMoney;
	public string itemDescription;
	public string itemModifier1;
	public int itemValue1;
	public string itemModifier2;
	public int itemValue2;
	public string itemModifier3;
	public int itemValue3;
	
	public Item(int id,string name, string type, int money, string description, string modifier1=null, int value1=0,string modifier2=null,int value2=0,string modifier3=null, int value3=0)
	{
		itemId=id;
		itemName=name;
		itemType=type;
		itemMoney=money;
		itemDescription=description;
		itemModifier1=modifier1;
		itemValue1=value1;
		itemModifier2=modifier2;
		itemValue2=value2;
		itemModifier3=modifier3;
		itemValue3=value3;
	}
	public Item()
	{}
}
