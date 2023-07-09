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

	public MonsterCardType monstertype; // ����
	public Attribute attribute; // �Ӽ�
	public int level; //����, ��ũ, ��ũ
	public List<Archetype> archetype; // ȿ��, ����, ��ⷳ, ���Ǹ� ��

	public int atk; //���ݷ�
	public int def; //�����

}