using System.Collections.Generic;
using Unity.VisualScripting;

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

	public int Count { 
		get { return cards.Count; }
	}

	public void Add(Card card)
	{
		cards.Add(card);
	}

	public void Remove(Card card)
	{
		cards.Remove(card);
	}

	public Card this[int index]
	{
		get { return cards[index]; }
		set { cards[index] = value; }
	}

	public void RemoveAt(int index)
	{
		cards.RemoveAt(index);
	}

	public void Clear() { cards.Clear(); }
}
