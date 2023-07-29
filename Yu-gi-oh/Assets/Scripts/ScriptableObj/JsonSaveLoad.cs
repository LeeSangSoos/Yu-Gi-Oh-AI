using System.IO;
using UnityEngine;
using System.Collections.Generic;

public static class JsonSaveLoad
{
	public static void SaveMyDeckData(CardList Deck, CardList ExtraDeck)
	{
		Json_Deckclass deckdata = new Json_Deckclass();
		foreach (Card card in Deck)
		{
			deckdata.deck.Add(card.CardName);
		}

		string json = JsonUtility.ToJson(deckdata);
		string filePath = Path.Combine(Application.persistentDataPath, "MyDeckData.json");
		File.WriteAllText(filePath, json);

		Json_Deckclass extradata = new Json_Deckclass();
		foreach (Card card in ExtraDeck)
		{
			extradata.deck.Add(card.CardName);
		}
		json = JsonUtility.ToJson(extradata);
		filePath = Path.Combine(Application.persistentDataPath, "MyExtraDeckData.json");
		File.WriteAllText(filePath, json);
	}
	public static CardList LoadMyDeckData()
	{
		//json에서 덱 카드들의 이름 읽어오기
		string filePath = Path.Combine(Application.persistentDataPath, "MyDeckData.json");
		string json = File.ReadAllText(filePath);
		Json_Deckclass deckdata = JsonUtility.FromJson<Json_Deckclass>(json);

		//GameManager의 카드 리스트에서 카드명에 따른 카드들 로드
		CardList Deck = new CardList();
		foreach (string cardname in deckdata.deck)
		{
			Card card = GameManagerScript.CardList.Find(match => match.CardName == cardname);
			Deck.Add(card);
		}
		return Deck;
	}
	public static CardList LoadMyExtraDeckData()
	{
		string filePath = Path.Combine(Application.persistentDataPath, "MyExtraDeckData.json");
		string json = File.ReadAllText(filePath);
		Json_Deckclass ExtraDeckData = JsonUtility.FromJson<Json_Deckclass>(json);

		CardList ExtraDeck = new CardList();
		foreach (string cardname in ExtraDeckData.deck)
		{
			Card card = GameManagerScript.CardList.Find(match => match.CardName == cardname);
			ExtraDeck.Add(card);
		}

		return ExtraDeck;
	}
	public static void SaveAiDeckData(CardList Deck, CardList ExtraDeck)
	{
		Json_Deckclass deckdata = new Json_Deckclass();
		foreach (Card card in Deck)
		{
			deckdata.deck.Add(card.CardName);
		}
		string json = JsonUtility.ToJson(deckdata);
		string filePath = Path.Combine(Application.persistentDataPath, "AiDeckData.json");
		File.WriteAllText(filePath, json);

		Json_Deckclass extradata = new Json_Deckclass();
		foreach (Card card in ExtraDeck)
		{
			extradata.deck.Add(card.CardName);
		}
		json = JsonUtility.ToJson(extradata);
		filePath = Path.Combine(Application.persistentDataPath, "AiExtraDeckData.json");
		File.WriteAllText(filePath, json);
	}
	public static CardList LoadAiDeckData()
	{
		string filePath = Path.Combine(Application.persistentDataPath, "AiDeckData.json");
		string json = File.ReadAllText(filePath);
		Json_Deckclass deckdata = JsonUtility.FromJson<Json_Deckclass>(json);

		CardList Deck = new CardList();
		foreach (string cardname in deckdata.deck)
		{
			Card card = GameManagerScript.CardList.Find(match => match.CardName == cardname);
			Deck.Add(card);
		}
		return Deck;
	}
	public static CardList LoadAiExtraDeckData()
	{
		string filePath = Path.Combine(Application.persistentDataPath, "AiExtraDeckData.json");
		string json = File.ReadAllText(filePath);
		Json_Deckclass ExtraDeckData = JsonUtility.FromJson<Json_Deckclass>(json);

		CardList ExtraDeck = new CardList();
		foreach (string cardname in ExtraDeckData.deck)
		{
			Card card = GameManagerScript.CardList.Find(match => match.CardName == cardname);
			ExtraDeck.Add(card);
		}

		return ExtraDeck;
	}
}