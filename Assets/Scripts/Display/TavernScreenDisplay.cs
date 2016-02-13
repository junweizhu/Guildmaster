using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TavernScreenDisplay : MonoBehaviour {
	public Text joinText;
	public Text questText;
	

	public void UpdateText(int recruits,int requests)
	{
		if (recruits>0)
		{
			joinText.text="Some people wants to join your guild";
		} else {
			joinText.text="Currently nobody is interested in joining your guild";
		}
	}
}
