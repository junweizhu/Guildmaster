using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TavernScreenDisplay : MonoBehaviour {
	public Text joinText;
	public Text questText;
	private bool refresh;
	private CanvasGroup canvasGroup;
	void Start(){
		canvasGroup=GetComponent<CanvasGroup>();
	}
	void Update(){
		if(canvasGroup.alpha!=1){
			refresh=true;
		} else if(refresh){
			refresh=false;
			UpdateText();
		}
	}

	public void UpdateText()
	{
		if (Database.characters.GetRecruitables ().Count>0)
		{
			joinText.text="Some people wants to join your guild";
		} else {
			joinText.text="Currently nobody is interested in joining your guild";
		}
		if (Database.quests.AvailableQuests ().Count>0)
		{
			questText.text="Some people requires help from your guild";
		} else {
			questText.text="Currently nobody has any requests for your guild";
		}
	}
}
