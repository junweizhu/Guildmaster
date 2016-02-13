using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue
{

	public int id;
	public string name;
	public int order;
	public string text;
	public List<KeyValuePair<int,string>> stringId = new List<KeyValuePair<int,string>> ();

	public Dialogue ()
	{
	}

	public Dialogue (int id, string name, int order, string text, List<int>stringIds=null, List<string> stringTypes=null)
	{
		this.id = id;
		this.name = name;
		this.order = order;
		this.text = text;
		if (stringIds != null) {
			for (int i=0; i<stringIds.Count; i++) {
				stringId.Add (new KeyValuePair<int,string> (stringIds [i], stringTypes [i]));
			}
		}
	}

	public string GetSpeakerName (List<string> characterlist)
	{
		return GetName (0, characterlist);
	}

	public string GetText (List<string> characterlist)
	{
		if (stringId.Count > 1) {
			if (stringId.Count == 2) {
				return string.Format (text, GetName (1, characterlist));
			} else if (stringId.Count == 3) {
				return string.Format (text, GetName (1, characterlist), GetName (2, characterlist));
			} else if (stringId.Count == 4) {
				return string.Format (text, GetName (1, characterlist), GetName (2, characterlist), GetName (3, characterlist));
			}
		}
		return text;
	}

	public string GetName (int id, List<string> characterlist)
	{
		if (stringId.Count > 0) {
			if (stringId [id].Value == "Character") {
				return Database.characters.GetCharacter (stringId [id].Key).name;
			} else if (stringId [id].Value == "Conversation") {
				return characterlist [stringId [id].Key];
			} else if (stringId [id].Value == "Item") {
				return Database.items.FindItem (stringId [id].Key).name;
			} else if (stringId [id].Value == "Area") {
				return Database.areas.FindArea (stringId [id].Key).name;
			}
		}
		return "";
	}
}
