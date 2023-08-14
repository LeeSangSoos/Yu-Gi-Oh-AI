using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour
{
	#region values
	//Field, turn info
	private CardList hand = new CardList();
	private CardList maindeck = new CardList();
	private CardList extradeck = new CardList();
	private CardList grave = new CardList();
	private CardList exiled = new CardList();
	private CardList monsterfield = new CardList(5);
	private CardList magicfield = new CardList(5);
	private Card linkleftfield;
	private CardList linkrightfield;
	private Card fieldmagic;
	public PlayManagerScript playmanager;
	public bool myturn = false;

	//enemy & work available
	public Player enemy;
	private bool isworking = false;

	//card action in game
	public Card CardOnWork;
	private bool workleft = true;
	public bool WorkLeft
	{
		get { return workleft; }
	}

	//is this user or ai
	[SerializeField] private bool is12;
	private bool isuser;
	[SerializeField] private User userscript;
	[SerializeField] private AiPlayer aiscript;

	//UI
	public Transform HandParent;
	public Sprite backsideimage;
	public List<Transform> MonsterZone;
	public List<Transform> MagicZone;

	//Turn action
	private int turndraw;
	private int turnsummon;
	private int turnpendulum;
	private int turndrawlimit=1;
	private int turnsummonlimit=1;
	private int turnpendulumlimit=1;

	#endregion
	#region Getter&Setter
	public CardList Hand
	{
		get { return hand; }
		set
		{
			CardList cards1 = new CardList(value);
			hand.Clear();
			foreach (Card card in cards1)
			{
				hand.Add(card);
			}
		}
	}
	public CardList MainDeck
	{
		get { return maindeck; }
		set
		{
			CardList cards1 = new CardList(value);
			maindeck.Clear();
			foreach (Card card in cards1)
			{
				maindeck.Add(card);
			}
		}
	}
	public CardList ExtraDeck
	{
		get { return extradeck; }
		set
		{
			CardList cards1 = new CardList(value);
			extradeck.Clear();
			foreach (Card card in cards1)
			{
				extradeck.Add(card);
			}
		}
	}
	public CardList Grave
	{
		get { return grave; }
		set
		{
			CardList cards1 = new CardList(value);
			grave.Clear();
			foreach (Card card in cards1)
			{
				grave.Add(card);
			}
		}
	}
	public CardList Exiled
	{
		get { return exiled; }
		set
		{
			CardList cards1 = new CardList(value);
			exiled.Clear();
			foreach (Card card in cards1)
			{
				exiled.Add(card);
			}
		}
	}
	public CardList MonsterField
	{
		get { return monsterfield; }
		set
		{
			CardList cards1 = new CardList(value);
			monsterfield.Clear();
			foreach (Card card in cards1)
			{
				monsterfield.Add(card);
			}
		}
	}
	public CardList MagicField
	{
		get { return magicfield; }
		set
		{
			CardList cards1 = new CardList(value);
			magicfield.Clear();
			foreach (Card card in cards1)
			{
				magicfield.Add(card);
			}
		}
	}
	public Card FieldMagic
	{
		get { return fieldmagic; }
		set
		{
			fieldmagic = value;
		}
	}
	public void setPlayerType(PlayerType p)
	{
		if (p == PlayerType.User)
		{
			isuser=true;
		}
		else
		{
			isuser=false;
		}
	}
	public PlayerType GetPlayerType() { return isuser ? PlayerType.User : PlayerType.Ai; }
	#endregion
	#region hand functions
	public void AddHand(Card card)
	{
		hand.Add(card);
		card.CardObejct.transform.SetParent(HandParent, false);
		card.CardObejct.SetActive(true);
		if (is12)
		{
			card.CardObejct.GetComponent<Image>().sprite = backsideimage;
		}
		else
		{
			card.CardObejct.GetComponent<Image>().sprite = card.CardImage;
		}
		card.pos = CardPosition.Hand;
	}
	public Card SubtractHand(Card card)
	{
		Card result = hand.Find(c => card.GetCardNum() == c.GetCardNum());
		hand.Remove(card);
		Destroy(HandParent.Find(card.CardName).gameObject);
		return card;
	}
	public void draw()
	{
		if (maindeck.Count == 0) { Debug.Log("No Deck left for player"); }
		else
		{
			AddHand(maindeck[0]);
			maindeck.RemoveAt(0);

		}
	}
	public void RemoveMonsterFromField(int pos)
	{
		if (pos >= 0 && pos < monsterfield.Count)
		{
			monsterfield[pos] = null;
			MonsterZone[pos].GetComponent<Image>().sprite = backsideimage;
			MonsterZone[pos].eulerAngles = new Vector3(0, 0, 0);
		}
	}

	public void NormalSummon(Card card, int pos)
	{
		if (monsterfield[pos] != null) return;
		Card targetcard = SubtractHand(card);
		monsterfield[pos] = targetcard;
		MonsterZone[pos].GetComponent<Image>().sprite = targetcard.CardImage;
	}

	#region monsterhandfunction
	public void Summon1_FromHand()
	{
		MonsterCard card = (MonsterCard)CardOnWork;
		if (card.SummonMethod == SummonMethod.Normal)
		{

		}
	}
	public void Set()
	{

	}
	public void Summon2()
	{

	}
	public void Effect()
	{

	}
	#endregion
	#endregion
	#region GeneralFunctions

	/*void AbleToWork()
	{
		if (isworking)
		{
			Debug.Log("Waiting for work to be done");
			while (isworking)
			{
				
			}
			isworking = true;
			Debug.Log("Work done");
		}
	}*/
	public void UserAction(Card card)
	{
		userscript.CardClickEvent(card);
	}
	#endregion
	#region TimeFunctions
	private void Update()
	{
		switch (playmanager.GetPage())
		{
			case Page.Draw:
				DrawPageWork();
				break;
			case Page.Standby:
				StandbyPageWork();
				break;
			case Page.Main1:
				Main1PageWork();
				break;
			case Page.Battle:
				BattlePageWork();
				break;
			case Page.Main2: 
				Main2PageWork();
				break;
			case Page.End:
				EndPageWork();
				break;
		}
	}
	#endregion
	#region Check for work
	//Function for checking the effect of card
	//Function for checking the work to do
	public void SetTurnActions()
	{
		turndraw = turndrawlimit;
		turnsummon = turnsummonlimit;
		turnpendulum = turnpendulumlimit;
	}
	void DrawPageWork()
	{
		switch (playmanager.GetPageTime())
		{
			case PageTime.Start:
				if (myturn)
				{
					while (turndraw>=1)
					{
						draw();
						turndraw--;
					}
				}
				workleft = false;
				break;
			case PageTime.OnGoing:
				workleft = false;
				break;
			case PageTime.End:
				workleft = false;
				break;
		}
	}
	void StandbyPageWork()
	{

	}
	void Main1PageWork()
	{
		switch(playmanager.GetPageTime())
		{
			case PageTime.Start:
				workleft = false;
				break;
			case PageTime.OnGoing:
				workleft = true;
				break;
			case PageTime.End:
				break;
		}
	}
	void BattlePageWork()
	{

	}
	void Main2PageWork()
	{

	}
	void EndPageWork()
	{

	}
	#endregion
}
