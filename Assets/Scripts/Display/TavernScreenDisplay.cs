using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TavernScreenDisplay : MonoBehaviour {
	public Text joinText;
	public Text questText;
	

	public void UpdateText(List<Member> recruits)
	{
		if (recruits.Count>0)
		{
			joinText.text="Some people seem interested in your guild";
		}
	}
}
