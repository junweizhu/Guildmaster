using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {
	public List<CanvasGroup> screens=new List<CanvasGroup>();
	public List<CanvasGroup> screens2=new List<CanvasGroup>();
	public List<CanvasGroup> screens3=new List<CanvasGroup>();
	public CanvasGroup background;
	public CanvasGroup background2;
	public CanvasGroup background3;

	// Update is called once per frame
	void Update () {
		CheckScreen(screens,background);
		CheckScreen(screens2,background2);
		CheckScreen(screens3,background3);

	}
	void CheckScreen(List<CanvasGroup> screens,CanvasGroup background){
		for (int i=0;i<screens.Count;i++)
		{
			if (screens[i].alpha==1){
				background.alpha=1;
				background.blocksRaycasts=true;
				break;
			}
			else{
				background.alpha=0;
				background.blocksRaycasts=false;
			}
		}
	}
}
