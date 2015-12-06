using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private Guild myGuild;
	private Member myUnit;
	private GameObject database;
	public GuildScreenDisplay guildScreen;
	public MemberScreenDisplay memberScreen;
	public QuestScreenDisplay questScreen;
	public WarehouseScreenDisplay warehouseScreen;
	public TavernScreenDisplay tavernScreen;
	public RecruitScreenDisplay recruitScreen;
	public MarketScreenDisplay marketScreen;
	public StatScreenDisplay memberStatScreen;
	public ItemStatScreenDisplay itemStatScreen;
	public ShopScreenDisplay shopScreen;
	public BuyerScreenDisplay buyerScreen;
	public string screenDisplay = "Guild";
	[SerializeField]
	private int statDisplay;

	// Use this for initialization
	void Start ()
	{
		database = GameObject.Find ("Database");
		myGuild = database.GetComponent<GuildDatabase> ().FindGuild (0);
		myGuild.RecruitMember (database.GetComponent<CharDatabase> ().GetMember (0));
		myGuild.RecruitMember (database.GetComponent<CharDatabase> ().GetMember (1));
		myGuild.guildInventory.AddItem (0);
		myGuild.guildInventory.AddItem (1, 50);
		myGuild.guildMemberlist [0].AddItem (database.GetComponent<ItemDatabase> ().FindItem (0), 100);
		myGuild.guildMemberlist [0].AddItem (database.GetComponent<ItemDatabase> ().FindItem (0), 100);
		myGuild.guildMemberlist [0].AddItem (database.GetComponent<ItemDatabase> ().FindItem (0), 100);
		myGuild.guildMemberlist [0].AddItem (database.GetComponent<ItemDatabase> ().FindItem (0), 100);
		myGuild.guildMemberlist [0].AddItem (database.GetComponent<ItemDatabase> ().FindItem (0), 100);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (myGuild != null) {

			if (screenDisplay == "Guild" && guildScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (guildScreen.GetComponent<CanvasGroup> (), true);
				guildScreen.UpdateText (myGuild.guildName, myGuild.guildLevel, myGuild.guildExp, myGuild.guildFame, myGuild.guildSize, myGuild.guildMoney);
			} else if (screenDisplay != "Guild") {
				DisplayScreen (guildScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Memberlist" && memberScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (memberScreen.GetComponent<CanvasGroup> (), true);
				memberScreen.UpdateText (myGuild.guildMemberlist);

			} else if (screenDisplay != "Memberlist") {
				DisplayScreen (memberScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Warehouse" && warehouseScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (warehouseScreen.GetComponent<CanvasGroup> (), true);
				warehouseScreen.UpdateText (myGuild.guildInventory);
			} else if (screenDisplay != "Warehouse") {
				DisplayScreen (warehouseScreen.GetComponent<CanvasGroup> (), false);
			}
			/*if(screenDisplay=="Questlist"&&questScreen.GetComponent<CanvasGroup>().alpha==false){
				DisplayScreen(questScreen.GetComponent<CanvasGroup>(),true);
				questScreen.UpdateText(myGuild.guildQuestList);
			}
			else if(screenDisplay!="Questlist")
			{
				DisplayScreen(questScreen.GetComponent<CanvasGroup>(),false);
			}*/
			if (screenDisplay == "Tavern" && tavernScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (tavernScreen.GetComponent<CanvasGroup> (), true);
				tavernScreen.UpdateText (database.GetComponent<CharDatabase> ().GetRecruitables ());
			} else if (screenDisplay != "Tavern") {
				DisplayScreen (tavernScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Recruit" && recruitScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (recruitScreen.GetComponent<CanvasGroup> (), true);
				recruitScreen.UpdateText (database.GetComponent<CharDatabase> ().GetRecruitables ());

			} else if (screenDisplay != "Recruit") {
				DisplayScreen (recruitScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Market" && recruitScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (marketScreen.GetComponent<CanvasGroup> (), true);
				
			} else if (screenDisplay != "Market") {
				DisplayScreen (marketScreen.GetComponent<CanvasGroup> (), false);
			}
		}
	}

	private void DisplayScreen (CanvasGroup screen, bool display)
	{
		if (display == true) {
			if (screen.GetComponentInChildren<ScrollRect> () != null) {
				screen.GetComponentInChildren<ScrollRect> ().normalizedPosition = new Vector2 (0, 1);
			}
			screen.alpha = 1;
			screen.blocksRaycasts = true;
		} else {
			screen.alpha = 0;
			screen.blocksRaycasts = false;
		}

	}

	public void SwitchScreen (string name)
	{
		screenDisplay = name;
	}

	public void BackScreen ()
	{
		if (screenDisplay == "Memberlist" || screenDisplay == "Warehouse" || screenDisplay == "Questlist") {
			screenDisplay = "Guild";
			return;
		}

	}

	public void DisplayMemberStats (SlotInfo slot)
	{
		statDisplay = int.Parse (slot.slotNumber.text) - 1;

		if (slot.name.Contains ("Member")) {
			memberStatScreen.show = true;
			memberStatScreen.FillSlot (statDisplay + 1, myGuild.guildMemberlist [statDisplay]);
			memberStatScreen.playerType = "member";
		}
		else if (slot.name.Contains ("Recruit")) 
		{

			memberStatScreen.show = true;
			memberStatScreen.FillSlot (statDisplay + 1, database.GetComponent<CharDatabase> ().GetRecruitables () [statDisplay]);
			memberStatScreen.playerType = "recruit";

		}

	}

	public void NextMember ()
	{
		if (memberStatScreen.playerType == "member") {
			if (statDisplay < myGuild.guildMemberlist.Count - 1) {
				statDisplay += 1;
				memberStatScreen.FillSlot (statDisplay + 1, myGuild.guildMemberlist [statDisplay]);
			}
		} else if (memberStatScreen.playerType == "recruit") {
			if (statDisplay < database.GetComponent<CharDatabase> ().GetRecruitables ().Count - 1) {
				statDisplay += 1;
				memberStatScreen.FillSlot (statDisplay + 1, database.GetComponent<CharDatabase> ().GetRecruitables () [statDisplay]);
			}
		}
	}

	public void PreviousMember ()
	{

		if (memberStatScreen.playerType == "member") {
			if (statDisplay > 0) {
				statDisplay -= 1;
				memberStatScreen.FillSlot (statDisplay + 1, myGuild.guildMemberlist [statDisplay]);
			}
		} else if (memberStatScreen.playerType == "recruit") {
			if (statDisplay > 0) {
				statDisplay -= 1;
				memberStatScreen.FillSlot (statDisplay + 1, database.GetComponent<CharDatabase> ().GetRecruitables () [statDisplay]);
			}
		}
	}

	public void DisplayItemStats (SlotInfo itemslot)
	{
		int quantity = 1;
		if (itemslot.slotQuantity != null) {
			quantity = int.Parse (itemslot.slotQuantity.text);
		}
		itemStatScreen.show = true;
		itemStatScreen.FillSlot (database.GetComponent<ItemDatabase> ().FindItem (itemslot.id), int.Parse (itemslot.slotQuality.text), quantity);
	}

	public void RecruitMember(int id)
	{
		myGuild.RecruitMember(database.GetComponent<CharDatabase>().GetMember(id));
		recruitScreen.UpdateText (database.GetComponent<CharDatabase> ().GetRecruitables ());
	}
	public void OpenShop(string shop)
	{
		if(myGuild.GetAvailableMembers().Count>0)
		{
		shopScreen.show=true;
		shopScreen.UpdateText(shop,myGuild.GetAvailableMembers()[0],database.GetComponent<ItemDatabase>().GetShopList(shop));
		}
	}
	public void OpenBuyerScreen()
	{
		shopScreen.GetComponent<CanvasGroup>().interactable=false;
		buyerScreen.show=true;
		buyerScreen.UpdateText(myGuild.GetAvailableMembers(),database.GetComponent<SkillDatabase>().GetSkill(4),shopScreen.buyerId);
	}
	public void ReturnToShop()
	{
		buyerScreen.show=false;
		shopScreen.GetComponent<CanvasGroup>().interactable=true;
	}
	public void SelectBuyer()
	{
		shopScreen.UpdateBuyer(database.GetComponent<CharDatabase>().GetMember(buyerScreen.buyerId));
		ReturnToShop();
	}
	public void BuyStuff()
	{
		if (shopScreen.BuyingSomething())
		{
			shopScreen.buyer.shopList=shopScreen.GetItemToBuy();
			shopScreen.buyer.memberStatus="Shopping";
			shopScreen.show=false;
		}
	}
}
