using UnityEngine;
using UnityEngine.UI;

public class ClearCardsInPlay : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(gameManager.ClearCards);
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            button.onClick.RemoveListener(gameManager.ClearCards);
        }
    }
}
