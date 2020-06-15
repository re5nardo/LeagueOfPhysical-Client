using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DirectionKey : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	[HideInInspector] public Vector2Handler onHold = null;
	[HideInInspector] public Vector2Handler onPress = null;
	[HideInInspector] public Vector2Handler onRelease = null;

    private Vector2 pressedPosition; 
    public Vector2 PressedPosition { get { return pressedPosition; } }

    private Vector2 lastPosition;

    #region Event Handler
    public void OnPointerDown(PointerEventData eventData)
    {
        pressedPosition = lastPosition = eventData.position;

        onPress?.Invoke(eventData.position);

        StartCoroutine("ProcessHold");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onRelease?.Invoke(eventData.position);

        StopCoroutine("ProcessHold");
    }

    public void OnDrag(PointerEventData eventData)
    {
        lastPosition = eventData.position;
    }
    #endregion

    private IEnumerator ProcessHold()
    {
        while (true)
        {
            yield return null;

            onHold?.Invoke(lastPosition);
        }
    }
}
