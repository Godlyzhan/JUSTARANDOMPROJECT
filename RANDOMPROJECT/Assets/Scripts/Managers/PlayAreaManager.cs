using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayAreaManager : MonoBehaviour
{
	[SerializeField] private GameManager gameManager;
	[SerializeField] private CardDeck cardDeck;
	[SerializeField] private RectTransform playAea;
	[SerializeField, Range(0.2f, 5f)] private List<float> cardSpacings = new List<float>();

	private int numberOfRows = 4;
	private int numberOfColumns = 4;
	private float cardSpacing;
	private float cardScale;

	private List<GameObject> cards = new List<GameObject>();
	private List<Vector2> cardPositions = new List<Vector2>();
	private List<CardIdentifier.CardID> cardIds = new List<CardIdentifier.CardID>();

	public string GameMode { get; private set; }
	public int BestScore { get; set; }

	private void InstantiateCards()
	{
		RemoveCardInPlay();
		GenerateGrid();
	}

	public void SelectGameMode(GameMode gameMode)
	{
		numberOfColumns = gameMode.Columns;
		numberOfRows    = gameMode.Rows;
		GameMode        = $"{numberOfColumns}x{numberOfRows}";

		if (SaveData.GameModeScores.Count > 0)
		{
			foreach (var gameModeScore in SaveData.GameModeScores)
			{
				if (gameModeScore.GameMode == GameMode)
				{
					BestScore = gameModeScore.BestScore;
					break;
				}
			}
		}

		InstantiateCards();
	}

	public void LoadCardsInPlay()
	{
		for (int i = 0; i < SaveData.CardIdentifiers.Count; i++)
		{
			for (int j = 0; j < cardDeck.CardIdentifier.Count; j++)
			{
				if (SaveData.CardIdentifiers[i] == (int)cardDeck.CardIdentifier[j].CardID)
				{
					GameObject newCard = Instantiate(cardDeck.Cards[j], SaveData.CardPositions[i], Quaternion.identity);
					newCard.transform.SetParent(transform);
					newCard.transform.localScale *= cardScale;
					cards.Add(newCard);
					cardIds.Add(newCard.GetComponent<Card>().CardID);
				}
			}
		}
	}

	public void RemoveCardInPlay()
	{
		if (cards.Count > 0)
		{
			foreach (GameObject card in cards)
			{
				DestroyImmediate(card);
			}
			cards.Clear();
		}

		cardIds.Clear();
		cardPositions.Clear();
	}

	public void RemoveCardFromPlay(GameObject cardToRemove, CardIdentifier.CardID cardID)
	{
		if (cardToRemove != null)
		{
			cards.Remove(cardToRemove);
			cardIds.Remove(cardID);
			Destroy(cardToRemove);
		}

		if (cards.Count == 0)
		{
			gameManager.EndGame();
			gameManager.CanContinue = false;
			SaveData.CardIdentifiers.Clear();
			SaveData.CardPositions.Clear();
			cards.Clear();
			cardIds.Clear();
		}
	}

	public void BuildCardIndex()
	{
		for (int i = 0; i < cards.Count; i++)
		{
			var id           = cardIds[i];
			var cardPosition = new Vector2(cards[i].transform.position.x, cards[i].transform.position.y);

			SaveData.CardIdentifiers.Add((int)id);
			SaveData.CardPositions.Add(cardPosition);
			gameManager.CanContinue = true;
		}
	}

	private void GenerateGrid()
	{
		SetSpacingSize();

		// Card are the same size so we can grab the first card
		Transform cardTransform = cardDeck.Cards[0].transform;
		Vector3   scale     = cardTransform.localScale;

		float cardWidth  = scale.x + cardSpacing;
		float cardHeight = scale.y + cardSpacing;
		
		float gridWidth  = numberOfColumns * cardWidth;
		float gridHeight = numberOfRows * cardHeight;

		Vector3 position = playAea.position;

		float startX = position.x - (gridWidth / 2) + (scale.x / 2);
		float startY = position.y + (gridHeight / 2) - (scale.y / 2);

		float minX = position.x - playAea.sizeDelta.x / 2;
		float maxX = position.x + playAea.sizeDelta.x / 2;
		float minY = position.y - playAea.sizeDelta.y / 2;
		float maxY = position.y + playAea.sizeDelta.y / 2;

		for (int row = 0; row < numberOfRows; row++)
		{
			for (int column = 0; column < numberOfColumns; column++)
			{
				float posX = startX + column * (scale.x + cardSpacing);
				float posY = startY - row * (scale.y + cardSpacing);

				posX = Mathf.Clamp(posX, minX, maxX);
				posY = Mathf.Clamp(posY, minY, maxY);

				Vector2 cardPos = new Vector2(posX, posY);
				cardPositions.Add(cardPos);
			}
		}

		SpawnCards();
	}

	private async void SpawnCards()
	{
		List<GameObject> duplicatedCards = new List<GameObject>();

		foreach (GameObject card in cardDeck.Cards)
		{
			if (duplicatedCards.Count == numberOfRows * numberOfColumns)
			{
				break;
			}

			for (int i = 0; i < 2; i++)
			{
				duplicatedCards.Add(card);
			}
		}

		Shuffle(duplicatedCards);

		await Task.Delay(TimeSpan.FromSeconds(0.5f));

		for (int i = 0; i < Mathf.Min(cardPositions.Count, duplicatedCards.Count); i++)
		{
			GameObject newCard = Instantiate(duplicatedCards[i], cardPositions[i], Quaternion.identity);
			newCard.transform.SetParent(transform);
			newCard.transform.localScale *= cardScale;

			cards.Add(newCard);
			cardIds.Add(newCard.GetComponent<Card>().CardID);
		}
	}

	private void Shuffle(List<GameObject> cards)
	{
		for (int i = 0; i < cards.Count; i++)
		{
			GameObject temp = cards[i];
			int randomIndex = Random.Range(i, cards.Count);
			cards[i] = cards[randomIndex];
			cards[randomIndex] = temp;
		}
	}

	/// <summary>
	/// Could not get a formula to correctly space the cards out.
	/// </summary>
	private void SetSpacingSize()
	{
		int value = numberOfRows > numberOfColumns ? numberOfRows : numberOfColumns;
		
		switch (value)
		{
			case 2:
				cardSpacing = cardSpacings[0];
				cardScale   = 0.45f;
				break;
			case 3:
				cardSpacing = cardSpacings[1];
				cardScale   = 0.45f;
				break;
			case 4:
				cardSpacing = cardSpacings[2];
				cardScale   = 0.38f;
				break;
			case 5:
				cardSpacing = cardSpacings[3];
				cardScale   = 0.32f;
				break;
			case 6:
				cardSpacing = cardSpacings[4];
				cardScale   = 0.28f;
				break;
		}
	}
}