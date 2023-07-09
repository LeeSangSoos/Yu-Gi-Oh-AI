using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayManagerScript : MonoBehaviour
{
	CardList mydeck;
	CardList myextra;
	CardList aideck;
	CardList aiextra;
	Page gamepage;
	Turn turn;

	CardList myhand;
	CardList aihand;
	private void Start()
	{
		//Set Decks
		mydeck = JsonSaveLoad.LoadMyDeckData();
		myextra = JsonSaveLoad.LoadMyExtraDeckData();
		aideck = JsonSaveLoad.LoadAiDeckData();
		aiextra = JsonSaveLoad.LoadAiExtraDeckData();
		mydeck = Shuffle(mydeck);
		aideck = Shuffle(aideck);

		//Random Turn
		turn = Random.Range(0, 1) == 0 ? Turn.Ai : Turn.My;

		//Draw 5 cards from deck for each player
		for(int i = 0; i < 5; i++)
		{
			myhand.Add(mydeck[i]);
			aihand.Add(aideck[i]);
			mydeck.RemoveAt(i);
			aideck.RemoveAt(i);
		}

		gamepage = Page.Draw;
	}

	private void Update()
	{
		switch (gamepage)
		{
			case Page.Draw: 
				
				break;
			case Page.Standby: break;
			case Page.Main1: break;
			case Page.Battle: break;
			case Page.Main2: break;
			case Page.End: break;
		}
	}

	public CardList Shuffle(CardList list)
	{
		for (int i = list.Count - 1; i > 0; i--)
		{
			int randomIndex = Random.Range(0, i + 1);
			Card temp = list.cards[i];
			list.cards[i] = list.cards[randomIndex];
			list.cards[randomIndex] = temp;
		}
		return list;
	}

	public void NextPage()
	{
		switch(gamepage)
		{
			case Page.Draw: 
				gamepage=Page.Standby;
				break;
			case Page.Standby: 
				gamepage = Page.Main1;
				break;
			case Page.Main1: 
				gamepage=Page.Battle;
				break;
			case Page.Battle:
				gamepage = Page.Main2;
				break;
			case Page.Main2: 
				gamepage=Page.End;
				break;
			case Page.End:
				turn = turn == Turn.My ? Turn.Ai : Turn.My;
				gamepage = Page.Draw;
				break;
		}
	}
}
