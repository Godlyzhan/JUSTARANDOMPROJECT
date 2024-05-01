using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

public class SaveGameManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private string filePath;

    public UnityAction SaveDataReady;

    private void Start()
    {
        gameManager.SaveGame += SaveGame;
        filePath = Application.persistentDataPath + "/gameData.dat";
    }

    private void OnDestroy()
    {
        gameManager.SaveGame -= SaveGame;
    }

    public void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        GameData        data      = new GameData();

        data.score       = gameManager.Score;
        data.CardsInPlay = new List<SerializableKeyValuePair<CardIdentifier.CardID, Vector2>>(); // Use string as the key type

        foreach (var kvp in gameManager.cardsInPlay)
        {
            CardIdentifier.CardID keyName = kvp.Key; // Store the name of the GameObject
            Vector2        value   = kvp.Value;
            data.CardsInPlay.Add(new SerializableKeyValuePair<CardIdentifier.CardID, Vector2>(keyName, value));
        }

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(fileStream, data);
        }
    }


    public void LoadGame()
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                GameData data = (GameData)formatter.Deserialize(fileStream);
                gameManager.SetScore(data.score);

                var cardsInPlay = new Dictionary<CardIdentifier.CardID, Vector2>();

                foreach (var cardInPlay in data.CardsInPlay)
                {
                    if (cardInPlay.KeyName != null)
                    {
                        cardsInPlay.Add(cardInPlay.KeyName, cardInPlay.Value);
                    }
                    else
                    {
                        Debug.LogWarning("Failed to find GameObject with name: " + cardInPlay.KeyName);
                    }
                }

                gameManager.SetCardPositions(cardsInPlay);
            }

            SaveDataReady?.Invoke();
        }
    }
}

[Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    public CardIdentifier.CardID KeyName; // Store the name of the GameObject
    public TValue Value;

    public SerializableKeyValuePair(CardIdentifier.CardID keyName, TValue value)
    {
        KeyName = keyName;
        Value   = value;
    }
}


[Serializable]
public class GameData
{
    public int BestScore;
    public int score;
    public List<SerializableKeyValuePair<CardIdentifier.CardID, Vector2>> CardsInPlay;
}
