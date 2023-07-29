using UnityEngine;

public class Card : ScriptableObject
{
	#region cardinfo
	[SerializeField]
	private string description; // ȿ��
	[SerializeField]
	private Sprite cardimage; //�̹���

	public string Description
	{
		get { return description ?? ""; }
	}
	public string CardName
	{
		get { return name.ToString(); }
	}
	public Sprite CardImage
	{
		get { return cardimage; }
	}
	#endregion
	#region cardeffect
	

	#endregion
	#region cardpublicdata
	public CardPosition pos; //ī�� ���� ��ġ

	#endregion
	
}