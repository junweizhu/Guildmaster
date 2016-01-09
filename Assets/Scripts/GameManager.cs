using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	public Guild
		myGuild;
	private Member myUnit;
	private GameObject database;
	public GuildScreenDisplay guildScreen;
	public MemberScreenDisplay memberScreen;
	public QuestScreenDisplay questScreen;
	public WarehouseScreenDisplay warehouseScreen;
	public TavernScreenDisplay tavernScreen;
	public SearchScreenDisplay searchScreen;
	public MarketScreenDisplay marketScreen;
	public OutsideScreenDisplay outsideScreen;
	public StatScreenDisplay memberStatScreen;
	public ItemStatScreenDisplay itemStatScreen;
	public QuestStatScreenDisplay questStatScreen;
	public ShopScreenDisplay shopScreen;
	public MemberSelectScreenDisplay selectScreen;
	public NextDayScreenDisplay nextDayScreen;
	public CanvasGroup gameMenu;
	private ItemDatabase idb;
	private CharDatabase cdb;
	private StringDatabase sdb;
	private GuildDatabase gdb;
	private AreaDatabase adb;
	public string screenDisplay;
	[SerializeField]
	private int
		statDisplay;
	private string nextAction;
	private int day=1;

	// Use this for initialization
	void Start ()
	{
		database = GameObject.Find ("Database");
		idb = database.GetComponent<ItemDatabase> ();
		cdb = database.GetComponent<CharDatabase> ();
		sdb = database.GetComponent<StringDatabase> ();
		gdb = database.GetComponent<GuildDatabase> ();
		adb = database.GetComponent<AreaDatabase> ();
		myGuild = gdb.FindGuild (0);
		myGuild.RecruitMember (cdb.GetMember (0));
		myGuild.RecruitMember (cdb.GetMember (1));
		myGuild.inventory.AddItem (0);
		myGuild.inventory.AddItem (1, 50);
		myGuild.memberlist [0].AddItem (idb.FindItem (0), 100);
		myGuild.memberlist [0].AddItem (idb.FindItem (0), 100);
		myGuild.memberlist [0].AddItem (idb.FindItem (0), 100);
		myGuild.memberlist [0].AddItem (idb.FindItem (0), 100);
		myGuild.memberlist [0].AddItem (idb.FindItem (0), 100);
		myGuild.memberlist [1].AddItem (idb.FindItem (1), 100);
		myGuild.questBoard.AddQuest (1, 1);
		myGuild.questBoard.AddQuest (0, 1);
		myGuild.FindNewArea(adb.FindArea(0));
		screenDisplay="Guild";
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (myGuild != null) {

			if (screenDisplay == "Guild" && guildScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (guildScreen.GetComponent<CanvasGroup> (), true);
				guildScreen.UpdateText (myGuild.name, myGuild.level, myGuild.exp, myGuild.fame, myGuild.size, myGuild.money);
			} else if (screenDisplay != "Guild") {
				DisplayScreen (guildScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Memberlist" && memberScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (memberScreen.GetComponent<CanvasGroup> (), true);
				memberScreen.UpdateText (myGuild.memberlist);

			} else if (screenDisplay != "Memberlist") {
				DisplayScreen (memberScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Warehouse" && warehouseScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (warehouseScreen.GetComponent<CanvasGroup> (), true);
				warehouseScreen.UpdateText (myGuild.inventory);
			} else if (screenDisplay != "Warehouse") {
				DisplayScreen (warehouseScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Questlist" && questScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (questScreen.GetComponent<CanvasGroup> (), true);
				questScreen.UpdateText (myGuild.questBoard);
			} else if (screenDisplay != "Questlist") {
				DisplayScreen (questScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Tavern" && tavernScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (tavernScreen.GetComponent<CanvasGroup> (), true);
				tavernScreen.UpdateText (database.GetComponent<CharDatabase> ().GetRecruitables ());
			} else if (screenDisplay != "Tavern") {
				DisplayScreen (tavernScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Market" && marketScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (marketScreen.GetComponent<CanvasGroup> (), true);
				
			} else if (screenDisplay != "Market") {
				DisplayScreen (marketScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Outside" && outsideScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (outsideScreen.GetComponent<CanvasGroup> (), true);
				outsideScreen.UpdateText (myGuild);
			} else if (screenDisplay != "Outside") {
				DisplayScreen (outsideScreen.GetComponent<CanvasGroup> (), false);
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			CloseGame();
		}
	}

	public void DisplayScreen (CanvasGroup screen, bool display)
	{
		if (display == true) {
			if (screen.GetComponentInChildren<ScrollRect> () != null) {
				screen.GetComponentInChildren<ScrollRect> ().normalizedPosition = new Vector2 (0, 1);
			}

		} 
		EnableScreen(screen,display);
		screen.alpha = Convert.ToInt32(display);

	}

	public void EnableScreen(CanvasGroup screen,bool enable)
	{
		screen.blocksRaycasts = enable;
		screen.interactable=enable;
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
			memberStatScreen.FillSlot (statDisplay + 1, myGuild.memberlist [statDisplay]);
			memberStatScreen.playerType = "member";
		}

	}

	public void NextMember ()
	{
		if (memberStatScreen.playerType == "member") {
			if (statDisplay < myGuild.memberlist.Count - 1) {
				statDisplay += 1;
				memberStatScreen.FillSlot (statDisplay + 1, myGuild.memberlist [statDisplay]);
			}
		} else if (memberStatScreen.playerType == "recruit") {
			if (statDisplay < database.GetComponent<CharDatabase> ().GetRecruitables ().Count - 1) {
				statDisplay += 1;
				memberStatScreen.FillSlot (statDisplay + 1, cdb.GetRecruitables () [statDisplay]);
			}
		}
	}

	public void PreviousMember ()
	{

		if (memberStatScreen.playerType == "member") {
			if (statDisplay > 0) {
				statDisplay -= 1;
				memberStatScreen.FillSlot (statDisplay + 1, myGuild.memberlist [statDisplay]);
			}
		}
	}

	public void RecruitMember (int id)
	{
		myGuild.RecruitMember (cdb.GetMember (id));
	}

	public void OpenShop (string shop)
	{
		if (myGuild.GetAvailableMembers ().Count > 0) {
			shopScreen.show = true;
			shopScreen.UpdateText (shop, idb.GetShopList (shop, myGuild.level));
		}
	}

	public void OpenSelectScreen (string action)
	{
		int maxSelection = 0;
		int id = 0;
		if (action == "Shop") {
			EnableScreen(shopScreen.GetComponent<CanvasGroup> (),false);
			maxSelection = 5;
			id = shopScreen.firstId;
		}

		if (action == "Quest") {
			maxSelection = int.Parse (questStatScreen.questMaxParticipants.text);
			id = questStatScreen.firstId;
		}
		selectScreen.UpdateText (myGuild.GetAvailableMembers (), sdb.GetString (action + "Select"), maxSelection, id);
		nextAction=action;
		selectScreen.show = true;
	}

	public void OpenSearchScreen (string action)
	{
		nextAction = action;
		searchScreen.UpdateText (myGuild.GetAvailableMembers (), sdb.GetString (action+"Title"),sdb.GetString (action), 5,action);
		searchScreen.show = true;
	}

	public void ReturnToLastScreen ()
	{
		selectScreen.show = false;
		searchScreen.show = false;
		if (nextAction == "Shop") {
			EnableScreen(shopScreen.GetComponent<CanvasGroup> (),true);
		}
	}

	public void SelectMembers ()
	{
		List<Member> selectedmembers = new List<Member> ();
		List<SlotInfo> members = new List<SlotInfo> ();
		if (selectScreen.show) {
			members = selectScreen.selectedMembers;
		} else if (searchScreen.show) {
			members = searchScreen.selectedMembers;
		}

		foreach (SlotInfo member in members) {
			selectedmembers.Add (myGuild.GetMember (member.id));
		}
		if (nextAction == "Shop") {
			shopScreen.UpdateBuyer (selectedmembers, selectScreen.ShowFirstBuyer (), myGuild.GetMember (selectScreen.ShowFirstBuyer ()).name);
		} else if (nextAction == "Quest") {
			questStatScreen.FillParticipants (selectedmembers);
		} else if (nextAction == "SearchRecruit"|| nextAction=="SearchQuest") {
			myGuild.AddTask (nextAction, 0.5f, selectedmembers, searchScreen.searchType.name);
			for (int i=0; i<selectedmembers.Count; i++) {
				selectedmembers [i].status = sdb.GetString (nextAction+"ing");
			}
		} else if(nextAction=="Adventure"){
			myGuild.AddTask (nextAction, outsideScreen.GetSelectedArea(), selectedmembers, searchScreen.searchType.name);
			for (int i=0; i<selectedmembers.Count; i++) {
				selectedmembers [i].status = sdb.GetString (searchScreen.searchType.name)+ " "+outsideScreen.areaName.text;
			}
		}
		ReturnToLastScreen ();
	}

	public void BuyStuff ()
	{
		if (shopScreen.BuyingSomething ()) {
			for (int i=0; i<shopScreen.buyers.Count; i++) {
				shopScreen.buyers [i].status = sdb.GetString ("Shopping");
			}
			myGuild.AddTask ("Shop", 0.5f, shopScreen.buyers, shopScreen.GetItemToBuy (),shopScreen.TotalCost());
			shopScreen.show = false;
		}
	}

	public void StartQuest ()
	{
		myGuild.questBoard.questParticipants [statDisplay] = questStatScreen.participants;
		for (int i=0; i< questStatScreen.participants.Count; i++) {
			questStatScreen.participants [i].status = sdb.GetString ("Questing") + " \"" + questStatScreen.quest.name + "\"";
		}
		myGuild.AddTask ("Quest", questStatScreen.quest.duration, questStatScreen.participants, myGuild.questBoard.questList[statDisplay].id,statDisplay);
		myGuild.questBoard.questStatus [statDisplay] = sdb.GetString ("Ongoing");
		CloseQuestStatScreen();
	}
	public void CloseQuestStatScreen()
	{
		questScreen.UpdateText (myGuild.questBoard);
		questStatScreen.show = false;
	}

	public void DisplayQuestStats (SlotInfo slot)
	{
		statDisplay = int.Parse (slot.slotNumber.text) - 1;
		questStatScreen.questBoard = myGuild.questBoard;
		questStatScreen.myGuild=myGuild;
		questStatScreen.questNumber.text = slot.slotNumber.text;
		questStatScreen.quest = myGuild.questBoard.questList [statDisplay];
		questStatScreen.refresh = true;
		questStatScreen.show = true;
	}

	public void NextDay()
	{
		myGuild.UpdateTasks(day);
		myGuild.UpdateMembers();
		if (myGuild.taskLog.ContainsKey(day))
		{
			nextDayScreen.UpdateText(day,myGuild.taskLog[day],myGuild.GetMemberActivity());
		}
		else
			nextDayScreen.UpdateText(day,null,myGuild.GetMemberActivity());
		day+=1;
		nextDayScreen.show=true;
	}
	public void ReturnToMainScreen()
	{
		myGuild.NextDayReset();
		nextDayScreen.show=false;
		screenDisplay="Guild";
		guildScreen.GetComponent<CanvasGroup> ().alpha = 0;
	}
	public void FinishQuest(int questid)
	{
		myGuild.FinishQuest(questid,null);
		myGuild.questBoard.RemoveQuest (questid);
		CloseQuestStatScreen();
	}

	public void CloseGame()
	{
		Application.Quit();
	}
}
