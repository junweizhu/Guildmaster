using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharDatabase
{
	[SerializeField]
	private List<Character> memberList = new List<Character> ();
	private List<string> maleNames = new List<string> (){ "Test3", "Test4", "Test5", "Test6", "Test7", "Test8" };
	private List<string> femaleNames = new List<string> (){ "test3", "test4", "test5", "test6", "test7", "test8" };
	int magicSkillId = 1;

	// Use this for initialization
	public CharDatabase ()
	{
	}

	public void Generate ()
	{
		CustomCharacter (0,"Farren","", true,1,Stats(80,0,40,0,50,60),new Dictionary<int, int>(){{0,1},{1,1},{2,2},{3,3},{4,1}},null);
		CustomCharacter (1,"Ellisabeth","Elly", false, 1,Stats(70,60,40,55,50,55),new Dictionary<int, int>(){{0,2},{1,3},{2,2},{3,1},{4,2}},new List<int>(){11});
		//GenerateNewCharacter (1, 2, true);
		//GenerateNewCharacter (1, 3, true,false,true);
	}

	public List<Character> GetCharacter ()
	{
		return memberList;
	}

	public void LoadCharacter (List<Character> members)
	{
		memberList = members;
		for (int i = 0; i < memberList.Count; i++) {
			memberList [i].UpdateStats ();
		}
	}

	public Character GetCharacter (int id)
	{
		foreach (Character member in memberList) {
			if (member.id == id) {
				return member;
			}
		}
		return null;
	}

	public List<string> GetCharacterNames (List<int> id)
	{
		List<string> name = new List<string> ();
		for (int i = 0; i < id.Count; i++) {
			name.Add (GetCharacter (id [i]).name);
		}
		return name;
	}

	public void CustomCharacter (int id,string name, string nickname,bool male, int level, Dictionary<string,int> growthrate, Dictionary<int,int> initialSkillLevel, List<int> abilities)
	{
		Character character = new Character (id, name, male, level, "", growthrate);
		foreach (KeyValuePair<int,int> skill in initialSkillLevel) {
			if (skill.Value > 1) {
				character.GiveExp (skill.Key, (skill.Value - 1) * 100);
			}
		}
		if (abilities!=null) {
			for (int i = 0; i < abilities.Count; i++) {
				character.abilities.Add (abilities [i]);
			}
		}
		character.nickname = nickname;
		memberList.Add (character);
	}

	public Character GenerateNewCharacter (int level, int mainSkill, bool fixedGender = false, bool genderIsMale = true, bool learnMagic = false)
	{
		string charName;
		bool male = (Random.Range (0, 2) == 0);
		if (fixedGender) {
			male = genderIsMale;
		} 
		if (male) {
			charName = maleNames [Random.Range (0, maleNames.Count)];
			maleNames.Remove (charName);
		} else {
			charName = femaleNames [Random.Range (0, femaleNames.Count)];
			femaleNames.Remove (charName);
		}
		memberList.Add (new Character (memberList.Count, charName, male, level));
		foreach (Skill skill in Database.skills.SkillList()) {
			float modifier = 1.0f;
			int minimum = 0;
			if (skill.id == mainSkill) {
				modifier = 1.25f;
				minimum = 50;
			}

			int exp = Mathf.RoundToInt (Random.Range (50 * modifier, ((level) * modifier) * 100)) + minimum;
			memberList [memberList.Count - 1].GiveExp (skill.id, exp);
		}
		if (mainSkill == magicSkillId || Random.Range (0, 100) <= 15 || learnMagic) {
			int magic = Random.Range (4, 7);
			memberList [memberList.Count - 1].abilities.Add (magic);
		}
		memberList [memberList.Count - 1].NextDayResets ();
		memberList [memberList.Count - 1].recruitable = true;
		return memberList [memberList.Count - 1];
	}

	public List<Character> GetRecruitables ()
	{
		List<Character> unrecruitedList = new List<Character> ();
		for (int i = 0; i < memberList.Count; i++) {
			if (memberList [i].recruited == false && memberList [i].recruitable)
				unrecruitedList.Add (memberList [i]);
		}
		return unrecruitedList;
	}

	public int RecruitableId (Character member)
	{
		List<Character> unrecruitedList = GetRecruitables ();
		for (int i = 0; i < unrecruitedList.Count; i++) {
			if (unrecruitedList [i].id == member.id) {
				return i;
			}
		}
		return 999;
	}

	public Dictionary<string,int> Stats (int health, int mana, int strength, int intelligence, int dexterity, int agility)
	{
		Dictionary<string,int> stat = new Dictionary<string,int> ();
		stat ["Health"] = health;
		stat ["Mana"] = mana;
		stat ["Strength"] = strength;
		stat ["Dexterity"] = dexterity;
		stat ["Agility"] = agility;
		stat ["Intelligence"] = intelligence;
		return stat;
	}
}
