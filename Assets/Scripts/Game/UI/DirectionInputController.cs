using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DirectionInputController : MonoBehaviour
{
    [SerializeField] private DirectionKey directionKey = null;
    [SerializeField] private InputCircleDisplay inputCircleDisplay = null;

    public event Action<Vector2> onPress;
    public event Action<Vector2> onRelease;
    public event Action<Vector2> onHold;

    public Vector2 PressedPosition => directionKey.PressedPosition;
    public float NormalizedPower { get; private set; }
    public Vector2 Direction { get; private set; }

    private void Awake()
    {
        directionKey.onPress += OnDirectionKeyPress;
        directionKey.onRelease += OnDirectionKeyRelease;
        directionKey.onHold += OnDirectionKeyHold;
    }

    private void OnDestroy()
    {
        directionKey.onPress -= OnDirectionKeyPress;
        directionKey.onRelease -= OnDirectionKeyRelease;
        directionKey.onHold -= OnDirectionKeyHold;
    }

    #region Event Handler
    private void OnDirectionKeyPress(Vector2 vec2ScreenPosition)
    {
        inputCircleDisplay.Show(vec2ScreenPosition);

        onPress?.Invoke(vec2ScreenPosition);
    }

    private void OnDirectionKeyRelease(Vector2 screenPosition)
    {
        Direction = (screenPosition - directionKey.PressedPosition).normalized;

        var normalizedPower = (screenPosition - directionKey.PressedPosition).magnitude / (inputCircleDisplay.MaxRadius * GetComponentInParent<Canvas>().GetComponent<RectTransform>().localScale.x);
        NormalizedPower = Mathf.Min(normalizedPower, 1);

        inputCircleDisplay.Hide();

        onRelease?.Invoke(screenPosition);
    }

    private void OnDirectionKeyHold(Vector2 vec2ScreenPosition)
    {
        inputCircleDisplay.OnTouchHold(vec2ScreenPosition);

        onHold?.Invoke(vec2ScreenPosition);
    }
    #endregion
}
