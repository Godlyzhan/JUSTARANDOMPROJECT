using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayGameStats : MonoBehaviour
{
	[SerializeField]
	private List<TextMeshProUGUI> scoreTexts = new List<TextMeshProUGUI>();
	[SerializeField]
	private List<TextMeshProUGUI> gameModeTexts = new List<TextMeshProUGUI>();
	[SerializeField]
	private List<TextMeshProUGUI> bestScoreTexts = new List<TextMeshProUGUI>();

	[SerializeField]
	private GameManager gameManager;

	private void OnEnable()
	{
		UpdateScore(gameManager.Score);
		gameManager.OnScoreChange += UpdateScore;
	}

	private void OnDisable()
	{
		gameManager.OnScoreChange -= UpdateScore;
	}

	public void UpdateGameMode(string gameMode)
	{
		foreach (var gameModeText in gameModeTexts)
		{
			gameModeText.text = gameMode;
		}
	}

	public void UpdateBestScore(int score)
	{
		foreach (var bestScoreText in bestScoreTexts)
		{
			bestScoreText.text = score.ToString();
		}
	}

	public void UpdateScore(int score)
	{
		foreach (var scoreText in scoreTexts)
		{
			scoreText.text = score.ToString();
		}
	}
	
}
