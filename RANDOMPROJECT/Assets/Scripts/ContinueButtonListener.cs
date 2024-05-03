using System;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonListener : MonoBehaviour
{
	[SerializeField]
	private GameManager gameManager;

	private Button button;
	private void Start()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(gameManager.ContinueGame);
	}

	private void OnDestroy()
	{
		button.onClick.RemoveListener(gameManager.ContinueGame);
	}
}
