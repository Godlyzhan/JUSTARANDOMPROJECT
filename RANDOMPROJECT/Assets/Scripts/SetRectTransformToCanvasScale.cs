using UnityEngine;

public class SetRectTransformToCanvasScale : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private float paddingPercent = 100f;

    void Start()
    { 
        rectTransform           = GetComponent<RectTransform>();
        var size = canvasRectTransform.sizeDelta * (paddingPercent/100);
        rectTransform.sizeDelta = size;
    }
}
