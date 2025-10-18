using UnityEngine;
using UnityEngine.EventSystems;

public class CircleData
{
    public float positionX;
    public float positionY;
}

public class DragCircle : MonoBehaviour, IDragHandler
{
    RectTransform rectTransform;
    Canvas canvas;

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = eventData.position / canvas.scaleFactor;
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    internal CircleData GetData()
        => new() { positionX = rectTransform.anchoredPosition.x, positionY = rectTransform.anchoredPosition.y };

    internal void SetData(CircleData circleData)
    {
        rectTransform.anchoredPosition = new Vector2(circleData.positionX, circleData.positionY);
    }
}
