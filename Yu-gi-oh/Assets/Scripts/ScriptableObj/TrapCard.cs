using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "TrapCard")]
public class TrapCard : Card
{
	public enum TrapCardtype
	{
		Normal
	}

	public TrapCardtype traptype;
}
