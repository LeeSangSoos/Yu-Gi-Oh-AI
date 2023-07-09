using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{

	public string cardName; //이름
	public string description; // 효과

	public Sprite image; //이미지

	public string getname() { return cardName; }
	public string getDescription() { return description; }
	public Sprite getImage() { return image; }
}