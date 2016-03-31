using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable()]
public class SaveData : ISerializable
{

	public List<Quest> quests;
	public List<Character> characters;
	public List<Guild> guilds;
	public int day;
	public int month;
	public int year;
	public List<int> activatedtriggers;
	public List<int> finishedevents;

	public SaveData ()
	{
	}

	public SaveData (SerializationInfo info, StreamingContext ctxt)
	{
		quests = (List<Quest>)info.GetValue ("quests", typeof(List<Quest>));
		characters = (List<Character>)info.GetValue ("characters", typeof(List<Character>));
		guilds = (List<Guild>)info.GetValue ("guilds", typeof(List<Guild>));
		day = (int)info.GetValue ("day", typeof(int));
		month = (int)info.GetValue ("month", typeof(int));
		year = (int)info.GetValue ("year", typeof(int));
		activatedtriggers=(List<int>)info.GetValue ("activatedtriggers", typeof(List<int>));
		finishedevents=(List<int>)info.GetValue("finishedevents",typeof(List<int>));

	}

	public void GetObjectData (SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue ("quests", quests);
		info.AddValue ("characters", characters);
		info.AddValue ("guilds", guilds);
		info.AddValue ("day", day);
		info.AddValue ("month", month);
		info.AddValue ("year", year);
		info.AddValue ("activatedtriggers",activatedtriggers);
		info.AddValue ("finishedevents",finishedevents);
	}


	/*public SaveData(SerializationInfo info, StreamingContext ctxt) { 
		FieldInfo[] fields = this.GetType().GetFields(); 
		foreach (FieldInfo f in fields) { 
			FieldInfo fi = this.GetType().GetField(f.Name);
			if (fi == null)
				Debug.Log("Field is null! Property name: " + f.Name + " --- Type: " + f.FieldType);
			fi.SetValue(this, info.GetValue(f.Name, f.FieldType));
		}
	}
	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		FieldInfo[] fields = this.GetType().GetFields();
		foreach (FieldInfo f in fields)
		{
			info.AddValue(f.Name, f.GetValue(this));
		}
	}*/
}

public class SaveLoad
{
	public static string currentFilePath = "Save.data";
	public static SaveData data=new SaveData();
	public static void Save ()
	{
		Save (currentFilePath);
	}
	public static bool CheckIfSaveFileExists(){
		if(File.Exists(Application.persistentDataPath+"\\"+currentFilePath)){
			return true;
		}
		return false;
	}
	public static void Save (string filePath)
	{
		FileStream stream =File.Create (Application.persistentDataPath+"\\"+filePath);
		BinaryFormatter bformatter = new BinaryFormatter ();
		bformatter.Binder = new VersionDeserializationBinder ();
		bformatter.Serialize(stream, data);
		stream.Close ();

	}
	public static void Load ()  { 
		Load(currentFilePath);  
	}

	public static void Load (string filePath)
	{
		BinaryFormatter bformatter = new BinaryFormatter ();
		bformatter.Binder = new VersionDeserializationBinder ();
		if(File.Exists(Application.persistentDataPath+"\\"+filePath)){
			FileStream stream = File.Open(Application.persistentDataPath+"\\"+filePath, FileMode.Open);
			data = (SaveData)bformatter.Deserialize (stream);
			stream.Close ();
		}
	}
}

public sealed class VersionDeserializationBinder: SerializationBinder
{
	public override Type BindToType (string assemblyName, string typeName)
	{
		if (!string.IsNullOrEmpty (assemblyName) && !string.IsNullOrEmpty (typeName)) {
			Type typeToDeserialize = null;
			assemblyName = Assembly.GetExecutingAssembly ().FullName;
			typeToDeserialize = Type.GetType (String.Format ("{0},{1}", typeName, assemblyName));
			return typeToDeserialize;
		}
		return null;
	}
}