using UnityEngine;

public class Card : ScriptableObject
{
	private string description; // ȿ��
	[SerializeField]
	private Sprite cardimage; //�̹���

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