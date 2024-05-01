using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : PersistentSingleton<GameManager>
{
    public Dictionary<CardIdentifier.CardID,Vector2> cardsInPlay = new Dictionary<CardIdentifier.CardID, Vector2>();

    [SerializeField] private PlayAreaManager playAreaManager;
    [SerializeField] private int moves;

    [SerializeField] private UnityEvent EndGameEvent;

    private List<Card> selectedCards = new List<Card>();

    public Action SaveGame;

    [field:SerializeField] 
    public SaveGameManager SaveGameManager { get; private set; }

    public int Score { get; set; }

    public Dictionary<CardIdentifier.CardID, Vector2> GameCardStates()
    {
        return playAreaManager.CardsInPlay();
    }

    public void ContinueGame()
    {
        cardsInPlay.Clear();
        SaveGameManager.SaveDataReady += LoadGame;
        SaveGameManager.LoadGame();
    }

    private void LoadGame()
    {
        SaveGameManager.SaveDataReady -= LoadGame;
        playAreaManager.UsePreviousGameGrid(cardsInPlay);
    }

    public void EndGame()
    {
        EndGameEvent?.Invoke();
    }

    public void SetScore(int score)
    {
        Debug.LogError(score);
        Score = score;
    }

    public void SetCardPositions(Dictionary<CardIdentifier.CardID,Vector2> cardsInPlay)
    {
        this.cardsInPlay = cardsInPlay;
        foreach (var cardInPlay in cardsInPlay)
        {
            Debug.LogError(cardInPlay.Key);
            Debug.LogError(cardInPlay.Value);
        }
    }

    public void SelectGameMode(GameMode gameMode)
    {
        playAreaManager.SelectGameMode(gameMode);
    }

    public void SelectedCard(Card selectedCard)
    {
        selectedCards.Add(selectedCard);

        if (selectedCards.Count == 2)
        {
            moves++;
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

            playAreaManager.RemoveCardFromPlay(firstCard.gameObject);
            playAreaManager.RemoveCardFromPlay(secondCard.gameObject);
            Score++;
            SaveGame?.Invoke();
        }
        else
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            firstCard.FlipCard();
            secondCard.FlipCard();
        }
    }
}
