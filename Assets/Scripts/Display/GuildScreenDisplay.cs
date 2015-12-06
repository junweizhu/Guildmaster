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

	public void UpdateText(string name,int level, int exp, int fame, int size, int money)
	{
		guildName.text=name;
		guildLevel.text=level.ToString();
		guildExp.text=exp.ToString();
		guildFame.text=fame.ToString();
		guildSize.text=size.ToString();
		guildMoney.text=money.ToString();
	}
}
