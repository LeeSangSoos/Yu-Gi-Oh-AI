using UnityEngine;

public class User : MonoBehaviour
{
	//UI
	public Transform HandParent;
	public GameObject HandPrefab;
	public Sprite backsideimage;
	public bool ai;
	public Player player;
	Card cardonwork;

	//User UI
	public GameObject MonsterHandEventPanel;
	public GameObject SummonChoicePanel;

	//what summon
	SummonMethod summonMethodOnWork;
	int postosummon = 0;

	#region Timefunctions
	private void Start()
	{
		MonsterHandEventPanel.SetActive(false);
		SummonChoicePanel.SetActive(false);
	}
	#endregion
	#region UIFunction
	public void CardClickEvent(Card card)
	{
		if(card.owner != player)
		{

		}
		else
		{
			switch (card.pos)
			{
				case CardPosition.Hand: MonsterHandEventPanel.SetActive(true); break;
				case CardPosition.MainDeck:break;
				case CardPosition.ExtraDeck:break;
				case CardPosition.Grave: break;
				case CardPosition.Exiled: break;
				case CardPosition.MonsterField: break;
				case CardPosition.MagicField: break;
				case CardPosition.FieldMagic: break;
				default: break;
			}
		}
		player.CardOnWork = card;
	}
	public void NormalSummon()
	{
		MonsterHandEventPanel.SetActive(false);
		SummonChoicePanel.SetActive(true );
		summonMethodOnWork = SummonMethod.Normal;
	}
	public void FieldMagicZone() {  }
	public void LinkZoneLeft() { }
	public void LinkZoneRight() { }
	 
	public void MonsterZone1() { player.NormalSummon(cardonwork, 1);	}
	public void MonsterZone2() { player.NormalSummon(cardonwork, 2); }
	public void MonsterZone3() { player.NormalSummon(cardonwork, 3); }
	public void MonsterZone4() { player.NormalSummon(cardonwork, 4); }
	public void MonsterZone5() { player.NormalSummon(cardonwork, 5); }

	public void MagicZone1() { }
	public void MaigcZone2() { }
	public void MagicZone3() { }
	public void MagicZone4() { }
	public void MagicZone5() { }
	#endregion
}
