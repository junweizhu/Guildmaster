using UnityEngine;
using System.Collections;

public class ImageResize : MonoBehaviour {
	private RectTransform icon;
	// Use this for initialization
	void Start () {
		icon=GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		if (icon!=null){
			if (Mathf.Abs(icon.rect.height-48)<Mathf.Abs(icon.rect.height-64)){
				icon.sizeDelta=new Vector2(48,48-icon.rect.height);
			} else if(Mathf.Abs(icon.rect.height-48)>Mathf.Abs(icon.rect.height-64)){
				icon.sizeDelta=new Vector2(64,64-icon.rect.height);
			}

		}
	}
}
