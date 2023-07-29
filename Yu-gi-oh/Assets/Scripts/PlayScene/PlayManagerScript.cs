using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayManagerScript : MonoBehaviour
{
	Page gamepage;
	Turn turn;
	int TotalTurn = 1;
	public Player Ai;
	public Player User;
	Dictionary<Turn, Player> playermap = new Dictionary<Turn, Player>();
	Player player;
	bool TurnStart = true;
	public Text turntext;

	private void Start()
	{
		//Set Decks
		User.MainDeck = (JsonSaveLoad.LoadMyDeckData());
		User.ExtraDeck = (JsonSaveLoad.LoadMyExtraDeckData());
		Ai.MainDeck = (JsonSaveLoad.LoadAiDeckData());
		Ai.ExtraDeck = (JsonSaveLoad.LoadAiExtraDeckData());
		User.MainDeck = (Shuffle(User.MainDeck));
		Ai.MainDeck = (Shuffle(Ai.MainDeck));

		//Set Player AI and User
		playermap[Turn.My] = User;
		playermap[Turn.Ai] = Ai;

		//Random Turn
		turn = Random.Range(0, 2) == 0 ? Turn.Ai : Turn.My;
		player = playermap[turn];
		TurnColor.color = turn == Turn.My ? Constant.mycolor: Constant.aicolor;
		turntext.text = "Turn: " + TotalTurn;

		gamepage = Page.Draw;
	}

	private void Update()
	{
		if (TurnStart)
		{
			switch (gamepage)
			{
				case Page.Draw:
					DrawPageAction();
					break;
				case Page.Standby:

					break;
				case Page.Main1:

					break;
				case Page.Battle: break;
				case Page.Main2: break;
				case Page.End: break;
			}
			TurnStart = false;
		}
		//Check_effect(player);
		//Check_effect(player.enemy);
		//카드가 직접 이벤트 요청
		//요청 타이밍이 맞으면 이벤트 UI ON
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

	public Text PageText;
	public Image TurnColor;
	public void NextPage()
	{
		switch (gamepage)
		{
			case Page.Draw:
				gamepage = Page.Standby;
				break;
			case Page.Standby:
				gamepage = Page.Main1;
				break;
			case Page.Main1:
				if (TotalTurn == 1) gamepage = Page.End;
				else gamepage = Page.Battle;
				break;
			case Page.Battle:
				gamepage = Page.Main2;
				break;
			case Page.Main2:
				gamepage = Page.End;
				break;
			case Page.End:
				turn = turn == Turn.My ? Turn.Ai : Turn.My;
				player = playermap[turn];
				TurnColor.color = turn == Turn.My ? Constant.mycolor : Constant.aicolor;
				TotalTurn++;
				gamepage = Page.Draw;
				break;
		}
		turntext.text = "Turn: " + TotalTurn;
		PageText.text = gamepage.ToString();
		TurnStart = true;
	}
	public void NextpageBtnAction()
	{
		if(turn== Turn.My)
		{
			NextPage();
		}
	}
	private void DrawPageAction()
	{

		if (TotalTurn > 1)
		{
			player.draw();
		}
		else
		{
			//Draw 5 cards from deck for each player
			for (int i = 0; i < 5; i++)
			{
				User.draw();
				Ai.draw();
			}
		}
	}

	private void StandbyAction()
	{

	}

	private void Main1Action()
	{
		/*player.normalsummon = true;
		 *player.set = true;
		 *player.normaleffect = true;
		 *player.mainspecialsummon = true;
		 */
	}

	private void BattleAction()
	{
		/* player.attack = true;
		 * 
		 */
	}

	private void Main2Action()
	{
		/*player.normalsummon = true;
		 *player.set = true;
		 *player.normaleffect = true;
		 *player.mainspecialsummon = true;
		 */
	}

	private void EndAction()
	{
		/*
		 */
	}

	#region Info for Ai
	public Turn GetTurn { 
		get { return turn; } set { }
	}

	#endregion

	#region Function for Ai
	public void Ai_NextPage()
	{
		if(turn== Turn.Ai)
		{
			NextPage();
		}
	}
	#endregion
}
