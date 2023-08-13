using UnityEngine;

public class Card : ScriptableObject
{
	#region cardinfo
	[SerializeField]
	private string description;
	[SerializeField]
	private Sprite cardimage;

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
	#region Card Public Data
	public CardPosition pos; //Card position on field
	public Player owner; //Card possesion
	private Player originowner; //Card original owner
	public void SetOriginOwner(Player player) { originowner = player; }
	public Player GetOriginOwner() {  return originowner; }
	private int cardnum; //Card identification number
	public void SetCardNum(int n)
	{
		cardnum = n;
	}
	public int GetCardNum() {  return cardnum; }
	private GameObject cardobject;
	public void SetCardObject(GameObject cardo) { cardobject = cardo; }
	public GameObject CardObejct { get { return cardobject; } }

	#endregion

}