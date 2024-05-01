using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
	[field: SerializeField, Range(2, 5)] 
	public int Rows { get; private set; } = 2;

	[field: SerializeField, Range(2, 6)] 
	public int Columns { get; private set; } = 2;

	private GameManager gameManager => GameManager.Instance;

	private void Start()
	{
		var button = GetComponent<Button>();
		button.onClick.AddListener(StartGameMode);
	}

	private void StartGameMode()
	{
		gameManager.SelectGameMode(this);
	}
}
