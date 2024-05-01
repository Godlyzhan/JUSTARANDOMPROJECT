using UnityEngine;

public class Card : MonoBehaviour
{
    [field: SerializeField]
    public CardIdentifier.CardID CardID { get; private set; }
    public bool IsFlipped { get; set; }
    
    public void FlipCard()
    {
        if (!IsFlipped)
        {
            transform.Rotate(Vector3.up, 180f);
            IsFlipped = true;
            OnCardFlipped();
        }
        else
        {
            transform.Rotate(Vector3.up, -180f);
            IsFlipped = false;
        }
    }

    public void OnCardFlipped()
    {
    }

    public void MatchCard()
    {
    }
}
