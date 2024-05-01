using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public void BackToMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	public void LoadScene()
	{
		SceneManager.LoadScene("Gameplay");
	}

	public void Quit()
	{
		Application.Quit();
	}
}
