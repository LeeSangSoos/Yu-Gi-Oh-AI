using UnityEngine;

public class Card : ScriptableObject
{
	#region cardinfo
	[SerializeField]
	private string description; // 효과
	[SerializeField]
	private Sprite cardimage; //이미지

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
	public CardPosition pos; //카드 현재 위치

	#endregion
	
}