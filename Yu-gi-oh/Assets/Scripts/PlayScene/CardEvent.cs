using UnityEngine;
using UnityEngine.EventSystems;

public class CardEvent : MonoBehaviour, IPointerClickHandler
{
	public Player player;
	public Card card;
	public void OnPointerClick(PointerEventData eventData)
	{
		if(card is MonsterCard && card.pos==CardPosition.Hand)
		{
			player.OpenHandMonsterEvent(card);
		}
		else
		{

		}
	}
}