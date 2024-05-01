using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
	private Text text;
	private GameManager gameManager => GameManager.Instance;
	private void Start()
	{
		text = GetComponent<Text>();
		text.text = "Score: " + gameManager.Score.ToString();
	}
}
