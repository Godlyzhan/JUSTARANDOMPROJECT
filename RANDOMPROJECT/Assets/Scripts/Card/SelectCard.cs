using UnityEngine;

public class SelectCard : MonoBehaviour
{
	[SerializeField] private GameManager gameManager;

	void Update()
	{
		if (Input.GetMouseButtonUp(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			Ray ray;

#if UNITY_ANDROID && !UNITY_EDITOR
				Vector3 touchPosition = Input.GetTouch(0).position;
				ray = Camera.main.ScreenPointToRay(touchPosition);
#else
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif

			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

			if (hit.collider != null && hit.collider.TryGetComponent(out Card card))
			{
				if (!card.IsFlipped)
				{
					card.FlipCard();
					gameManager.SelectedCard(card);
				}
			}
		}
	}
}
