using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemDatabase
{
	[SerializeField]
	private List<Item>itemList = new List<Item> ();
	private Dictionary<string,Shop> shopList = new Dictionary<string,Shop> ();
	private Dictionary<int,string> shopName= new Dictionary<int,string>();
	// Use this for initialization
	public ItemDatabase ()
	{
		AddShop ("Alchemy Workshop", "Sells potions");//0
		AddShop ("Weaponsmith", "Sells all kinds of weapons");//1
		AddShop ("Armorsmith", "sells all kinds of armor");//2
		AddShop ("Arcanesmith", "sells staves and other magical items");//3
		AddShop ("Library", "sells educational books for learning and improvement");//4

		Consumables();
		Weapons ();
		Armor();
		Accessory();
		Books ();
		Materials ();
	}

	public void Consumables(){
		AddItem ("Health Vial", "Consumable","Heal", 50, "",3,5,0,0,0,0,0, true, 0);
		AddItem ("Health Flask", "Consumable","Heal", 110, "",3,10,0,0,0,0,0, true, 0);
		AddItem ("Health Potion", "Consumable","Heal", 230, "",3,20,0,0,0,0,0, true, 0);
		AddItem ("Mana Vial", "Consumable","Heal", 80, "",3,0,5,0,0,0,0, true, 0);
		AddItem ("Mana Flask", "Consumable","Heal", 170, "",3,0,10,0,0,0,0, true, 0);
		AddItem ("Mana Potion", "Consumable","Heal", 350, "",3,0,20,0,0,0,0, true, 0);

	}

	public void Weapons(){
		AddEquipment ("Iron Sword","Weapon", "Sword", 240, "A sword made of iron",40,2,2,0,0,0,100,20,1,15,"", true,1);
		AddEquipment ("Iron Axe","Weapon", "Axe", 220, "An axe made of iron",50,4,4,0,0,0,80,0,3,20,"", true,1);
		AddEquipment ("Iron Spear","Weapon", "Spear", 230, "A spear made of iron",60,3,3,0,0,0,110,5,2,30,"", true,1);
		AddEquipment ("Iron Crossbow","Weapon", "Bow", 270, "A bow made of iron",40,3,5,0,0,0,70,0,1,5,"", true,1);
		AddEquipment ("Iron Mace", "Weapon","Mace", 250, "A mace made of iron",45,4,1,0,0,0,80,0,2,20,"", true,1);
		AddEquipment ("Iron Dagger","Weapon", "Dagger", 210, "A dagger made of iron",35,1,1,0,0,0,85,30,1,5,"", true,1);
		AddEquipment ("Wooden Staff","Weapon", "Staff", 260, "A staff made of wood",60,2,2,3,0,0,105,5,2,30,"", true,3);
		AddEquipment ("Wooden Rod", "Weapon","Rod", 270, "A rod made of wood",50,1,1,4,0,0,100,20,1,5,"", true,3);
	}

	public void Armor(){

		AddEquipment("Reinforced Clothes","Armor","",170,"Clothing layered for extra protection",60,1,0,0,2,2,0,0,0,0,"",true,2);
		AddEquipment("Leather Armour","Armor","",180,"Armour made of thick leather",55,2,0,0,3,1,0,-3,0,0,"",true,2);
		AddEquipment("Iron Chainmail","Armor","",200,"Armour made of linked iron rings to form a lightweight but strong protection",50,3,0,0,4,0,-3,-3,0,0,"",true,2);
		AddEquipment("Iron Breastplate","Armor","",220,"Protects the upper body and shoulders",45,4,0,0,5,0,-3,-5,0,0,"",true,2);
		AddEquipment("Iron Cuirass","Armor","",240,"Full body protection",40,5,0,0,7,0,-10,-10,0,0,"",true,2);
		AddEquipment("Cotton Robe","Armor","",180,"Robe magically woven with cotton",55,0,0,1,1,2,10,10,0,0,"",true,3);
	}

	public void Accessory(){
		AddEquipment("Wooden Shield","Accessory","Shield",135,"A shield made of wood",60,1,0,0,0,0,-5,0,4,35,"",true,2);
		AddEquipment("Leather Helm","Accessory","Helm",145,"Helm made of leather",60,0,0,0,1,1,0,0,0,0,"",true,2);
		AddEquipment("Cotton Hood", "Accessory","Helm",145,"Hood woven with enchanted cotton",55,0,0,0,0,2,0,0,0,0,"",true,3);
		AddEquipment("Iron Circlet", "Accessory","Helm",150,"Magically enchanted circlet made of iron",45,0,0,1,0,1,0,0,0,0,"",true,3);
		AddEquipment("Leather Armguard","Accessory","Gauntlet",160,"An armguard made of leather",55,0,1,0,1,0,5,0,0,0,"",true,2);
		AddEquipment("Iron Bangle", "Accessory","Gauntlet",170,"Magically enchanted bangle made of iron",45,0,0,1,1,0,5,0,0,0,"",true,3);
		AddEquipment("Leather Boots","Accessory","Boots",155,"Boots made out of leather",60,1,0,0,1,0,0,5,0,0,"",true,2);
		AddEquipment("Leather Pouch","Accessory","Backpack",75,"Pouch made of leather large enough for 5 items",60,0,5,true);
		AddEquipment("Leather Backpack","Accessory","Backpack",175,"Backpack made of leather for gathering 10 items",50,1,5,true,0,1);

	}

	public void Books(){

	}

	public void Materials(){
		AddItem ("Common Branch","Material","Wood",5,"You see these branches everywhere");

	}




	void AddItem(string name, string type,string subtype, int money, string description){
		Dictionary<string,int> stats=null;
		itemList.Add (new Item (itemList.Count, name, type, money, description,1,subtype,"", stats));
	}

	void AddItem(string name, string type, string subtype, int money, string description, int durability, int health, int mana, int strength, int intelligence, int dexterity, int agility, bool sold=false,int shop=0, int level=0){
		Dictionary<string,int> stats=new Dictionary<string,int>();
		if (health>0){
			stats["Health"]=health;
		}
		if (mana>0){
			stats["Mana"]=mana;
		}
		if (strength>0){
			stats["Strength"]=strength;
		}
		if (intelligence>0){
			stats["Intelligence"]=intelligence;
		}
		if (dexterity>0){
			stats["Dexterity"]=dexterity;
		}
		if (agility>0){
			stats["Agility"]=agility;
		}

		itemList.Add (new Item (itemList.Count, name, type, money, description,durability,subtype,"", stats));
		if (sold)
			shopList [shopName[shop]].AddItem (level, itemList.Last ());
	}

	void AddEquipment(string name, string type, string subtype, int money, string description, int durability, int weight, int carrySize, bool sold=false, int shop=0, int level=0){
		Dictionary<string,int> stats=new Dictionary<string,int>();
		if (carrySize>0){
			stats["CarrySize"]=carrySize;
		}
		if (weight>0){
			stats["Weight"]=weight;
		}
		itemList.Add (new Item (itemList.Count, name, type, money, description,durability,subtype,"", stats));
		if (sold)
			shopList [shopName[shop]].AddItem (level, itemList.Last ());
	}



	void AddEquipment(string name,string type, string subtype,int money,string description,int durability, int weight, int pattack, int mattack, int pdefense, int mdefense,int acc, int eva, int block, int blockchance,string element="",bool sold=false,int shop=1,int level=0){
		Dictionary<string,int> stats= new Dictionary<string,int>();
		if (pattack>0){
			stats["PAttack"]=pattack;
		}
		if (mattack>0){
			stats["MAttack"]=mattack;
		}
		if (pdefense>0){
			stats["PDefense"]=pdefense;
		}
		if (mdefense>0){
			stats["MDefense"]=mdefense;
		}
		if (acc>0){
			stats["Accuracy"]=acc;
		}
		if (eva>0){
			stats["Evade"]=eva;
		}
		if (block>0){
			stats["Block"]=block;
		}
		if (blockchance>0){
			stats["BlockChance"]=blockchance;
		}
		if (weight>0){
			stats["Weight"]=weight;
		}
		itemList.Add (new Item (itemList.Count, name, type, money, description,durability,subtype,element, stats));
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

	public List<Item> FindItems(string type, string ignoreSubtype1="", string ignoreSubtype2=""){
		List<Item>items=new List<Item>();
		foreach (Item item in itemList) {
			if (item.type == type && item.subType!=ignoreSubtype1 && item.subType!=ignoreSubtype2)
				items.Add(item);
		}
		return items;
	}
	public List<Item> FindGatheringItems(string subtype){
		List<Item>items=new List<Item>();
		foreach (Item item in itemList) {
			if (item.type == subtype)
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
