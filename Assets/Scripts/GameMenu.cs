using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {
	public List<CanvasGroup> screens=new List<CanvasGroup>();
	public CanvasGroup background;

	// Update is called once per frame
	void Update () {
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
