using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class AiPlayer : MonoBehaviour
{
	public PlayManagerScript playmanager;
	public Player player;
	float stepDelay = 1f;
	bool step_on_work = false;
	int signleFromAi = 0;
	bool gotSignleFromAi = false;
	string actionString = "";
	List<string> actionList;

	IEnumerator StepCoroutine()
	{
		while (true) // Infinite loop to keep the coroutine running
		{
			Step();
			yield return new WaitForSeconds(stepDelay); // Wait for few seconds before the next step
		}
	}

	private void Awake()
	{
		LoadActionMapping();
	}
	private void Start()
	{
		StartCoroutine(StepCoroutine());
	}

	void Step()
	{
		if (step_on_work) { return; }
		if (player.myturn)
		{
			step_on_work = true;
			//Give data to ai & signle
			if((playmanager.GetPage() == Page.Main1 && playmanager.GetPageTime() == PageTime.OnGoing) || 
				(playmanager.GetPage() == Page.Battle && playmanager.GetPageTime() == PageTime.OnGoing) || 
				(playmanager.GetPage() == Page.End && playmanager.GetPageTime() == PageTime.End))
			{
				SignleAi();
				//Wait for action
				WaitForSignleFromAi();
			}
			else
			{
				//Call Next Page
				Ai_NextPage();
			}
		}

		step_on_work = false;
	}

	void Ai_NextPage()
	{
		if (player.myturn)
		{
			if(playmanager.GetPage() == Page.End && playmanager.GetPageTime() == PageTime.End && player.Hand.Count > 6)
			{
				
			}
			else
			{
				if (player.WorkLeft) player.NoWorkLeft();

				player.ToPageEnd();
			}
		}
	}

	public void SeeCardsToActivate(List<Card> cards, Card target)
	{
		player.EffectAddToChain(cards[0], target);
	}

	public Card ChooseTargetCard(MagicCard card)
	{
		MonsterCard target = null;
		foreach (MonsterCard t in player.MonsterField)
		{
			if (card.TargetTempCondition(t))
			{
				target = t; break;
			}
		}
		foreach (MonsterCard t in player.enemy.MonsterField)
		{
			if (card.TargetTempCondition(t))
			{
				target = t; break;
			}
		}
		return target;
	}

	public void SignleAi()
	{
		playmanager.SavePlayer(player.enemy, "HumanPlayer.json");
		playmanager.SavePlayer(player, "DqnPlayer.json");
		playmanager.SignleAi();
	}

	void WaitForSignleFromAi()
	{
		while (true)
		{
			string filePath = Path.Combine(Application.persistentDataPath, "SignleGame.json");
			if (File.Exists(filePath))
			{
				string json = File.ReadAllText(filePath);
				int signle2unity = JsonConvert.DeserializeObject<int>(json);

				if (signleFromAi < signle2unity)
				{
					signleFromAi = signle2unity;
					playmanager.Single2Ai++; 
					break;
				}
			}
			System.Threading.Thread.Sleep(1000);
		}
		DoAction();
	}

	public void DoAction()
	{
		DecodeAction();
		string[] actionValues = actionString.Split('-');
		if (actionValues[0] == "endpage")
		{
			Ai_NextPage();
		}
		else if (actionValues[0] == "summon")
		{
			int PlaceToSummon = -1;
			int[] vics = new int[2];
			MonsterCard monster = player.Hand[int.Parse(actionValues[2])] as MonsterCard;
			if (actionValues[1] == "0")
			{
				PlaceToSummon = GetPlaceToSummon(vics);
				if(actionValues[3] == "faceup")
					player.NormalSummon(monster, PlaceToSummon, monster.level, vics[0], vics[1]);
				else
				{
					player.SetMonster(monster, PlaceToSummon, monster.level, vics[0], vics[1]);
				}
			}
			else if (actionValues[1] == "1")
			{
				vics[0] = int.Parse(actionValues[3]);
				PlaceToSummon = GetPlaceToSummon(vics);
				if (actionValues[4] == "faceup")
					player.NormalSummon(monster, PlaceToSummon, monster.level, vics[0], vics[1]);
				else
				{
					player.SetMonster(monster, PlaceToSummon, monster.level, vics[0], vics[1]);
				}
			}
			else if (actionValues[1] == "2")
			{
				vics[0] = int.Parse(actionValues[3]);
				vics[1] = int.Parse(actionValues[4]);
				PlaceToSummon = GetPlaceToSummon(vics);
				if (actionValues[5] == "faceup")
					player.NormalSummon(monster, PlaceToSummon, monster.level, vics[0], vics[1]);
				else
				{
					player.SetMonster(monster, PlaceToSummon, monster.level, vics[0], vics[1]);
				}
			}
		}
		else if (actionValues[0] == "attack")
		{
			MonsterCard monsterCard = player.Hand[int.Parse(actionValues[1])] as MonsterCard;
			int target = int.Parse(actionValues[2]);
			if (actionValues[2] == "direct")
			{
				player.DirectAttack(monsterCard);
			}
			else
			{
				player.Attack(monsterCard, target);
			}
		}
		else if (actionValues[0] == "discard")
		{
			player.DiscardHand(player.Hand[int.Parse(actionValues[1])]);
		}
	}

	void DecodeAction()
	{
		//decode action
		string filePath = Path.Combine(Application.persistentDataPath, "DqnAction.json");
		if (File.Exists(filePath))
		{
			string json = File.ReadAllText(filePath);
			int action = JsonConvert.DeserializeObject<int>(json);

			actionString = actionList[action];
		}
	}

	void LoadActionMapping()
	{
		string filePath = Path.Combine(Application.persistentDataPath, "action_space.json");
		if (File.Exists(filePath))
		{
			string json = File.ReadAllText(filePath);
			Dictionary<string, int> actionMapping = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

			actionList = new List<string>(new string[actionMapping.Count]);
			foreach (var pair in actionMapping)
			{
				actionList[pair.Value] = pair.Key;
			}
		}
	}

	int GetPlaceToSummon(int[] vics)
	{
		int PlaceToSummon = -1;
		for (int i = 0; i < 5; i++)
		{
			if (vics.Contains(i) || player.MonsterField[i] == null)
			{
				PlaceToSummon = i; break;
			}
		}
		return PlaceToSummon;
	}
}
