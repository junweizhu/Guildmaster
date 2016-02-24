using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuildScreenDisplay : MonoBehaviour {
	public Text guildName;
	public Text guildLevel;
	public Text guildExp;
	public Text guildFame;
	public Text guildSize;
	public Text guildMoney;
	public Text guildDay;

	public void UpdateText(int day, int month, int year)
	{
		Guild guild=Database.myGuild;
		guildName.text=guild.name;
		guildLevel.text=guild.level.ToString();
		guildExp.text=guild.exp.ToString();
		guildFame.text=guild.fame.ToString();
		guildSize.text=guild.size.ToString()+"/"+Database.upgrades.GetUpgrade(0).MaxSize(guild.upgradelist[0]).ToString();
		guildMoney.text=guild.money.ToString();
		guildDay.text=string.Format(Database.strings.GetString("Date"),day.ToString(),Database.strings.monthNames[month],year.ToString());
	}
}
