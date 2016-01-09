using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemDatabase : MonoBehaviour
{
	[SerializeField]
	private List<Item>itemList = new List<Item> ();
	private Dictionary<string,Shop> shopList = new Dictionary<string,Shop> ();
	private Dictionary<int,string> shopName= new Dictionary<int,string>();
	// Use this for initialization
	void Start ()
	{
		AddShop ("Alchemy Workshop", "Sells potions");//0
		AddShop ("Weaponsmith", "Sells all kinds of weapons");//1
		AddShop ("Armorsmith", "sells all kinds of armor");//2
		AddShop ("Arcanesmith", "sells staves and other magical items");//3
		AddShop ("Library", "sells educational books for learning and improvement");//4

		AddNewItem ("Health Potion", "Consumable", 15, "Recovers 10 health","",new Dictionary<string,int>(){{"Heal", 10}}, true, 0);
		AddNewItem ("Iron Sword", "Weapon", 15, "A sword made of iron","", new Dictionary<string,int>(){{"Attack", 2}}, true, 1);
		AddNewItem ("Iron Armor", "Armor", 15, "An armor made of iron","", new Dictionary<string,int>(){{"Defense", 5}}, true, 2);
		AddNewItem ("Wooden Staff", "Weapon", 15, "A simple staff made of wood","Magic",new Dictionary<string,int>(){{ "Attack", 5}}, true, 3);
		AddNewItem ("Azalea","Flower",10, "A flower known to be highly toxic");
	}

	void AddNewItem (string name, string type, int money, string description,string element="", Dictionary<string,int> stats=null, bool sold=false, int shop=0, int level=0)
	{
		itemList.Add (new Item (itemList.Count, name, type, money, description,element, stats));
		if (sold)
			shopList [shopName[shop]].AddItem (level, itemList.Last ());
	}
	

	void AddItemToShop (string shop, int level, Item item)
	{
		shopList [shop].AddItem (level, item);
	}

	public void AddShop (string name, string description)
	{
		shopName[shopList.Count]=name;
		shopList [name] = new Shop (shopList.Count, name, description);

	}

	public Item FindItem (int id)
	{
		foreach (Item item in itemList) {
			if (item.id == id)
				return item;
		}
		return null;
	}

	public List<Item> FindItems(string type){
		List<Item>items=new List<Item>();
		foreach (Item item in itemList) {
			if (item.type == type)
				items.Add(item);
		}
		return items;

	}
	/*public int FindItemId(string name)
		{
			foreach(Item item in itemList)
			{
				if (item.itemName==name)
					return item.itemId;
			}
			return null;
		}*/

	public List<Item> GetShopList (string shop, int level)
	{
		List<Item> shoppinglist = new List<Item> ();

		foreach (KeyValuePair<int,List<Item>> list in shopList[shop].itemList) {
			if (level >= list.Key) {
				foreach (Item item in list.Value) {
					shoppinglist.Add (item);
				}
			}
		}
		return shoppinglist;
	}
}
