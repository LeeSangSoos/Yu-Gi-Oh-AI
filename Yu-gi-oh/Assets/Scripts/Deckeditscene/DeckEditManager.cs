using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckEditManager : MonoBehaviour
{
	public GameObject DeckCardPrefab;

	public CardList Decklist;
	public Transform DeckParent;

	public CardList ExtraDecklist;
	public Transform ExtraDeckParent;

	public EachCardinDeck[] cardobjects;
	public Text DeckSizetext;

	private void Start()
	{

		if (GameManagerScript.editmydeck)
		{
			Decklist = JsonSaveLoad.LoadMyDeckData();
			ExtraDecklist = JsonSaveLoad.LoadMyExtraDeckData();
		}
		else
		{
			Decklist = JsonSaveLoad.LoadAiDeckData();
			ExtraDecklist = JsonSaveLoad.LoadAiExtraDeckData();
		}

		if (Decklist != null)
		{
			foreach (Card card in Decklist)
			{
				GameObject cardGameObject = Instantiate(DeckCardPrefab, DeckParent);
				EachCardinDeck cardSetting = cardGameObject.GetComponent<EachCardinDeck>();
				cardSetting.image.sprite = card.getImage();
				cardSetting.SetCardinfo(card);
				cardSetting.Setdeckmanager(this);
				DeckSizetext.text = Decklist.Count.ToString();
			}
		}
	}

	public void AddToDeck(Card card)
	{
		if (Decklist.Count >= 60) return;
		int count = 0;
		foreach (Card c in Decklist)
		{
			if (c.getname() == card.getname())
			{
				count++;
			}
		}
		if (count == 3) return;
		GameObject cardGameObject = Instantiate(DeckCardPrefab, DeckParent);
		EachCardinDeck cardSetting = cardGameObject.GetComponent<EachCardinDeck>();
		cardSetting.image.sprite = card.getImage();
		cardSetting.SetCardinfo(card);
		cardSetting.Setdeckmanager(this);
		Decklist.Add(card);
		DeckSizetext.text = Decklist.Count.ToString();
	}

	public void AddToExtraDeck()
	{
		foreach (Card card in ExtraDecklist)
		{
			GameObject cardGameObject = Instantiate(DeckCardPrefab, ExtraDeckParent);

			EachCardList cardSetting = cardGameObject.GetComponent<EachCardList>();
			cardSetting.image.sprite = card.getImage();
		}
	}

	//Find card from Deck by card name and draw it from deck
	public void DrawfromDeck(Card card)
	{
		cardobjects = DeckParent.GetComponentsInChildren<EachCardinDeck>();
		foreach (EachCardinDeck child in cardobjects)
		{
			if (child.card.getname() == card.getname())
			{
				child.transform.SetParent(null);

				Destroy(child.gameObject);
				Decklist.Remove(card);
				break;
			}
		}
		DeckSizetext.text = Decklist.Count.ToString();
	}

	public void ToMenu()
	{
		GameManagerScript.mydecksize = JsonSaveLoad.LoadMyDeckData().Count;
		GameManagerScript.myextrasize = JsonSaveLoad.LoadMyExtraDeckData().Count;
		GameManagerScript.aidecksize = JsonSaveLoad.LoadAiDeckData().Count;
		GameManagerScript.aiextrasize = JsonSaveLoad.LoadAiExtraDeckData().Count;
		SceneManager.LoadScene("MenuScene");
	}

	public void SaveDeck()
	{
		if(GameManagerScript.editmydeck)
			JsonSaveLoad.SaveMyDeckData(Decklist, ExtraDecklist);
		else
			JsonSaveLoad.SaveAiDeckData(Decklist, ExtraDecklist);
	}
}
