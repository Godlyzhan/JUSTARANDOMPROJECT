using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SaveGameManager saveGameManager;
    [SerializeField] private PlayAreaManager playAreaManager;
    [SerializeField] private int moves;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject continueButton;

    private List<Card> selectedCards = new List<Card>();

    public Action SaveGame;

    public int Score { get; set; }
    public Dictionary<GameObject,Vector2> cardsInPlay;

    public void SetScore(int score)
    {
        Score = score;
    }

    public void SetCardPositions(Dictionary<GameObject,Vector2> cardsInPlay)
    {
        this.cardsInPlay = cardsInPlay;
    }

    public void SelectGameMode(GameMode gameMode)
    {
        if (saveGameManager.LoadGame())
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }

        playAreaManager.SelectGameMode(gameMode);
    }

    public void SelectedCard(Card selectedCard)
    {
        selectedCards.Add(selectedCard);

        if (selectedCards.Count == 2)
        {
            moves++;
            text.text = moves.ToString();
            MatchCards();
        }
    }

    private async void MatchCards()
    {
        Card firstCard = selectedCards[0];
        Card secondCard = selectedCards[1];
        selectedCards.Clear();

        if (firstCard.CardID == secondCard.CardID)
        {
            firstCard.MatchCard();
            secondCard.MatchCard();

            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            Destroy(firstCard.gameObject);
            Destroy(secondCard.gameObject);
        }
        else
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            firstCard.FlipCard();
            secondCard.FlipCard();
        }

        SaveGame?.Invoke();
    }
}
