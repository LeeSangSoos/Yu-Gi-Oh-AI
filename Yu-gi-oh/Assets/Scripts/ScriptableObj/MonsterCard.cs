using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "MonsterCard")]
public class MonsterCard : Card
{
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

	public MonsterCardType monstertype; // 종족
	public Attribute attribute; // 속성
	public int level; //레벨, 랭크, 링크
	public List<Archetype> archetype; // 효과, 융합, 펜듈럼, 스피릿 등

	public int atk; //공격력
	public int def; //수비력

}