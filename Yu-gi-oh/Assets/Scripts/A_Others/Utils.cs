using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public static JsonList<JsonDictionary<string, object>> CardList2JsonList(List<Card> cardList)
	{
		JsonList<JsonDictionary<string, object>> jsonList = new JsonList<JsonDictionary<string, object>>();
		foreach (Card card in cardList)
		{
			if (card is MonsterCard monsterCard)
			{
				JsonDictionary<string, object> cardInfo = new JsonDictionary<string, object>();

				cardInfo.Dictionary.Add("cardId", monsterCard.CardId);

				string name = monsterCard.CardName;
				if (name == "Fiend'sHand") { name = "FiendsHand"; }
				cardInfo.Dictionary.Add("cardName", name);

				cardInfo.Dictionary.Add("cardAtk", monsterCard.atk);
				cardInfo.Dictionary.Add("cardDef", monsterCard.def);
				cardInfo.Dictionary.Add("cardLevel", monsterCard.level);

				jsonList.List.Add(cardInfo);
			}
		}
		return jsonList;
	}
}
