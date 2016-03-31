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
	public CanvasGroup startScreen;
	public CanvasGroup mainScreen;
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
	public TaskScreenDisplay taskScreen;
	public GuildStatusScreenDisplay statusScreen;
	public CharacterStatScreenDisplay characterStatScreen;
	public ItemStatScreenDisplay itemStatScreen;
	public QuestStatScreenDisplay questStatScreen;
	public ShopScreenDisplay shopScreen;
	public CharacterSelectScreenDisplay selectScreen;
	public NextDayScreenDisplay nextDayScreen;
	public ItemSelectScreenDisplay itemSelectScreen;
	public PromptScreenDisplay promptScreen;
	public PromptScreenDisplay textInputScreen;
	public DialogueScreenDisplay dialogueScreen;
	public TownHallScreenDisplay townhallScreen;
	public CanvasGroup gameMenu;
	public GameEvent gameEvent;
	public string screenDisplay;
	public Dictionary<string,CanvasGroup> displayScreen=new Dictionary<string,CanvasGroup>();
	public List<string> screenList;
	[SerializeField]
	private int
		statDisplayId;
	private string nextAction;
	private SaveData data;
	public Button loadButton;
	private WaitForSeconds waitTime=new WaitForSeconds(0.5f);
	private WaitForSeconds sellWaitTime=new WaitForSeconds(0.3f);
	private WaitForSeconds adventureWaitTime=new WaitForSeconds(0.05f);
	public List<Task> tasksWithCoroutine=new List<Task>();
	public CanvasGroup waitingScreen;

	// Use this for initialization
	void Start ()
	{
		Application.targetFrameRate = 15;
		screenDisplay="Start";
		DisplayScreen ("Start",startScreen);
		DisplayScreen ("Main",mainScreen);
		loadButton.interactable = SaveLoad.CheckIfSaveFileExists ();
		displayScreen.Add ("Guild", guildScreen.GetComponent<CanvasGroup> ());
		displayScreen.Add ("Status", statusScreen.GetComponent<CanvasGroup> ());
		displayScreen.Add ("Characterlist", characterScreen.GetComponent<CanvasGroup> ());
		displayScreen.Add ("Warehouse", warehouseScreen.GetComponent<CanvasGroup> ());
		displayScreen.Add ("Questlist", questScreen.GetComponent<CanvasGroup> ());
		displayScreen.Add ("Upgrade", upgradeScreen.GetComponent<CanvasGroup> ());
		displayScreen.Add ("Task", taskScreen.GetComponent<CanvasGroup> ());
		displayScreen.Add ("Tavern", tavernScreen.GetComponent<CanvasGroup> ());
		displayScreen.Add ("Market", marketScreen.GetComponent<CanvasGroup> ());
		displayScreen.Add ("Outside", outsideScreen.GetComponent<CanvasGroup> ());
		displayScreen.Add ("Recruit", recruitScreen.GetComponent<CanvasGroup> ());
		displayScreen.Add ("Request", requestScreen.GetComponent<CanvasGroup> ());
		displayScreen.Add ("TownHall", townhallScreen.GetComponent<CanvasGroup> ());
		screenList=new List<string>(displayScreen.Keys);
	}

	// Update is called once per frame
	void Update ()
	{
		if (mainScreen.alpha == 1) {
			if (myGuild != null) {
				for (int i=0;i<screenList.Count;i++){
					DisplayScreen(screenList[i],displayScreen[screenList[i]]);
				}
			}
			CheckEvent ();
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			CloseGame ();
		}
	}

	public void StartGame (bool load)
	{
		if (load) {
			Database.LoadData ();
			Database.myGuild=Database.guilds.FindGuild(0);
			Database.events.UpdateTrigger ();
		} else {
			Database.Initialize ();
			Database.myGuild=Database.guilds.FindGuild(0);
			Database.myGuild.RecruitCharacter (Database.characters.GetCharacter (0));
			Database.myGuild.RecruitCharacter (Database.characters.GetCharacter (1));
			Database.quests.GenerateQuest (0, 0, "main");
			Database.quests.GenerateQuest (1, 0, "main");
		}
		screenDisplay="Main";
		DisplayScreen ("Start",startScreen);
		DisplayScreen ("Main",mainScreen);
		if (load){
			screenDisplay = "Guild";
		} else{
			screenDisplay = "TownHall";
		}
		myGuild = Database.myGuild;
		Database.game = this;
		guildScreen.GetComponent<CanvasGroup> ().alpha = 0;
		waitingScreen.alpha=0;
		waitingScreen.interactable=false;
		waitingScreen.blocksRaycasts=false;
	}

	public void DisplayScreen (string screenName, CanvasGroup screen, bool refresh=false)
	{
		if (screenDisplay == screenName && screen.alpha == 0) {
			if (screen.GetComponentInChildren<ScrollRect> () != null) {
				screen.GetComponentInChildren<ScrollRect> ().normalizedPosition = new Vector2 (0, 1);
			} 
			EnableScreen (screen, true);
			screen.alpha = 1;
		} else if (screenDisplay != screenName) {
			EnableScreen (screen, false);
			screen.alpha = 0;
		}
		if (refresh){
			screen.alpha=0;
		}
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

	public void RecruitCharacter ()
	{
		Debug.Log ("Recruiting");
		Character character = Database.characters.GetRecruitables () [statDisplayId];
		character.name = textInputScreen.textInput.text;
		myGuild.RecruitCharacter (character);
		recruitScreen.UpdateText ();
		characterStatScreen.CloseScreen ();
		promptScreen.Close ();
		if (!Database.events.GetTrigger(105).activated){
			Database.events.UpdateTrigger ();
		}
	}

	public void PromptRecruit (int id)
	{
		if (id != 999) {
			statDisplayId = id;
		}
		InputText ("Recruit", Database.characters.GetRecruitables () [id].id);
	}

	public void InputText (string action, int id, string type="Character", bool forced=true)
	{
		textInputScreen.InputText (action, id, type, forced);
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
		if (myGuild.GetAvailableCharacters ().Count > 0) {
			selectScreen.UpdateText (myGuild.GetAvailableCharacters (), Database.strings.GetString (action + "Select"), maxSelection);
			nextAction = action;
			selectScreen.show = true;
		}
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
			OpenPrompt (nextAction);
			return;
		}
		ReturnToLastScreen ();
	}

	public void OpenPrompt (string action)
	{
		promptScreen.Prompt (action);
	}

	public void GoOnAdventure ()
	{
		List<Character> selectedcharacters = new List<Character> ();
		if (selectScreen.show) {
			selectedcharacters = GetSelectedCharacters (selectScreen.selectedCharacters);
		} else if (searchScreen.show) {
			selectedcharacters = GetSelectedCharacters (searchScreen.selectedCharacters);
		}
		Task task=new Task (nextAction, outsideScreen.GetSelectedArea (), selectedcharacters, searchScreen.searchType.name);
		myGuild.AddTask (task);
		for (int i=0; i<selectedcharacters.Count; i++) {
			selectedcharacters [i].status = searchScreen.searchType.name;
			selectedcharacters [i].statusAdd = outsideScreen.GetSelectedArea ().name;
		}
		StartCoroutine(task.AdventureTime(adventureWaitTime));
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

	public void Upgrade ()
	{
		myGuild.Upgrade (upgradeScreen.lastSelected.id);
		upgradeScreen.UpdateText ();
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
			int money = 0;
			if (shopScreen.buying) {
				status = "Shopping";
				task = "Shop";
				money = shopScreen.TotalCost ();
			} else {
				status = "Selling";
				task = "Sell";
			}
			Task sellTask=new Task (task, 1.0f, shopScreen.buyers, shopScreen.GetItemToBuyOrSell (), money);
			myGuild.AddTask (sellTask);
			if (task=="Sell"){
				StartCoroutine(sellTask.SellTime(sellWaitTime));
			}
		} else {
			SlotInfo slot = shopScreen.GetSelected ();
			Skill skill = null;
			Ability ability = null;
			if (slot.isAbility) {
				ability = Database.skills.GetAbility (slot.id);
			} else {
				skill = Database.skills.GetSkill (slot.id);
			}
			status = "Studying";
			myGuild.AddTask (new Task ("School", 1.0f, shopScreen.buyers, skill, ability, slot.SetCost (shopScreen.buyers)));
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
	public void StartNextDay(){
		StartCoroutine(WaitForTasksToBeDone());
	}

	public void NextDay ()
	{
		if (myGuild.inventory.GetSpace () >= 0) {
			int day = Database.day;
			int month = Database.month;
			int year = Database.year;
			myGuild.UpdateTasks (day);

			if (myGuild.tasklog.Count > 0) {
				nextDayScreen.UpdateText (day, month, year, myGuild.tasklog, myGuild.GetCharacterActivity ());
			} else
				nextDayScreen.UpdateText (day, month, year, null, myGuild.GetCharacterActivity ());
			Database.day += 1;
			if (Database.day > 30) {
				Database.month += 1;
				Database.day -= 30;
				if (Database.month > 12) {
					Database.year += 1;
					Database.month -= 12;
				}
			}
			myGuild.UpdateCharacters ();
			nextDayScreen.show = true;
			myGuild.NextDayReset ();
			Database.SaveData ();
		} else {
			Debug.Log ("There's not enough space in the storage to continue the day");
			Database.events.GetTrigger (100).Activate ();
		}
	}

	public void ReturnToMainScreen ()
	{
		if (nextDayScreen.show) {
			Database.events.UpdateTrigger ();
		}
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

	public void CheckEvent ()
	{
		gameEvent = Database.events.GetActiveEvent ();
		if (gameEvent != null) {
			dialogueScreen.ShowDialogue (gameEvent, null);
		}
	}
	public void ActivateEvent(){
		Database.events.ActivateObjectiveEvent();
	}

	IEnumerator WaitForTasksToBeDone ()
	{
		waitingScreen.alpha=1;
		waitingScreen.interactable=true;
		waitingScreen.blocksRaycasts=true;
		while (tasksWithCoroutine.Count>0){
			Debug.Log(tasksWithCoroutine.Count);
			yield return waitTime;
		}
		waitingScreen.alpha=0;
		waitingScreen.interactable=false;
		waitingScreen.blocksRaycasts=false;
		NextDay();
	}
}
