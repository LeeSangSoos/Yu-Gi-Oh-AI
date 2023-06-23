using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckEditManager : MonoBehaviour
{
	public void ToMenu()
	{
		SceneManager.LoadScene("MenuScene");
	}
}
