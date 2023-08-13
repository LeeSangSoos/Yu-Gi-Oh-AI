using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "MonsterCard")]
public class MonsterCard : Card
{
	#region CardData
	public enum MonsterCardType
	{
		Spellcaster, Warrior, Beast_Warrior, Dinosaur, Beast
	}

	public enum Attribute
	{
		DARK, EARTH
	}

	public enum Archetype
	{
		Normal, Effect
	}

	public MonsterCardType monstertype; // race
	public Attribute attribute; // element
	public int level; //level, rank, link
	public List<Archetype> archetype; // effect, fusion ...

	public int atk; 
	public int def; 
	#endregion
	#region CardEffect
	[SerializeField]
	private SummonMethod summonmMethod;
	public SummonMethod SummonMethod { get { return summonmMethod; } }
	#endregion

}