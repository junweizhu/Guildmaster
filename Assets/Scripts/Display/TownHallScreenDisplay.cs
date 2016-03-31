using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TownHallScreenDisplay : MonoBehaviour
{
	public Text objective;
	private bool refresh;
	private CanvasGroup canvasGroup;

	void Start ()
	{
		canvasGroup = GetComponent<CanvasGroup> ();
	}

	void Update ()
	{
		if (canvasGroup.alpha == 1) {
			string text = Database.strings.GetObjectiveString (Database.events.CurrentObjectiveId ());

			if (text == "") {
				objective.text = text;
			} else {
				objective.text = "Objective: " + text;
			}
		}
	}
}
