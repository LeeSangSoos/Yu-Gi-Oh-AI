using System.Collections.Generic;
using System.Diagnostics;

[System.Serializable]
public class CardList : IEnumerable<Card>
{
	public List<Card> cards;

	public CardList() { cards = new List<Card>(); }
	public CardList(CardList cardlist) { cards = new List<Card>(cardlist.cards); }
	public IEnumerator<Card> GetEnumerator()
	{
		return cards.GetEnumerator();
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return ((System.Collections.IEnumerable)cards).GetEnumerator();
	}

	public int Count
	{
		get { return cards.Count; }
	}

	public void Add(Card card)
	{
		if (card != null)
		{
			cards.Add(card);
		}
		else
		{
			Debug.WriteLine("Error CardList : No cards to Add");
		}
	}

	public void Remove(Card card)
	{
		if (cards.Contains(card))
		{
			cards.Remove(card);
		}
		else
		{
			Debug.WriteLine("Attempted to remove a card that doesn't exist in the list.");
		}
	}

	public Card this[int index]
	{
		get
		{
			if (index >= 0 && index < cards.Count)
			{
				return cards[index];
			}
			else
			{
				Debug.WriteLine($"Invalid index {index} for accessing card. Returning null.");
				return null;
			}
		}
		set
		{
			if (index >= 0 && index < cards.Count)
			{
				cards[index] = value;
			}
			else
			{
				Debug.WriteLine($"Invalid index {index} for setting card. The card is not added to the list.");
			}
		}
	}

	public void RemoveAt(int index)
	{
		cards.RemoveAt(index);
	}

	public void Clear() { cards.Clear(); }
}
