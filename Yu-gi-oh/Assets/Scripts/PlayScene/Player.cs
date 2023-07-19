using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	#region values
	private CardList hand = new CardList();
	private CardList maindeck = new CardList();
	private CardList extradeck = new CardList();
	private CardList grave = new CardList();
	private CardList exiled = new CardList();
	private CardList monsterfield = new CardList();
	private CardList magicfield = new CardList();
	private Card fieldmagic;
	public Transform HandParent;
	public GameObject HandPrefab;
	public bool ai;
	public Sprite backsideimage;
	[SerializeField]
	private Player enemy;
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
	public Player Enemy { get { return enemy; } }
	#endregion
	#region hand functions
	public void AddHand(Card card)
	{
		hand.Add(card);
		GameObject cardobject = Instantiate(HandPrefab, HandParent);
		cardobject.name = card.CardName;
		if (ai)
		{
			cardobject.GetComponent<Image>().sprite = backsideimage;
		}
		else
		{
			cardobject.GetComponent<Image>().sprite = card.CardImage;
		}
	}
	public Card SubtractHand(Card card)
	{
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
	#endregion


	private void Update()
	{

	}
}
