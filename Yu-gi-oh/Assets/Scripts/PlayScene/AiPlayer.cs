using UnityEngine;

public class AiPlayer : MonoBehaviour
{
	public PlayManagerScript playmanager;
	public Player player;

	void Start()
	{

	}

	void Update()
	{
		if (player.myturn)
		{
			if (playmanager.GetPage() == Page.Main1)
			{
				if (player.Hand[0] is MonsterCard && player.MonsterField[0]==null)
				{
					MonsterCard mons = player.Hand[0] as MonsterCard;
					if (mons.level <= 4 && mons.atk >= 1000)
					{
						player.PlayerOnWork();
						player.NormalSummon(player.Hand[0], 0, mons.level, -1, -1);
					}
				}
			}

			//Call Next Page
			Ai_NextPage();
		}

		//DisCard Hand at end
		if (player.myturn && playmanager.GetPage() == Page.End && player.discardhand)
		{
			Card card = player.Hand[0];
			player.DiscardHand(card);
		}
	}
	void Ai_NextPage()
	{
		if (player.myturn)
		{
			if (player.WorkLeft) player.NoWorkLeft();
			player.ToPageEnd();
		}
	}

	void Ai_Summon(Card card)
	{
		/*
		case SummonMethod.Normal:
			if (monsterCardOnWork.level < 5)
			{
				player.NormalSummon(cardonwork, pos, monsterCardOnWork.level, -1, -1);
			}
			else if (monsterCardOnWork.level >= 5 && monsterCardOnWork.level <= 6)
			{
				player.NormalSummon(cardonwork, pos, monsterCardOnWork.level, poslist[0], -1);
			}
			else
			{
				player.NormalSummon(cardonwork, pos, monsterCardOnWork.level, poslist[0], poslist[1]);
			}
			FieldChoicePanel.SetActive(false);
			poslist.Clear();
			break;
		}
		*/
	}
}
