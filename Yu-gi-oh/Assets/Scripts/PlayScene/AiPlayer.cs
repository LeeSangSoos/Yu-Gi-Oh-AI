using UnityEngine;

public class AiPlayer : MonoBehaviour
{
	public PlayManagerScript game;

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
