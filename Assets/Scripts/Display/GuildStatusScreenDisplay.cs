using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuildStatusScreenDisplay : MonoBehaviour
{
	public Text questsDone;
	public Text itemsGathered;
	public Text itemsSold;
	public Text monstersSlain;
	public Text tasksGiven;
	public Text membersInjured;
	public Text shopsVisited;
	public Text schoolVisited;


	// Update is called once per frame
	void Update ()
	{
		if (GetComponent<CanvasGroup> ().alpha == 1) {
			questsDone.text = Database.myGuild.questFinished.ToString ();
			itemsGathered.text = Database.myGuild.itemsGathered.ToString ();
			itemsSold.text = Database.myGuild.itemsSold.ToString ();
			monstersSlain.text = Database.myGuild.monstersSlain.ToString ();
			tasksGiven.text = Database.myGuild.tasksGiven.ToString ();
			membersInjured.text = Database.myGuild.membersInjured.ToString ();
			shopsVisited.text = Database.myGuild.visitedShop.ToString ();
			schoolVisited.text = Database.myGuild.visitedSchool.ToString ();
		}
	}
}
