using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Json_Deckclass
{
	public List<string> deck;
	public Json_Deckclass() { deck = new List<string>(); }
}

// Json 저장용 클래스들
[Serializable]
public class JsonPair<TKey, TValue>
{
	public TKey Key;
	public TValue Value;
}

[Serializable]
public class JsonDictionary<TKey, TValue>
{
	[JsonProperty("Dictionary")]
	public Dictionary<TKey, TValue> Dictionary;

	public JsonDictionary()
	{
		Dictionary = new Dictionary<TKey, TValue>();
	}

	public string ToJson()
	{
		return JsonConvert.SerializeObject(this, Formatting.Indented);
	}

	public static JsonDictionary<TKey, TValue> FromJson(string json)
	{
		return JsonConvert.DeserializeObject<JsonDictionary<TKey, TValue>>(json);
	}
}

[Serializable]
public class JsonList<T>
{
	[JsonProperty("List")]
	public List<T> List;

	public JsonList()
	{
		List = new List<T>();
	}

	public string ToJson()
	{
		return JsonConvert.SerializeObject(this, Formatting.Indented);
	}

	public static JsonList<T> FromJson(string json)
	{
		return JsonConvert.DeserializeObject<JsonList<T>>(json);
	}
}

public static class JsonFileHandler
{
	public static void SaveToJsonFile(string filePath, string jsonString)
	{
		File.WriteAllText(filePath, jsonString);
	}

	public static string LoadFromJsonFile(string filePath)
	{
		if (File.Exists(filePath))
		{
			return File.ReadAllText(filePath);
		}
		return null;
	}
}