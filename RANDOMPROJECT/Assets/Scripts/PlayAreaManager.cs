using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayAreaManager : MonoBehaviour
{
	[Header("Card Grid")]
	[SerializeField, Range(2, 5)]
	private int numberOfRows = 4;
	[SerializeField, Range(2, 6)]
	private int numberOfColumns = 4;

	[Header("Spacing between cards")]
	[SerializeField, Range(0.2f, 5f)]
	private List<float> cardSpacings = new List<float>();
	
	// TODO: Make an play area and scale the object with screen size and use that size
	[SerializeField] 
	private Transform playAea;
	[SerializeField]
	private Vector2 playAreaSize = new Vector2(5f, 5f);

	[SerializeField]
	private CardDeck cardDeck;
	
	private float cardSpacing;
	private float cardScale;

	private List<GameObject> cards = new List<GameObject>();
	private List<Vector2> cardPositions = new List<Vector2>();
	
	public void InstantiateCards()
	{
		if (cards.Count > 0)
		{
			foreach (GameObject card in cards)
			{
				DestroyImmediate(card);
			}
			cards.Clear();
		}

		cardPositions.Clear();
		GenerateGrid();
	}

	void GenerateGrid()
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

		float minX = position.x - playAreaSize.x / 2;
		float maxX = position.x + playAreaSize.x / 2;
		float minY = position.y - playAreaSize.y / 2;
		float maxY = position.y + playAreaSize.y / 2;

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

	void SpawnCards()
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

		for (int i = 0; i < Mathf.Min(cardPositions.Count, duplicatedCards.Count); i++)
		{
			GameObject newCard = Instantiate(duplicatedCards[i], cardPositions[i], Quaternion.identity);
			newCard.transform.SetParent(transform);
			newCard.transform.localScale *= cardScale;

			cards.Add(newCard);
		}
	}

	void Shuffle(List<GameObject> cards)
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