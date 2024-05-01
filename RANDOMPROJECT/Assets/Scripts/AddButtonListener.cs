using UnityEngine;
using UnityEngine.UI;

public class AddButtonListener : MonoBehaviour
{
	private GameManager gameManager => GameManager.Instance;

	private void Start()
	{
		var button = GetComponent<Button>();
		button.onClick.AddListener(gameManager.ContinueGame);
	}
}
