using System.IO;
using UnityEngine;

public static class JsonSaveLoad
{
	public static void SaveMyDeckData(CardList Deck, CardList ExtraDeck)
	{
		string json = JsonUtility.ToJson(Deck);
		string filePath = Path.Combine(Application.persistentDataPath, "MyDeckData.json");
		File.WriteAllText(filePath, json);
		json = JsonUtility.ToJson(ExtraDeck);
		filePath = Path.Combine(Application.persistentDataPath, "MyExtraDeckData.json");
		File.WriteAllText(filePath, json);
	}
	public static CardList LoadMyDeckData()
	{
		string filePath = Path.Combine(Application.persistentDataPath, "MyDeckData.json");
		string json = File.ReadAllText(filePath);
		CardList Deck = JsonUtility.FromJson<CardList>(json);
		return Deck;
	}

	public static CardList LoadMyExtraDeckData()
	{
		string filePath = Path.Combine(Application.persistentDataPath, "MyExtraDeckData.json");
		string json = File.ReadAllText(filePath);
		CardList ExtraDeck = JsonUtility.FromJson<CardList>(json);
		return ExtraDeck;
	}

	public static void SaveAiDeckData(CardList Deck, CardList ExtraDeck)
	{
		string json = JsonUtility.ToJson(Deck);
		string filePath = Path.Combine(Application.persistentDataPath, "AiDeckData.json");
		File.WriteAllText(filePath, json);
		json = JsonUtility.ToJson(ExtraDeck);
		filePath = Path.Combine(Application.persistentDataPath, "AiExtraDeckData.json");
		File.WriteAllText(filePath, json);
	}
	public static CardList LoadAiDeckData()
	{
		string filePath = Path.Combine(Application.persistentDataPath, "AiDeckData.json");
		string json = File.ReadAllText(filePath);
		CardList Deck = JsonUtility.FromJson<CardList>(json);
		return Deck;
	}

	public static CardList LoadAiExtraDeckData()
	{
		string filePath = Path.Combine(Application.persistentDataPath, "AiExtraDeckData.json");
		string json = File.ReadAllText(filePath);
		CardList ExtraDeck = JsonUtility.FromJson<CardList>(json);
		return ExtraDeck;
	}
}
