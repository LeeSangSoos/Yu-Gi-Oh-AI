using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.GraphicsBuffer;

public class AiPlayer : MonoBehaviour
{
	public PlayManagerScript playmanager;
	public Player player;

	void Update()
	{
		if (player.myturn)
		{
			if ((playmanager.GetPage() == Page.Main1 || playmanager.GetPage() == Page.Main2) && playmanager.GetPageTime()==PageTime.OnGoing)
			{
				/*Normal summon process
					 * Check player.turnsummon >= 1
					 * Check if there is monster in hand that can be summoned
					 * if there were, summon a choose a random monster to summon from hand
					 * find place to summon and victoms
					 * summon
					 */
				if (player.turnsummon >= 1)
				{
					List<MonsterCard> monsters = new List<MonsterCard>();
					int CountMonsterOnField = player.MonsterField.Count(card => card != null);
					
					int PlaceToSummon = -1;

					foreach (Card card in player.Hand)
					{
						if (card is MonsterCard)
						{
							MonsterCard monsterCard = card as MonsterCard;

							if (monsterCard.level >= 7 && CountMonsterOnField >= 2)
							{
								monsters.Add(monsterCard);
							}
							else if (monsterCard.level >= 5 && monsterCard.level < 7 && CountMonsterOnField >= 1)
							{
								monsters.Add(monsterCard);
							}
							else if (monsterCard.level <= 4)
							{
								monsters.Add(monsterCard);
							}
						}
					}

					if (monsters.Count() > 0)
					{
						int rand = Random.Range(0, monsters.Count());
						MonsterCard monster = monsters[rand];

						List<int> vics = FindVictom(monster);

						for (int i = 0; i < 5; i++)
						{
							if (vics.Contains(i) || player.MonsterField[i] == null)
							{
								PlaceToSummon = i; break;
							}
						}

						if (PlaceToSummon != -1)
						{
							player.PlayerOnWork();
							rand = Random.Range(0, 10);
							if (rand < 5)
							{
								player.SetMonster(monster, PlaceToSummon, monster.level, vics[0], vics[1]);
							}
							else
								player.NormalSummon(monster, PlaceToSummon, monster.level, vics[0], vics[1]);
						}
					}
				}

				/*Magic/Trap set
				 */
				for (int i = 0; i < 5; i++)
				{
					if (player.MagicTrapField[i] == null)
					{
						foreach (Card hand in player.Hand.ToList())
						{
							if (hand is MagicCard || hand is TrapCard)
							{
								player.SetMagicTrap(hand, i);
							}
						}
					}
				}

				/*Magic activate
				 * See set cards.
				 * if card is available to act activate it
				 * 
				*/
				for (int i = 0; i < player.MagicTrapField.Count; i++)
				{
					MagicCard magiccard;
					if (player.MagicTrapField[i] is MagicCard)
					{
						magiccard = player.MagicTrapField[i] as MagicCard;
						if (magiccard.EffectCondition())
						{
							if (!playmanager.IsWorkLeft())
							{
								player.PlayerOnWork();
								player.MagicTrapEffectOnField(magiccard);
							}
						}
					}
				}

				/*Magic Handactivate
				 * 
				for (int i = 0; i < player.Hand.Count; i++)
				{
					MagicCard magiccard;
					if(player.Hand[i] is MagicCard)
					{
						magiccard = player.Hand[i] as MagicCard;
						if (magiccard.EffectCondition()) {
							player.PlayerOnWork();
							for (int j = 0; j < 5;j++)
							{
								if (player.MagicTrapField[j] == null)
								{
									player.MagicTrapEffectFromHand(magiccard, j);
									break;
								}
							}
						}
					}
				}
				*/

			}
			else if (playmanager.GetPage() == Page.Battle)
			{

				if (player.MonsterField.Any(monsterCard => monsterCard != null))
				{
					for (int m=0;m<player.MonsterField.Count;m++)
					{
						MonsterCard monsterCard = player.MonsterField[m] as MonsterCard;
						if (monsterCard != null)
						{
							if (monsterCard.attackchance >= 1 && monsterCard.iscardfaceup)
							{
								int target = -1;
								for (int i = 0; i < 5; i++)
								{
									if (player.enemy.MonsterField[i] != null)
									{
										MonsterCard enemy = player.enemy.MonsterField[i] as MonsterCard;
										target = i;
										Debug.Log("Ai Attacked");
										break;
									}
								}

								if (target != -1)
									player.Attack(monsterCard, target);
								else
									player.DirectAttack(monsterCard);
							}
						}
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

	List<int> FindVictom(MonsterCard monster)
	{
		List<int> list = new List<int>{ -1, -1 };
		List<int> monslist = new List<int>();

		for(int i=0;i<5;i++)
		{
			if (player.MonsterField[i] != null)
			{
				monslist.Add(i);
			}
		}

		if (monster.level >= 7)
		{
			if(monslist.Count < 2) { return list; }
			int rand = Random.Range(0, monslist.Count);
			list[0] = (rand);
			monslist.RemoveAt(rand);
			rand = Random.Range(0, monslist.Count);
			list[1] = (rand);
		}
		else if(monster.level >= 5)
		{
			if (monslist.Count < 1) { return list; }
			int rand = Random.Range(0, monslist.Count);
			list[0] = (rand);
		}

		return list;
	}

	public void SeeCardsToActivate(List<Card> cards, MonsterCard target)
	{
		if (target.atk >= 2000)
		{
			player.EffectAddToChain(cards[0], target);
		}
		else
		{
			player.PlayerEndWork();
			player.CancelEffect();
		}
	}

	public Card ChooseTargetCard(MagicCard card)
	{
		MonsterCard target = new MonsterCard();
		foreach(MonsterCard t in player.MonsterField)
		{
			if (CheckTarget(card, t))
			{
				target = t; break;
			}
		}
		foreach (MonsterCard t in player.enemy.MonsterField)
		{
			if (CheckTarget(card, t))
			{
				target = t; break;
			}
		}
		return target;
	}

	public bool CheckTarget(Card card, Card target)
	{
		if (card.NeedTarget)
		{
			if (card.Autotarget)
			{
				target = card.GetAutoTarget();
			}
			if (target == null)
			{
				return false;
			}
			if (card.Needtargetpos == CardPosition.MonsterField)
			{
				if (target.pos != CardPosition.MonsterField)
				{
					return false;
				}
			}
			if (card.Needtargetowner == TargetOwner.Mine)
			{

			}
			if (card.Needtargettype == TargetType.Monster)
			{
				if (target is not MonsterCard)
				{
					return false;
				}
			}
		}
		return true;
	}

}
