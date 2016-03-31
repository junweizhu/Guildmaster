using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuildScreenDisplay : MonoBehaviour
{
	public Text guildName;
	public Text guildLevel;
	public Text guildExp;
	public Text guildFame;
	public Text guildSize;
	public Text guildMoney;
	public Text guildDay;
	public Text guildMaintenance;
	public Text guildDailyCost;
	private bool refresh;
	private CanvasGroup canvasGroup;

	void Start ()
	{
		canvasGroup = GetComponent<CanvasGroup> ();
	}

	void Update ()
	{
		if (canvasGroup.alpha != 1) {
			refresh = true;
		} else if (refresh) {
			refresh = false;
			UpdateText ();
		}
	}

	public void UpdateText ()
	{
		Guild guild = Database.myGuild;
		guildName.text = guild.name;
		guildLevel.text = guild.level.ToString ();
		guildExp.text = guild.exp.ToString () + "/" + guild.requiredExp.ToString ();
		guildFame.text = guild.fame.ToString ();
		guildSize.text = guild.size.ToString () + "/" + Database.upgrades.GetUpgrade (0).MaxSize (guild.upgradelist [0]).ToString ();
		guildMoney.text = string.Format (Database.strings.GetString("Currency"),guild.money.ToString ());
		guildDay.text = string.Format (Database.strings.GetString ("Date"), Database.day.ToString (), Database.strings.monthNames [Database.month], Database.year.ToString ());
		guildMaintenance.text= string.Format (Database.strings.GetString("Currency"),guild.maintenanceCost.ToString());
		if (guild.DailyMaintenance()>0){
			guildDailyCost.text=string.Format (Database.strings.GetString("Maintenance"),string.Format (Database.strings.GetString("Currency"),guild.DailyMaintenance()));
		} else{
			guildDailyCost.text="";
		}
	}
}
