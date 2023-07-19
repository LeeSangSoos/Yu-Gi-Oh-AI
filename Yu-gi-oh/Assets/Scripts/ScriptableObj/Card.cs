using UnityEngine;

public class Card : ScriptableObject
{
	private string description; // 효과
	[SerializeField]
	private Sprite cardimage; //이미지

	public string CardName
	{
		get { return name; }
		set { }
	}
	public string Description
	{
		get { return description; }
		set { }
	}
	public Sprite CardImage
	{
		get { return cardimage; }
		set { }
	}
}