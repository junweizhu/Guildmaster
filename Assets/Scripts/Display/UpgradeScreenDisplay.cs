using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UpgradeScreenDisplay : MonoBehaviour {
	public GameObject upgradeslotPrefab;
	public Transform upgradeslotList;
	private List<GameObject> slotPrefabList=new List<GameObject>();
	public Text currentUpgrade;
	public Text nextUpgrade;
	public Text cost;
	public Text materialCost;
	public Button upgradeButton;
	public SlotInfo lastSelected;
	public Text currency;
	public Text upgradecounter1;
	public Text upgradecounter2;


	public void UpdateText()
	{
		Dictionary<int,int> upgradelist=Database.myGuild.upgradelist;
		int count=upgradelist.Count;
		if (upgradelist.Count<slotPrefabList.Count){
			count=slotPrefabList.Count;
		}
		for(int i=0; i<count;i++)
		{
			slotPrefabList.GeneratePrefab(i,upgradeslotPrefab,upgradeslotPrefab.name,upgradeslotList);
			if (i<upgradelist.Count){
				slotPrefabList[i].GetComponent<SlotInfo>().FillSlotWithUpgrade(Database.upgrades.GetUpgrade(i),upgradelist[i]);
				slotPrefabList [i].GetComponent<SlotInfo> ().ResetSelection();
			} else{
				slotPrefabList[i].SetActive(false);
			}
		}
		upgradeslotList.SetSize(upgradelist.Count,64);
		FillStats(null);
		if (lastSelected != null) {
			lastSelected = null;
		}
		upgradeButton.interactable=false;
	}

	public void FillStats(Upgrade upgrade){
		Dictionary<int,int> upgradelist=Database.myGuild.upgradelist;
		if (upgrade!=null){
			currentUpgrade.text=upgrade.MaxSize(upgradelist[upgrade.id]).ToString();
			nextUpgrade.text=upgrade.MaxSize(upgradelist[upgrade.id]+1).ToString();
			cost.text=upgrade.UpgradeCost(upgradelist[upgrade.id]).ToString();
			materialCost.text=upgrade.MaterialCostString(upgradelist[upgrade.id]+1);
			upgradecounter1.text=" "+upgrade.sizeName;
			upgradecounter2.text=" "+upgrade.sizeName;
			currency.text=" "+Database.strings.GetString("CurrencyCounter");
		} else{
			currentUpgrade.text="";
			nextUpgrade.text="";
			cost.text="";
			materialCost.text="";
			upgradecounter1.text="";
			upgradecounter2.text="";
			currency.text="";
		}
	}

	public void DisplayUpgradeStats (SlotInfo upgradeslot)
	{
		if(lastSelected!=upgradeslot) {
			upgradeslot.Select ();
			int selectedupgrade=upgradeslot.id;
			FillStats (Database.upgrades.GetUpgrade(selectedupgrade));
			if (lastSelected != null) {
				lastSelected.Select ();
			}
			lastSelected = upgradeslot;
			if (Database.myGuild.CanUpgrade(selectedupgrade)){
				upgradeButton.interactable=true;
			} else{
				upgradeButton.interactable=false;
			}
		}
	}
}
