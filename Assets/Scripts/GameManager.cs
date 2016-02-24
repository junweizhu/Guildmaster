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
	private Character myUnit;
	public GuildScreenDisplay guildScreen;
	public CharacterScreenDisplay characterScreen;
	public CharacterScreenDisplay recruitScreen;
	public QuestScreenDisplay questScreen;
	public QuestScreenDisplay requestScreen;
	public WarehouseScreenDisplay warehouseScreen;
	public UpgradeScreenDisplay upgradeScreen;
	public TavernScreenDisplay tavernScreen;
	public SearchScreenDisplay searchScreen;
	public MarketScreenDisplay marketScreen;
	public OutsideScreenDisplay outsideScreen;
	public CharacterStatScreenDisplay characterStatScreen;
	public ItemStatScreenDisplay itemStatScreen;
	public QuestStatScreenDisplay questStatScreen;
	public ShopScreenDisplay shopScreen;
	public CharacterSelectScreenDisplay selectScreen;
	public NextDayScreenDisplay nextDayScreen;
	public ItemSelectScreenDisplay itemSelectScreen;
	public PromptScreenDisplay promptScreen;
	public DialogueScreenDisplay dialogueScreen;
	public CanvasGroup gameMenu;
	public GameEvent gameEvent;
	public string screenDisplay;
	[SerializeField]
	private int
		statDisplayId;
	private string nextAction;
	private int day = 1;
	private int month = 1;
	private int year = 1;
	private SaveData data;

	// Use this for initialization
	void Start ()
	{
		Application.targetFrameRate = 15;
		if (true) {
			Database.Initialize ();
			myGuild = Database.guilds.FindGuild (0);
			myGuild.RecruitCharacter (Database.characters.GetCharacter (0));
			myGuild.RecruitCharacter (Database.characters.GetCharacter (1));
			myGuild.inventory.AddItem (0);
			myGuild.inventory.AddItem (6);
			myGuild.inventory.AddItem (14);
			myGuild.inventory.AddItem (20);
			Database.quests.GenerateQuest (0, 1, "main");
			Database.quests.GenerateQuest (1, 1, "main");
			myGuild.questBoard.AddQuest (1);
			myGuild.questBoard.AddQuest (0);
			myGuild.FindNewArea (0);
		}
		screenDisplay = "Guild";
		Database.myGuild = myGuild;
		Database.game=this;
		guildScreen.GetComponent<CanvasGroup> ().alpha = 0;

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (myGuild != null) {

			if (screenDisplay == "Guild" && guildScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (guildScreen.GetComponent<CanvasGroup> (), true);
				guildScreen.UpdateText (day, month, year);
			} else if (screenDisplay != "Guild") {
				DisplayScreen (guildScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Characterlist" && characterScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (characterScreen.GetComponent<CanvasGroup> (), true);
				characterScreen.UpdateText ();
			} else if (screenDisplay != "Characterlist") {
				DisplayScreen (characterScreen.GetComponent<CanvasGroup> (), false);
			}

			if (screenDisplay == "Warehouse" && warehouseScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (warehouseScreen.GetComponent<CanvasGroup> (), true);
				warehouseScreen.UpdateText ();
			} else if (screenDisplay != "Warehouse") {
				DisplayScreen (warehouseScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Questlist" && questScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (questScreen.GetComponent<CanvasGroup> (), true);
				questScreen.UpdateText ();
			} else if (screenDisplay != "Questlist") {
				DisplayScreen (questScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Upgrade" && upgradeScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (upgradeScreen.GetComponent<CanvasGroup> (), true);
				upgradeScreen.UpdateText ();
			} else if (screenDisplay != "Upgrade") {
				DisplayScreen (upgradeScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Tavern" && tavernScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (tavernScreen.GetComponent<CanvasGroup> (), true);
				tavernScreen.UpdateText (Database.characters.GetRecruitables ().Count, Database.quests.AvailableQuests ().Count);
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
				outsideScreen.UpdateText ();
			} else if (screenDisplay != "Outside") {
				DisplayScreen (outsideScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Recruit" && outsideScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (recruitScreen.GetComponent<CanvasGroup> (), true);
				recruitScreen.UpdateText ();
			} else if (screenDisplay != "Recruit") {
				DisplayScreen (recruitScreen.GetComponent<CanvasGroup> (), false);
			}
			if (screenDisplay == "Request" && requestScreen.GetComponent<CanvasGroup> ().alpha == 0) {
				DisplayScreen (requestScreen.GetComponent<CanvasGroup> (), true);
				requestScreen.UpdateText ();
			} else if (screenDisplay != "Request") {
				DisplayScreen (requestScreen.GetComponent<CanvasGroup> (), false);
			}
		}
		CheckEvent ();
		if (Input.GetKeyDown (KeyCode.Escape)) {
			CloseGame ();
		}
	}

	public void DisplayScreen (CanvasGroup screen, bool display)
	{
		if (display == true) {
			if (screen.GetComponentInChildren<ScrollRect> () != null) {
				screen.GetComponentInChildren<ScrollRect> ().normalizedPosition = new Vector2 (0, 1);
			}

		} 
		EnableScreen (screen, display);
		screen.alpha = Convert.ToInt32 (display);

	}

	public void EnableScreen (CanvasGroup screen, bool enable)
	{
		screen.blocksRaycasts = enable;
		screen.interactable = enable;
	}

	public void SwitchScreen (string name)
	{
		screenDisplay = name;
	}

	public void BackScreen ()
	{
		if (screenDisplay == "Characterlist" || screenDisplay == "Warehouse" || screenDisplay == "Questlist") {
			screenDisplay = "Guild";
			return;
		}
	}

	public void DisplayCharacterStats (SlotInfo slot)
	{
		statDisplayId = slot.id;

		if (slot.name.Contains ("Recruit")) {
			characterStatScreen.playerType = "Recruit";
		} else {
			characterStatScreen.playerType = "Character";
		}
		nextAction = "Character";
		characterStatScreen.FillSlot (statDisplayId);
		characterStatScreen.show = true;
	}

	public void NextCharacter ()
	{
		statDisplayId += 1;
		characterStatScreen.FillSlot (statDisplayId);
	}

	public void PreviousCharacter ()
	{
		statDisplayId -= 1;
		characterStatScreen.FillSlot (statDisplayId);
	}

	public void RecruitCharacter (int id)
	{
		if (id != 999) {
			myGuild.RecruitCharacter (Database.characters.GetRecruitables () [id]);
		} else {
			myGuild.RecruitCharacter (Database.characters.GetRecruitables () [statDisplayId]);
		}
		characterStatScreen.CloseScreen ();
	}

	public void AcceptQuest (int id)
	{
		if (id != 999) {
			myGuild.questBoard.AddQuest (id);
		} else {
			myGuild.questBoard.AddQuest (statDisplayId);
		}
		CloseQuestStatScreen ();
	}

	public void OpenShop (int id)
	{
		if (myGuild.GetAvailableCharacters ().Count > 0) {
			shopScreen.guildMoney = myGuild.money;
			Shop shop = Database.items.GetShop (id);
			if (shop == null) {
				shopScreen.UpdateText (shop, false);
			} else {
				shopScreen.UpdateText (shop);
			}
			shopScreen.show = true;
		}
	}

	public void OpenSelectScreen (string action)
	{
		int maxSelection = 0;
		if (action == "Shop") {
			maxSelection = 5;
		}

		if (action == "Quest") {
			maxSelection = int.Parse (questStatScreen.questMaxParticipants.text);
		}
		if (action == "Socialize") {
			maxSelection = 5;

		}
		selectScreen.UpdateText (myGuild.GetAvailableCharacters (), Database.strings.GetString (action + "Select"), maxSelection);
		nextAction = action;
		selectScreen.show = true;
	}

	public void OpenSearchScreen (string action)
	{
		nextAction = action;
		searchScreen.UpdateText (myGuild.GetAvailableCharacters (), Database.strings.GetString (action + "Title"), Database.strings.GetString (action), 5, action);
		searchScreen.show = true;
	}

	public void ReturnToLastScreen ()
	{
		selectScreen.show = false;
		searchScreen.show = false;
		itemSelectScreen.show = false;
	}

	public void SelectCharacters ()
	{
		List<Character> selectedcharacters = new List<Character> ();
		if (selectScreen.show) {
			selectedcharacters = GetSelectedCharacters (selectScreen.selectedCharacters);
		} else if (searchScreen.show) {
			selectedcharacters = GetSelectedCharacters (searchScreen.selectedCharacters);
		}
		if (nextAction == "Shop") {
			shopScreen.UpdateBuyer (selectedcharacters);
		} else if (nextAction == "Quest") {
			questStatScreen.FillParticipants (selectedcharacters);
		} else if (nextAction == "Adventure" || nextAction == "Socialize") {
			promptScreen.Prompt (nextAction);
			return;
		}
		ReturnToLastScreen ();
	}

	public void GoOnAdventure ()
	{
		List<Character> selectedcharacters = new List<Character> ();
		if (selectScreen.show) {
			selectedcharacters = GetSelectedCharacters (selectScreen.selectedCharacters);
		} else if (searchScreen.show) {
			selectedcharacters = GetSelectedCharacters (searchScreen.selectedCharacters);
		}
		myGuild.AddTask (new Task (nextAction, outsideScreen.GetSelectedArea (), selectedcharacters, searchScreen.searchType.name));
		for (int i=0; i<selectedcharacters.Count; i++) {
			selectedcharacters [i].status = searchScreen.searchType.name;
			selectedcharacters [i].statusAdd = outsideScreen.GetSelectedArea ().name;
		}
		ReturnToLastScreen ();
	}

	public void GoToTavern ()
	{
		List<Character> selectedcharacters = new List<Character> ();
		if (selectScreen.show) {
			selectedcharacters = GetSelectedCharacters (selectScreen.selectedCharacters);
		} else if (searchScreen.show) {
			selectedcharacters = GetSelectedCharacters (searchScreen.selectedCharacters);
		}
		myGuild.AddTask (new Task (nextAction, 0.5f, selectedcharacters));
		for (int i=0; i<selectedcharacters.Count; i++) {
			selectedcharacters [i].status = nextAction + "ing";
		}
		ReturnToLastScreen ();
	}

	public List<Character> GetSelectedCharacters (List<SlotInfo> characters)
	{
		List<Character> selectedcharacters = new List<Character> ();
		foreach (SlotInfo character in characters) {
			selectedcharacters.Add (myGuild.GetCharacter (character.id));
		}
		return selectedcharacters;
	}
	public void Upgrade(){
		myGuild.Upgrade (upgradeScreen.lastSelected.id);
		upgradeScreen.UpdateText();
	}
	public void SelectItem ()
	{
		if (nextAction == "Character") {
			myGuild.GiveItemToCharacter (itemSelectScreen.selectedItem.id, itemSelectScreen.slotId, statDisplayId);
			characterStatScreen.FillSlot (statDisplayId);
		} else if (nextAction == "Adventure") {
			SelectCharacters ();
		} else if (nextAction == "Quest") {
			StartQuest ();
		}
		itemSelectScreen.show = false;
	}

	public void OpenItemSelect (int slotId)
	{
		itemSelectScreen.UpdateText (myGuild.inventory, Database.strings.GetString ("SelectItems"), slotId, myGuild.GetCharacter (statDisplayId));
		itemSelectScreen.show = true;
	}

	public void BuyOrSell ()
	{
		string status = "";
		if (shopScreen.BuyingOrSellingSomething ()) {
			string task = "";
			int money=0;
			if (shopScreen.buying) {
				status = "Shopping";
				task = "Shop";
				money=shopScreen.TotalCost();
			} else {
				status = "Selling";
				task = "Sell";
			}
			myGuild.AddTask (new Task (task, 1.0f, shopScreen.buyers, shopScreen.GetItemToBuyOrSell (),money));
		} else {
			SlotInfo slot = shopScreen.GetSelected ();
			Skill skill = null;
			Ability ability = null;
			if (slot.isAbility) {
				ability = Database.skills.GetAbility (slot.id);
			} else {
				skill = Database.skills.GetSkill (slot.id);
			}
			status="Studying";
			myGuild.AddTask (new Task ("School", 1.0f, shopScreen.buyers, skill, ability,slot.SetCost(shopScreen.buyers)));
		}
		for (int i=0; i<shopScreen.buyers.Count; i++) {
			shopScreen.buyers [i].status = status;
		}
		shopScreen.show = false;
	}

	public void StartQuest ()
	{
		questStatScreen.quest.participants = questStatScreen.participants;
		for (int i=0; i< questStatScreen.participants.Count; i++) {
			questStatScreen.participants [i].status = "Questing";
			questStatScreen.participants [i].statusAdd = questStatScreen.quest.name;
		}
		myGuild.AddTask (new Task ("Quest", questStatScreen.quest, questStatScreen.participants, statDisplayId));
		questStatScreen.quest.status = "Ongoing";
		CloseQuestStatScreen ();
	}

	public void CloseQuestStatScreen ()
	{
		if (screenDisplay == "Questlist") {
			questScreen.UpdateText ();
		} else if (screenDisplay == "Request") {
			requestScreen.UpdateText ();
		}
		questStatScreen.show = false;
	}

	public void DisplayQuestStats (SlotInfo slot)
	{
		statDisplayId = slot.id;
		questStatScreen.questBoard = myGuild.questBoard;
		questStatScreen.myGuild = myGuild;
		questStatScreen.questNumber.text = slot.slotNumber.text;
		questStatScreen.quest = Database.quests.FindQuest (statDisplayId);
		questStatScreen.refresh = true;
		questStatScreen.show = true;
	}

	public void NextDay ()
	{
		myGuild.UpdateTasks (day);
		myGuild.UpdateCharacters ();
		if (myGuild.tasklog.Count > 0) {
			nextDayScreen.UpdateText (day, month, year, myGuild.tasklog, myGuild.GetCharacterActivity ());
		} else
			nextDayScreen.UpdateText (day, month, year, null, myGuild.GetCharacterActivity ());
		day += 1;
		if (day > 30) {
			month += 1;
			day -= 30;
			if (month > 12) {
				year += 1;
				month -= 12;
			}
		}
		nextDayScreen.show = true;
		myGuild.NextDayReset ();
		SaveData ();
	}

	public void ReturnToMainScreen ()
	{
		nextDayScreen.show = false;
		screenDisplay = "Guild";
		guildScreen.GetComponent<CanvasGroup> ().alpha = 0;
	}

	public void FinishQuest (int questid)
	{
		myGuild.FinishQuest (questid, null);
		myGuild.questBoard.RemoveQuest (questid);
		CloseQuestStatScreen ();
	}

	public void CloseGame ()
	{
		Application.Quit ();
	}

	public void SaveData ()
	{
		SaveLoad.data.day = day;
		SaveLoad.data.month = month;
		SaveLoad.data.year = year;
		SaveLoad.data.guild = myGuild;
		SaveLoad.data.characters = Database.characters.GetCharacter ();
		SaveLoad.data.quests = Database.quests.GetQuest ();
		SaveLoad.Save ();
	}

	public bool LoadData ()
	{
		SaveLoad.Load ();
		if (SaveLoad.data.guild != null) {
			day = SaveLoad.data.day;
			month = SaveLoad.data.month;
			year = SaveLoad.data.year;
			myGuild = SaveLoad.data.guild;
			Database.characters.LoadCharacter (SaveLoad.data.characters);
			Database.quests.LoadQuest (SaveLoad.data.quests);
			return true;
		}
		return false;
	}

	public void CheckEvent ()
	{
		gameEvent = Database.events.GetActiveEvent ();
		if (gameEvent != null) {
			dialogueScreen.ShowDialogue (gameEvent, null);
		}
	}
}
