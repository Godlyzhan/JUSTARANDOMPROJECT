using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // fields
    [SerializeField] private PlayAreaManager playAreaManager;
    [SerializeField] private UnityEvent endGameEvent;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameUIState gameUIState;
    [SerializeField] private DisplayGameStats displayGameStats;
    [SerializeField] private SoundEffectsManager soundEffectsManager;

    public Action<int> OnScoreChange;
    private List<Card> selectedCards = new List<Card>();

    public int MatchedCards { get; private set; }
    
    public int Score
    {
        get => SaveData.CurrentScore;
        set
        {
            SaveData.CurrentScore = value;
        }
    }
    public bool CanContinue
    {
        get => SaveData.CanContinue;
        set
        {
            continueButton.SetActive(value);
            SaveData.CanContinue = value;
        }
    }

    private async void Start()
    {
        await SaveLoad.LoadAsync();
        CanContinue = SaveData.CanContinue;
    }

    public async void ContinueGame()
    {
        await SaveLoad.LoadAsync();

        if (CanContinue)
        {
            selectedCards.Clear();
            await Task.Delay(TimeSpan.FromSeconds(1f));
            playAreaManager.LoadCardsInPlay();
        }
    }

    public void PlayGameMode(GameMode gameMode)
    {
        Score = 0;
        SaveData.ReturnToDefaults();
        playAreaManager.SelectGameMode(gameMode);
        displayGameStats.UpdateScore(Score);
        displayGameStats.UpdateGameMode(playAreaManager.GameMode);
        displayGameStats.UpdateBestScore(playAreaManager.BestScore);
    }

    public void EndGame()
    {
        bool gameDataExists = false;

        if (SaveData.GameModeScores.Count != 0)
        {
            foreach (var gameModeScore in SaveData.GameModeScores)
            {
                if (gameModeScore.GameMode == playAreaManager.GameMode && gameModeScore.BestScore > Score)
                {
                    gameModeScore.BestScore = Score;
                    gameDataExists          = true;
                    break;
                }
            }
        }

        if (!gameDataExists)
        {
            GameModeScore gameModeScore = new GameModeScore
            {
                GameMode  = playAreaManager.GameMode,
                BestScore = Score
            };
            SaveData.GameModeScores.Add(gameModeScore);
        }

        endGameEvent?.Invoke();
    }

    public void ClearCards()
    {
        playAreaManager.RemoveCardInPlay();
    }

    public void SelectedCard(Card selectedCard)
    {
        selectedCards.Add(selectedCard);
        soundEffectsManager.CardFlipSfx();

        if (selectedCards.Count == 2)
        {
            Score++;
            OnScoreChange?.Invoke(Score);
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
            
            MatchedCards++;
            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            soundEffectsManager.CardMatchedSfx();
            if (firstCard != null)
            {
                playAreaManager.RemoveCardFromPlay(firstCard.gameObject, firstCard.CardID);
            }

            if (secondCard != null)
            {
                playAreaManager.RemoveCardFromPlay(secondCard.gameObject, secondCard.CardID);
            }

            await SaveLoad.SaveAsync();
        }
        else
        {
            firstCard.MismatchCard();
            secondCard.MismatchCard();

            await Task.Delay(TimeSpan.FromSeconds(0.4f));

            soundEffectsManager.CardMismatchSfx();
            firstCard.FlipCard();
            secondCard.FlipCard();
        }

        SaveData.CurrentScore = Score;
    }

    public void BackToMenu()
    {
        playAreaManager.BuildCardIndex();
        SaveGameProgress();
        gameUIState.SetMenuState();
    }

    /*private void OnApplicationQuit()
    {
        if (CanContinue)
        {
            SaveGameProgress();
        }
    }*/

    private async void SaveGameProgress()
    {
        await SaveLoad.SaveAsync();
    }
}
