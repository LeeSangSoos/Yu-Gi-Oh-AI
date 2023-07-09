using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{

	public string cardName; //�̸�
	public string description; // ȿ��

	public Sprite image; //�̹���

	public string getname() { return cardName; }
	public string getDescription() { return description; }
	public Sprite getImage() { return image; }
}