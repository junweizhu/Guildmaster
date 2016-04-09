using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemDatabase
{
	[SerializeField]
	private List<Item>itemList = new List<Item> ();
	private List<Shop> shopList = new List<Shop> ();
	private List<Item> baseMaterial=new List<Item>();
	private List<Item> baseItem=new List<Item>();
	private Dictionary<string,int> tier= new Dictionary<string,int> ();
	public int firstMaterialId;

	// Use this for initialization
	public ItemDatabase ()
	{
		AddShop ("Alchemy Workshop", "Sells potions");//0
		AddShop ("Weaponsmith", "Sells all kinds of weapons");//1
		AddShop ("Armorsmith", "sells all kinds of armor");//2
		AddShop ("Arcanesmith", "sells staves and other magical items");//3
		AddShop ("Training Hall", "Offers teachings in skills and abilities");//4

		tier.Add ("Leather",0);
		tier.Add ("Cotton",10);
		tier.Add ("Silk",20);
		tier.Add ("Cashmere",30);
		tier.Add ("Wood",0);
		tier.Add ("Copper",10);
		tier.Add ("Silver",20);
		tier.Add ("Gold",30);
		tier.Add ("Iron",20);
		tier.Add ("Steel",30);


		BaseMaterials();
		Consumables();
		Weapons ();
		Armor();
		Accessory();
		firstMaterialId = itemList.Count;
		Materials ();
	}
	public void BaseMaterials(){
		AddMaterial ("Leather", "Material","Leather", 25, "",	50,1,3,0,3,2,1.05f,1.05f,0);
		AddMaterial ("Cotton", "Material","Fabric", 25, "",		45,0.75f,2,1,2,4,1.1f,1.1f,0);
		AddMaterial ("Silk", "Material","Fabric", 30, "",		42.5f,0.5f,1,2,2,6,1.15f,1.15f,0);
		AddMaterial ("Cashmere", "Material","Fabric", 40, "",	40,0.25f,0,3,1,7,1.25f,1.25f,0);
		
		AddMaterial ("Wood", "Material","Wood", 20, "",			40,1,2,1,3,2,1.0f,1.0f,2.5f);	
		AddMaterial ("Copper", "Material","Metal", 25, "",		45,2,3,2,4,3,0.95f,0.95f,3.5f);
		AddMaterial ("Silver", "Material","Metal", 35, "",		50,3,3,4,4,5,0.9f,0.9f,4.5f);
		AddMaterial ("Gold", "Material","Metal", 45, "",		65,4,3,5,4,6,0.85f,0.85f,5);
		AddMaterial ("Iron", "Material","Metal", 30, "",		60,3,4,2,5,3,0.9f,0.9f,4);
		AddMaterial ("Steel", "Material","Metal", 40, "",		75,2,5,2,6,3,0.85f,0.85f,4.5f);
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
		AddEquipment ("Sword","Weapon", "Sword", 6, "",		1,2,1.5f,0.5f,0,0,100,20,0.5f,15,1, "Wood","Metal");
		AddEquipment ("Axe","Weapon", "Axe", 5.5f, "",		1.25f,4,3,0.2f,0,0,80,0,1.5f,20,1, "Wood","Metal");
		AddEquipment ("Spear","Weapon", "Spear", 5.75f, "",	1.5f,3,2.5f,0.5f,0,0,120,0,1,30,1, "Wood","Metal");
		AddEquipment ("Bow","Weapon", "Bow", 6.75f, "",		1.25f,3,3,0.3f,0,0,75,0,0.5f,5,1, "Wood","Metal");
		AddEquipment ("Mace", "Weapon","Mace", 6.25f, "",	1.125f,4,1.5f,1,0,0,80,0,1,20,1, "Wood","Metal");
		AddEquipment ("Dagger","Weapon", "Dagger", 5, "",	1.125f,0.5f,1,0.5f,0,0,85,30,0.5f,5,1, "Wood","Metal");
		AddEquipment ("Staff","Weapon", "Staff", 6.5f, "",	1.5f,2,2,2,0,0,105,6,1,25,3, "Wood","Metal");
		AddEquipment ("Rod", "Weapon","Rod", 6.75f, "",		1,1,1,3,0,0,100,20,0.5f,5,3, "Wood","Metal");
	}

	public void Armor(){

		AddEquipment("Robe","Body","Robe",4.5f,"",			1.2f,0.1f,0,1,.5f,2,2.5f,0,0,0,3,"Leather","Fabric");
		AddEquipment("Tunic","Body","Cloth",4.25f,"",		1.25f,1,0,0,1,1.5f,5,2.5f,0,0,2,"Leather","Fabric");
		AddEquipment("Garb","Body","Cloth",4.5f,"",			1.2f,2,0,0,1.5f,1,0,-2.5f,0,0,2,"Leather","Fabric");
		AddEquipment("Chainmail","Body","Armor",5,"",		1.25f,1.5f,0,0,1.5f,1,-2.5f,-5,0,0,2,"Metal");
		AddEquipment("Breastplate","Body","Armor",5.5f,"",	1.125f,2,0,0,2,0.75f,-5f,-10,0,0,2,"Metal");
		AddEquipment("Cuirass","Body","Armor",6,"",			1,2.5f,0,0,3,0.5f,-10,-15f,0,0,2,"Metal");

	}

	public void Accessory(){
		AddEquipment("Shield","Accessory","Shield",3.375f,"",1.5f,1,0,0,0,0,-5,0,2,35,2, "Wood","Metal");
		AddEquipment("Helm","Accessory","Helm",3.625f,"",	1.375f,0.2f,0,0,0.5f,0.5f,0,0,0,0,2,"Wood","Metal");
		AddEquipment("Hood", "Accessory","Helm",3.625f,"",	1.2f,0.1f,0,0.75f,0,2,0,0,0,0,3,"Leather","Fabric");
		AddEquipment("Circlet", "Accessory","Helm",3.75f,"",	1,0.1f,0,1,0,0.5f,0,0,0,0,3,"Wood","Metal");
		AddEquipment("Armguard","Accessory","Gauntlet",4,"",1,0.1f,0.5f,0,0.5f,0,5.0f,0,0,0,2,"Leather","Metal");
		AddEquipment("Bangle", "Accessory","Gauntlet",4.25f,"",1,0.1f,0,1,0.5f,0,5.0f,0,0,0,3,"Wood","Metal");
		AddEquipment("Boots","Accessory","Boots",3.875f,"",	1.2f,0.5f,0,0,0.5f,0,0,5.0f,0,0,2,"Leather","Metal");
		AddEquipment("Leather Pouch","Accessory","Backpack",50,"Pouch made of leather large enough for 5 items",60,0,5,true);
		AddEquipment("Leather Backpack","Accessory","Backpack",175,"Backpack made of leather for gathering 10 items",50,1,5,true,0,1);

	}

	public void Materials(){
		//Wood
		AddItem ("Common Stick","Material","Log",10,"You see these wooden sticks almost everywhere.");//0-6
		AddItem ("Oak wood","Material","Log",14,"");
		AddItem ("Pine wood","Material","Log",16,"");
		AddItem ("Cypress","Material","Log",18,"");
		AddItem ("Birch","Material","Log",26,"");
		AddItem ("Bamboo","Material","Log",28,"");
		AddItem ("Ebony","Material","Log",32,"");

		//Ores
		AddItem ("Pebble","Material","Ore",4,"");//7-13
		AddItem ("Large stone","Material","Ore",10,"");
		AddItem ("Coal","Material","Ore",16,"");
		AddItem ("Copper ore","Material","Ore",20,"");
		AddItem ("Iron ore","Material","Ore",30,"");
		AddItem ("Silver ore","Material","Ore",40,"");
		AddItem ("Gold ore","Material","Ore",45,"");

		//Gemstones
		AddItem ("Turquoise","Material","Gemstone",36,"");//14-20
		AddItem ("Lapis Lazuli","Material","Gemstone",36,"");
		AddItem ("Amethyst","Material","Gemstone",50,"");
		AddItem ("Emerald","Material","Gemstone",50,"");
		AddItem ("Sapphire","Material","Gemstone",70,"");
		AddItem ("Ruby","Material","Gemstone",70,"");
		AddItem ("Diamond","Material","Gemstone",110,"");

		//Flowers
		AddItem ("Lily","Material","Flower",8,"");//21-27
		AddItem ("Lavender","Material","Flower",8,"Gives a strong and pleasant fragrance.");
		AddItem ("Chamomile","Material","Flower",10,"Flowers frequently used in tea and medicine.");
		AddItem ("Saffron","Material","Flower",12,"These flowers can also be used as spices.");
		AddItem ("Orchid","Material","Flower",14,"Very colourful and often fragrant flower.");
		AddItem ("Helianthus","Material","Flower",16,"These flowers are also known as the sunflower.");
		AddItem ("Rose","Material","Flower",20,"Look out for the thorns!");

		//Fruits
		AddItem ("Apple","Material","Fruit",10,"");//28-34
		AddItem ("Orange","Material","Fruit",10,"");
		AddItem ("Banana","Material","Fruit",12,"");
		AddItem ("Peach","Material","Fruit",12,"");
		AddItem ("Grape","Material","Fruit",14,"");
		AddItem ("Strawberry","Material","Fruit",20,"");
		AddItem ("Pineapple","Material","Fruit",30,"");

		//Herbs 
		AddItem ("Mushroom","Material","Herb",8,"");//35-41
		AddItem ("Ginger","Material","Herb",10,"");
		AddItem ("Wolfberry","Material","Herb",10,"");
		AddItem ("Cinnamon","Material","Herb",11,"");
		AddItem ("Licorice","Material","Herb",12,"");
		AddItem ("Rhubarb","Material","Herb",14,"");
		AddItem ("Ginseng","Material","Herb",17,"");

		//Meat
		AddItem ("Poultry","Material","Meat",8,"");//42-48
		AddItem ("Animal meat","Material","Meat",9,"");
		AddItem ("Pork","Material","Meat",10,"");
		AddItem ("Beef","Material","Meat",10,"");
		AddItem ("Venison","Material","Meat",12,"");
		AddItem ("Fish","Material","Meat",14,"");
		AddItem ("Shellfish","Material","Meat",17,"");

		//Skin
		AddItem ("Bird Feather","Material","Skin",8,"");//49-55
		AddItem ("Bird Down","Material","Skin",10,"");
		AddItem ("Animal pelt","Material","Skin",10,"");
		AddItem ("Animal skin","Material","Skin",11,"");
		AddItem ("Animal hide","Material","Skin",12,"");
		AddItem ("Animal fur","Material","Skin",14,"");
		AddItem ("Animal scale","Material","Skin",17,"");

		//Claws
		AddItem ("Bird Talon","Material","Claws",8,"");//56-62
		AddItem ("Predator's talon","Material","Claws",10,"");
		AddItem ("Lizard claws","Material","Claws",10,"");
		AddItem ("Large reptilian claws","Material","Claws",11,"");
		AddItem ("Humanoid nails","Material","Claws",12,"");
		AddItem ("Animal hoof","Material","Claws",14,"");
		AddItem ("Beast claws","Material","Claws",17,"");

		//Bones
		AddItem ("Bird beak","Material","Bone",8,"");//63-69
		AddItem ("Lizard teeth","Material","Bone",10,"");
		AddItem ("Beast fang","Material","Bone",10,"");
		AddItem ("Ivory","Material","Bone",11,"");
		AddItem ("Antler","Material","Bone",12,"");
		AddItem ("Animal horn","Material","Bone",14,"");
		AddItem ("Large animal bone","Material","Bone",17,"");
	}
	


	void AddItem(string name, string type,string subtype, int money, string description){
		Dictionary<string,int> stats=null;
		itemList.Add (new Item (itemList.Count, name, type, money, description,-1,subtype,"", stats));
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
			shopList [shop].AddItem (level, itemList.Count-1);
	}
	void AddMaterial(string name, string type, string subtype, int money, string description,float durability, float weight, float physatk, float magatk,float physdef,float magdef,float acc, float eva,float block){
		Dictionary<string,float> stats=new Dictionary<string,float>();
		stats["PAttack"]=physatk;
		stats["PDefense"]=physdef;
		stats["MAttack"]=magatk;
		stats["MDefense"]=magdef;
		stats["Accuracy"]=acc;
		stats["Evade"]=eva;
		stats["Block"]=block;
		stats["Durability"]=durability;
		stats["Weight"]=weight;
		stats["Money"]=money;
		baseMaterial.Add (new Item (itemList.Count, name, type, description,subtype, stats));
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
			shopList [shop].AddItem (level, itemList.Count-1);
	}



	void AddEquipment(string name,string type, string subtype,float money,string description,float durability, float weight, float pattack, float mattack, float pdefense, float mdefense,float acc, float eva, float block, float blockchance,int shop, string materialType1, string materialType2=""){
		Dictionary<string,float> stats= new Dictionary<string,float>();
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
		if (durability>0){
			stats["Durability"]=durability;
		}
		if (weight>0){
			stats["Weight"]=weight;
		}
		if (money>0){
			stats["Money"]=money;
		}
		baseItem.Add (new Item (itemList.Count, name, type, description,subtype, stats));
		CombineEquipment(baseItem.Last(),materialType1,materialType2,shop);
	}

	public void AddShop (string name, string description)
	{
		shopList.Add (new Shop (shopList.Count, name, description));

	}

	public Item FindItem (int id)
	{
		for (int i=0;i<itemList.Count;i++){
			if (itemList[i].id == id)
				return itemList[i];
		}
		return null;
	}

	public List<Item> FindItems(string type){
		List<Item>items=new List<Item>();
		for (int i=0;i<itemList.Count;i++){
			if (itemList[i].type == type)
				items.Add(itemList[i]);
		}
		return items;
	}

	public int FindItemId(string name){
		for (int i=0;i<itemList.Count;i++){
			if (itemList[i].name == name)
				return itemList[i].id;
		}
		return 0;

	}
	public List<int> FindGatheringItems(string subtype){
		List<int>items=new List<int>();
		for (int i=0;i<itemList.Count;i++){
			if (itemList[i].subType == subtype)
				items.Add(itemList[i].id);
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

	/*public List<int> GetShopList (string shop, int level)
	{
		List<int> shoppinglist = new List<int> ();

		foreach (KeyValuePair<int,List<int>> list in shopList[shop].itemList) {
			if (level >= list.Key) {
				foreach (int item in list.Value) {
					shoppinglist.Add (item);
				}
			}
		}
		return shoppinglist;
	}*/

	public Shop GetShop(int id){
		foreach (Shop shop in shopList) {
			if (shop.id == id)
				return shop;
		}
		return null;

	}
	public void CombineEquipment(Item item, string material1, string material2, int shop){
		foreach( Item material in baseMaterial){
			if (material.subType==material1 ||material.subType==material2){
				itemList.Add (new Item (itemList.Count,item,material));
				shopList [shop].AddItem (tier[material.name], itemList.Count-1);
			}
		}
	}
}
