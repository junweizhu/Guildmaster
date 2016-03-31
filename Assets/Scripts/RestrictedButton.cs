using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RestrictedButton : MonoBehaviour
{
	public int triggerId;
	public CanvasGroup canvas;
	// Update is called once per frame
	void Update ()
	{

		if (canvas.alpha==1&&Database.events.GetTrigger (triggerId) != null) {
			if (Database.events.GetTrigger (triggerId).activated) {
				GetComponent<Button> ().interactable = true;
				this.enabled = false;
			} else {
				GetComponent<Button> ().interactable = false;

			}
		}
	}
}
