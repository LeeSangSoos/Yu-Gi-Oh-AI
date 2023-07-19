using System.IO;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
	public static GameManagerScript instance;
	public static bool editmydeck;
	private static bool OnGameStart=true;
	
	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}
		
		MakeJsonFiles();
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		
		if (OnGameStart)
		{
			
		}
		OnGameStart = false;
		
	}


	// Check and make json files
	private void MakeJsonFiles()
	{
		string myDeckFilePath = Path.Combine(Application.persistentDataPath, "MyDeckData.json");
		string myExtraDeckFilePath = Path.Combine(Application.persistentDataPath, "MyExtraDeckData.json");
		string aiDeckFilePath = Path.Combine(Application.persistentDataPath, "AiDeckData.json");
		string aiExtraDeckFilePath = Path.Combine(Application.persistentDataPath, "AiExtraDeckData.json");
		CardList Deck = new CardList();
		string json = JsonUtility.ToJson(Deck);

		if (!File.Exists(myDeckFilePath))
		{
			File.WriteAllText(myDeckFilePath, json);
		}
		if (!File.Exists(myExtraDeckFilePath))
		{
			File.WriteAllText(myExtraDeckFilePath, json);
		}
		if (!File.Exists(aiDeckFilePath))
		{
			File.WriteAllText(aiDeckFilePath, json);
		}
		if (!File.Exists(aiExtraDeckFilePath))
		{
			File.WriteAllText(aiExtraDeckFilePath, json);
		}

		
	}

}
