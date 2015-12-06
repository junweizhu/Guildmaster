using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemStatScreenDisplay : MonoBehaviour {
	public Text itemName;
	public Text itemType;
	public Text itemQuality;
	public Text itemQuantity;
	public Text itemValue;
	public Text itemDescription;
	public bool show;
	public List<Text> statlist=new List<Text>();
	public Dictionary<string,Text>stats=new Dictionary<string,Text >(); 

	// Use this for initialization
	void Start () {
		foreach (Text stat in statlist)
		{
			stats[stat.name]=stat;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(show)
		{
			GetComponent<CanvasGroup>().alpha=1;
			GetComponent<CanvasGroup>().blocksRaycasts=true;
		}
		else 
		{
			GetComponent<CanvasGroup>().alpha=0;
			GetComponent<CanvasGroup>().blocksRaycasts=false;
		}
	}
	public void FillSlot(Item item, int quality, int quantity)
	{
		foreach(KeyValuePair<string,Text> stat in stats)
		{
			stat.Value.text="";
		}
		itemName.text=item.itemName;
		itemType.text=item.itemType;
		itemQuality.text=quality.ToString()+"%";
		itemQuantity.text=quantity.ToString();
		itemValue.text=item.itemMoney.ToString()+" Gold";
		itemDescription.text=item.itemDescription;
		if (item.itemModifier1!=null)
		{
			if (item.itemValue1>0)
				stats[item.itemModifier1].text="+";
			if (item.itemValue1!=0)
				stats[item.itemModifier1].text+=item.itemValue1.ToString();
		}
	}
	public void CloseScreen()
	{
		show=false;
	}
}
