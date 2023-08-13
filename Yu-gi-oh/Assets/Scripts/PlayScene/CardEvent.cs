using UnityEngine;
using UnityEngine.EventSystems;

public class CardEvent : MonoBehaviour, IPointerClickHandler
{
	public Player player;
	public Card card;
	public PlayerType playertype;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (playertype == PlayerType.Ai) return;
		if (card.pos == CardPosition.Hand)
		{
			player.UserAction(card);
		}
	}
}