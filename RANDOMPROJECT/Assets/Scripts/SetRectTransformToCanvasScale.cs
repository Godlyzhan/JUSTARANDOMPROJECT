using UnityEngine;

public class SetRectTransformToCanvasScale : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private float paddingPercentX = 100f;
    [SerializeField] private float paddingPercentY = 100f;

    void Start()
    { 
        rectTransform           = GetComponent<RectTransform>();
        var sizeX = canvasRectTransform.sizeDelta.x * (paddingPercentX/100);
        var sizeY = canvasRectTransform.sizeDelta.y * (paddingPercentY/100);
        rectTransform.sizeDelta = new Vector2(sizeX,sizeY);
    }
}
