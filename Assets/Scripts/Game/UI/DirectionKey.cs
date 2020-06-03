using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DirectionKey : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	[HideInInspector] public Vector2Handler onHold = null;
	[HideInInspector] public Vector2Handler onPress = null;
	[HideInInspector] public Vector2Handler onRelease = null;

    private Vector2 m_vec2PressedPosition;  //  Screen Coordinates

    private bool m_bIsTouching = false;

    public Vector2 GetPressedPosition()
    {
        return m_vec2PressedPosition;
    }

    public bool IsTouching()
    {
        return m_bIsTouching;
    }

    #region Event Handler
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Mouse Down: " + eventData.pointerCurrentRaycast.gameObject.name);

        m_bIsTouching = true;

        m_vec2PressedPosition = eventData.pointerCurrentRaycast.worldPosition;

        onPress?.Invoke(m_vec2PressedPosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Mouse Up");

        m_bIsTouching = false;

        onRelease?.Invoke(eventData.pointerCurrentRaycast.worldPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");

        onHold?.Invoke(eventData.pointerCurrentRaycast.worldPosition);
    }
    #endregion
}