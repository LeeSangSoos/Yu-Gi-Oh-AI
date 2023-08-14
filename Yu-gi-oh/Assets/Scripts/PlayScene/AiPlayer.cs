using UnityEngine;

public class AiPlayer : MonoBehaviour
{
	public PlayManagerScript game;
	public Player player;

	void Start()
	{

	}

	void Update()
	{
		if( game.GetTurn==Turn.Ai)
		{
			game.Ai_NextPage();
		}
	}
}
