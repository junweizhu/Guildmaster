using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {
	[SerializeField]
	private List<Item> itemList= new List<Item>();
	private List<int> alchemyList=new List<int>();
	private List<int> weaponList=new List<int>();
	private List<int> armorList=new List<int>();
	private List<int> arcaneList=new List<int>();
	private List<int> bookList=new List<int>();
	// Use this for initialization
	void Start () {

		itemList.Add(new Item(0,"Potion","Consumable",15,"Heals 50 health","Heal",50));
		itemList.Add(new Item(1,"Iron Sword","Weapon",15,"A sword made of iron","Attack",5));


		AlchemyList();
		WeaponList();
		ArmorList();
		ArcaneList();
		BookList();
	}

	public Item FindItem(int id)
	{
		foreach(Item item in itemList)
		{
			if (item.itemId==id)
				return item;
		}
		return null;
	}
	public List<Item> GetShopList(string shop)
	{
		List<Item> shopList=new List<Item>();
		if (shop.Contains("Alchemy"))
		{
			foreach (int i in alchemyList)
			{
				shopList.Add(FindItem(i));
			}
		}
		if (shop.Contains("Weapon"))
		{
			foreach (int i in weaponList)
			{
				shopList.Add(FindItem(i));
			}
		}
		if (shop.Contains("Armor"))
		{
			foreach (int i in armorList)
			{
				shopList.Add(FindItem(i));
			}
		}
		if (shop.Contains("Arcane"))
		{
			foreach (int i in arcaneList)
			{
				shopList.Add(FindItem(i));
			}
		}
		if (shop.Contains("Library"))
		{
			foreach (int i in bookList)
			{
				shopList.Add(FindItem(i));
			}
		}
		return shopList;
	}
	void AlchemyList()
	{
		alchemyList.Add(0);
	}
	void WeaponList()
	{
		weaponList.Add(1);
	}
	void ArmorList()
	{
		;
	}
	void ArcaneList()
	{
		;
	}
	void BookList()
	{
		;
	}
}
