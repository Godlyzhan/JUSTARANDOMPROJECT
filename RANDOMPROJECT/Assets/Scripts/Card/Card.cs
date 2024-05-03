using UnityEngine;

public class Card : MonoBehaviour
{
	[SerializeField] private float flipDuration = 0.7f;
	[SerializeField] private Color matchColor;
	[SerializeField] private Color mismatchColor;
	[SerializeField] private SpriteRenderer spriteRenderer;

	private bool flip;
	private float flipAngle;
	private float flipTimer;
	private Color defaultColor;

	[field: SerializeField] public CardIdentifier.CardID CardID { get; private set; }

	public bool IsFlipped { get; set; }

	private void Start()
	{
		defaultColor = spriteRenderer.color;
	}

	private void Update()
	{
		if (flip)
		{
			flipTimer += Time.deltaTime;
			float time = Mathf.Clamp01(flipTimer / flipDuration);
			RotateObject(time, flipAngle);
			
			if (flipTimer >= flipDuration)
			{
				flip = false;
				flipTimer = 0f;
			}
		}
	}

	public void FlipCard()
	{
		if (!IsFlipped)
		{
			IsFlipped = true;
			OnCardFlipped(180);
		}
		else
		{
			IsFlipped = false;
			OnCardFlipped(0);
			spriteRenderer.color = defaultColor;
		}
	}

	public void OnCardFlipped(float rotation)
	{
		flipAngle = rotation;
		flip      = true;
	}

	public void MatchCard()
	{
		spriteRenderer.color = matchColor;
	}

	public void MismatchCard()
	{
		spriteRenderer.color = mismatchColor;
	}

	private void RotateObject(float time, float angle)
	{
		Quaternion startRotation = transform.rotation;
		Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);
		transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time);
	}
}
