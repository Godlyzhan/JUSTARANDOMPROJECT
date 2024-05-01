using System;
using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] 
    private float flipSpeed = 0.5f;

    [field: SerializeField]
    public CardIdentifier.CardID CardID { get; private set; }

    public bool IsFlipped { get; set; }

    private float flipDuration = 0.5f;
    private bool flip;

    private void Update()
    {

    }

    public void FlipCard()
    {
        if (!IsFlipped)
        {
            //transform.Rotate(Vector3.up, 180f);
            IsFlipped = true;
            flip      = true;
            OnCardFlipped(180);
        }
        else
        {
            IsFlipped = false;
            OnCardFlipped(0);
        }
    }

    public void OnCardFlipped(float rotation)
    {
        StartCoroutine(RotateObjectCoroutine(1.0f, rotation));
    }

    public void MatchCard()
    {
    }
    
    private IEnumerator RotateObjectCoroutine(float duration, float angle)
    {
        flip = true;
        float      elapsedTime    = 0f;
        Quaternion startRotation  = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.rotation =  Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime        += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; 
        flip               = false;
    }
}
