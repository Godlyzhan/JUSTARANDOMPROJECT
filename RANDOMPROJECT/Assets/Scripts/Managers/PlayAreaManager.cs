using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayAreaManager : MonoBehaviour
{
	[SerializeField] private GameManager gameManager;
	[SerializeField] private CardDeck cardDeck;
	[SerializeField] private RectTransform playArea;
	[SerializeField] private GameObject cardReference;

	private int numberOfRows = 4;
	private int numberOfColumns = 4;
	private float cardSpacing;
	private Vector3 cardScale;

	private SpriteRenderer spriteRenderer;
	private List<GameObject> cards = new List<GameObject>();
	private List<Vector2> cardPositions = new List<Vector2>();
	private List<CardIdentifier.CardID> cardIds = new List<CardIdentifier.CardID>();

	private Vector2 playerAreaScale;

	public string GameMode { get; private set; }
	public int BestScore { get; set; }

	private void Start()
	{
		playerAreaScale = playArea.sizeDelta;
		spriteRenderer  = cardReference.GetComponentInChildren<SpriteRenderer>();

		if (spriteRenderer == null)
		{
			Debug.LogError($"Could not get sprite render for card prefab : {cardReference}");
		}
	}

	private void InstantiateCards()
	{
		RemoveCardInPlay();
		SetSpacingSize();
		GenerateGrid();
		SpawnCards();
	}

	public void SelectGameMode(GameMode gameMode)
	{
		numberOfColumns           = gameMode.Columns;
		numberOfRows              = gameMode.Rows;
		GameMode                  = $"{numberOfColumns}x{numberOfRows}";
		SaveLoad.SaveData.Columns = numberOfColumns;
		SaveLoad.SaveData.Rows = numberOfRows;

		if (SaveLoad.SaveData.GameModeScores.Count > 0)
		{
			foreach (var gameModeScore in SaveLoad.SaveData.GameModeScores)
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
		numberOfColumns = SaveLoad.SaveData.Columns;
		numberOfRows    = SaveLoad.SaveData.Rows;
		GameMode        = $"{numberOfColumns}x{numberOfRows}";
		gameManager.DisplayGameStats.UpdateGameMode(GameMode);
		SetSpacingSize();
		GenerateGrid();

		for (int i = 0; i < SaveLoad.SaveData.CardIdentifiers.Count; i++)
		{
			for (int j = 0; j < cardDeck.CardIdentifier.Count; j++)
			{
				if (SaveLoad.SaveData.CardIdentifiers[i] == (int)cardDeck.CardIdentifier[j].CardID)
				{
					Vector2  cardPosition= SaveLoad.SaveData.CardPositions[i];
					GameObject newCard = Instantiate(cardDeck.Cards[j], cardPosition, Quaternion.identity);
					newCard.transform.SetParent(transform);
					newCard.transform.localScale = cardScale;
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
			SaveLoad.SaveData.CardIdentifiers.Clear();
			SaveLoad.SaveData.CardPositions.Clear();
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

			SaveLoad.SaveData.CardIdentifiers.Add((int)id);
			SaveLoad.SaveData.CardPositions.Add(cardPosition);
		}
		gameManager.CanContinue = true;
	}

	private void GenerateGrid()
	{
		float cardWidth  = cardScale.x + cardSpacing;
		float cardHeight = cardScale.y + cardSpacing;
		
		float gridWidth  = numberOfColumns * cardWidth;
		float gridHeight = numberOfRows * cardHeight;

		Vector3 position = playArea.position;

		float startX = position.x - (gridWidth / 2) + (cardSpacing / 2);
		float startY = position.y + (gridHeight / 2) - (cardSpacing/ 2);

		float minX = position.x - playerAreaScale.x / 2;
		float maxX = position.x + playerAreaScale.x / 2;
		float minY = position.y - playerAreaScale.y / 2;
		float maxY = position.y + playerAreaScale.y / 2;

		for (int row = 0; row < numberOfRows; row++)
		{
			for (int column = 0; column < numberOfColumns; column++)
			{
				float posX = startX + (column * (cardScale.x + cardSpacing));
				float posY = startY - row * (cardScale.y + cardSpacing);

				posX = Mathf.Clamp(posX, minX, maxX);
				posY = Mathf.Clamp(posY, minY, maxY);

				Vector2 cardPos = new Vector2(posX, posY);
				cardPositions.Add(cardPos);
			}
		}
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
			newCard.transform.localScale = cardScale;

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
		var spriteSize= spriteRenderer.bounds.size;

		Vector3[] localCorners = new Vector3[4];
		playArea.GetLocalCorners(localCorners);

		// Convert the local corners to world space
		Vector3[] worldCorners = new Vector3[4];
		for (int i = 0; i < 4; i++)
		{
			worldCorners[i] = playArea.TransformPoint(localCorners[i]);
		}

		// Calculate the size of the RectTransform in world space
		Vector3 sizeInWorldSpace = worldCorners[2] - worldCorners[0];

		float     desiredSize  = sizeInWorldSpace.y / (numberOfRows + 1);
		Vector3 cardScaleFactor  = new Vector3(desiredSize / spriteSize.x, desiredSize / spriteSize.y, 1f);
		
		cardScale   = cardScaleFactor;
		cardSpacing = desiredSize;
	}
}