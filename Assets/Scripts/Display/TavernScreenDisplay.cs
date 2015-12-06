using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TavernScreenDisplay : MonoBehaviour {
	public Text joinText;
	public Text questText;
	

	public void UpdateText(List<Member> recruits)
	{
		joinText.text=recruits.Count.ToString();
		if(recruits.Count==1)
		{
			joinText.text+=" person is ";
		}
		else
		{
			joinText.text+=" people are ";
		}
		joinText.text+="interested in joining your guild";
	}
}
