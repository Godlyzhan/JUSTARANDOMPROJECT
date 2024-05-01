using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] 
	private PlayAreaManager playAreaManager;

	private Card firstCard;
	private Card secondCard;

	private void Start()
	{
		playAreaManager.InstantiateCards();
	}

	public void SelectedCard(Card selectedCard)
	{
		if (firstCard == null)
		{
			firstCard = selectedCard;
		}
		else
		{
			secondCard = selectedCard;
			StartCoroutine(MatchCardsAsync());
		}
	}

	private IEnumerator MatchCardsAsync()
	{
		if (firstCard.CardID == secondCard.CardID)
		{
			firstCard.MatchCard();
			secondCard.MatchCard();
            
			yield return new WaitForSeconds(1f);
			Destroy(firstCard.gameObject);
			Destroy(secondCard.gameObject);
		}
		else
		{
			yield return new WaitForSeconds(1f);
			firstCard.FlipCard();
			secondCard.FlipCard();
		}
        
		firstCard  = null;
		secondCard = null;
	}
}
