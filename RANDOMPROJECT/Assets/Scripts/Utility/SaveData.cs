using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
	public static SaveData current;
	public static bool CanContinue;
	public static int CurrentScore;

	public static List<GameModeScore> GameModeScores = new List<GameModeScore>();
	public static List<int> CardIdentifiers = new List<int>();
	public static List<Vector2> CardPositions = new List<Vector2>();

	public static void ReturnToDefaults()
	{
		CardIdentifiers.Clear();
		CardPositions.Clear();
		CanContinue  = false;
		CurrentScore = 0;
	}
}