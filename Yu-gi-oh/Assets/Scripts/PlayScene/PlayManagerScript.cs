using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayManagerScript : MonoBehaviour
{
	Page gamepage;
	Turn turn;
	int TotalTurn = 1;
	public Player ai;
	public Player user;
	Dictionary<Turn, Player> playermap = new Dictionary<Turn, Player>();
	Player player;
	bool PageStart = true;
	public Text turntext;
	PageTime pageTime = PageTime.Start;
	bool managerworkleft;
	public GameManagerScript gamemanager;
	public GameObject CardPrefab;
	int n = 0;
	#region TimeFunctions
	private void Awake()
	{
		gamemanager = Object.FindFirstObjectByType<GameManagerScript>();
		if (gamemanager.IsUser)
		{
			user.GetComponent<AiPlayer>().enabled = false;
			user.GetComponent<User>().enabled = true;
			user.setPlayerType(PlayerType.User);
		}
		else
		{
			user.GetComponent<AiPlayer>().enabled = true;
			user.GetComponent<User>().enabled = false;
			user.setPlayerType(PlayerType.Ai);
		}
	}
	private void Start()
	{
	# region Set Decks
		user.MainDeck = (JsonSaveLoad.LoadMyDeckData());
		user.ExtraDeck = (JsonSaveLoad.LoadMyExtraDeckData());
		ai.MainDeck = (JsonSaveLoad.LoadAiDeckData());
		ai.ExtraDeck = (JsonSaveLoad.LoadAiExtraDeckData());

		InitializeDeck(user, user.MainDeck);
		InitializeDeck(ai, ai.MainDeck);
		Debug.Log("Ai Deck : " + string.Join(", ", ai.MainDeck.Select(c => c.GetCardNum())));
		Debug.Log("User Deck : " + string.Join(", ", user.MainDeck.Select(c => c.GetCardNum())));

		user.MainDeck = (Shuffle(user.MainDeck));
		ai.MainDeck = (Shuffle(ai.MainDeck));
		#endregion

		#region Set Player AI and user
		playermap[Turn.My] = user;
		playermap[Turn.Ai] = ai;
		user.setPlayerType(gamemanager.IsUser ? PlayerType.User : PlayerType.Ai);
		ai.setPlayerType(PlayerType.Ai);
		#endregion
		#region Random Turn
		turn = Random.Range(0, 2) == 0 ? Turn.Ai : Turn.My;
		player = playermap[turn];
		player.myturn = true;
		TurnColor.color = turn == Turn.My ? Constant.mycolor: Constant.aicolor;
		turntext.text = "Turn: " + TotalTurn;
		#endregion
		#region Game start
		player.SetTurnActions();
		gamepage = Page.Draw;

		for(int i=0; i < 4; i++)
		{
			user.draw();
			ai.draw();
		}
		player.enemy.draw();
		#endregion
	}
	private void Update()
	{
		if (PageStart)
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
			PageStart = false;
			if (!IsWorkLeft())
			{
				NextpageTime();
			}
		}
		//Check_effect(player);
		//Check_effect(player.enemy);
		//Card calls for event
		//event ui on when event happens
	}
	#endregion
	#region Fuctions needed
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
	public int GetTotalTurn()
	{
		return TotalTurn;
	}
	private bool IsWorkLeft()
	{
		return user.WorkLeft || ai.WorkLeft || managerworkleft;
	}
	private void InitializeDeck(Player player, CardList deck)
	{
		foreach (Card card in deck)
		{
			card.owner = player;
			card.SetOriginOwner(player);
			card.pos = CardPosition.MainDeck;
			card.SetCardNum(n++);

			GameObject cardobject = Instantiate(CardPrefab);
			cardobject.SetActive(false);

			CardEvent cardEventComponent = cardobject.GetComponent<CardEvent>();
			cardEventComponent.card = card;
			cardEventComponent.player = player;
			cardEventComponent.playertype = player.GetPlayerType();

			cardobject.name = card.CardName;
			card.SetCardObject(cardobject);
		}
	}
	#endregion
	#region PageActions
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
				player.myturn = false;
				player = playermap[turn];
				player.myturn = true;
				player.SetTurnActions();
				TurnColor.color = turn == Turn.My ? Constant.mycolor : Constant.aicolor;
				TotalTurn++;
				gamepage = Page.Draw;
				turntext.text = "Turn: " + TotalTurn;
				break;
		}
		PageText.text = gamepage.ToString();
		PageStart = true;
	}
	public void NextpageBtnAction()
	{
		if(turn == Turn.My)
		{
			NextPage();
		}
	}
	private void NextpageTime()
	{
		Debug.Log(pageTime.ToString());
		switch (pageTime)
		{
			case PageTime.Start: pageTime = PageTime.OnGoing; 
				break;
			case PageTime.OnGoing: pageTime = PageTime.End; 
				break;
			case PageTime.End: pageTime = PageTime.Start; 
				break;
		}
	}
	public PageTime GetPageTime()
	{
		return pageTime;
	}
	public Page GetPage()
	{
		return gamepage;
	}
	private void DrawPageAction()
	{
		
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
	#endregion

	#region Info for Ai
	public Turn GetTurn { 
		get { return turn; } set { }
	}

	#endregion

	#region Function for Ai
	public void Ai_NextPage()
	{
		if(turn== Turn.Ai && !IsWorkLeft())
		{
			NextPage();
		}
	}
	#endregion
}
