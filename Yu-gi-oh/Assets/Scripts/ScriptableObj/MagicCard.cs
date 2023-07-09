using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "MagicCard")]
public class MagicCard : Card
{
	public enum MagicCardtype
	{
		Normal, Equip
	}

	public MagicCardtype magictype;
}
