using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
	public bool CanContinue;
	public int CurrentScore;
	public int Columns;
	public int Rows;

	public List<GameModeScore> GameModeScores = new List<GameModeScore>();
	public List<int> CardIdentifiers = new List<int>();
	public List<SerializableVector2> CardPositions = new List<SerializableVector2>();

	public void ReturnToDefaults()
	{
		CardIdentifiers.Clear();
		CardPositions.Clear();
		CurrentScore = 0;
	}

}