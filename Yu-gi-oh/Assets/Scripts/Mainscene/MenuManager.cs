using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public GameManagerScript gamemanager;
	public Text decksizenotice;
	public void ToDeckEdit()
	{
		SceneManager.LoadScene("DeckEditScene");
		GameManagerScript.editmydeck = true;
	}

	public void ToAiDeckEdit()
	{
		SceneManager.LoadScene("DeckEditScene");
		GameManagerScript.editmydeck = false;
	}

	public void StartPlay()
	{
		decksizenotice.text = "";
		decksizenotice.text += ("UserDeckSize: " + GameManagerScript.mydecksize.ToString() + "\n");
		decksizenotice.text += ("UserExtraDeckSize: " + GameManagerScript.myextrasize.ToString() + "\n");
		decksizenotice.text += ("AiDeckSize: " + GameManagerScript.aidecksize.ToString() + "\n");
		decksizenotice.text += ("AiExtraDeckSize: " + GameManagerScript.aiextrasize.ToString());
		if (GameManagerScript.mydecksize >= 40 && GameManagerScript.mydecksize <= 60 && GameManagerScript.myextrasize <=15 &&
			GameManagerScript.aidecksize >= 40 && GameManagerScript.aidecksize <= 60 && GameManagerScript.aiextrasize <= 15)
		{
			SceneManager.LoadScene("PlayScene");
		}
	}

}
