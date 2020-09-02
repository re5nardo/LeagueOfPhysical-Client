using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Entity;

public class DirectionInputController : MonoBehaviour
{
    [SerializeField] private DirectionKey directionKey = null;
    [SerializeField] private InputCircleDisplay inputCircleDisplay = null;

    public event Action<Vector2> onPress;
    public event Action<Vector2> onRelease;
    public event Action<Vector2> onHold;

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

    public Vector2 GetPressedPosition()
    {
        return directionKey.PressedPosition;
    }

    #region Event Handler
    private void OnDirectionKeyPress(Vector2 vec2ScreenPosition)
    {
        if (!Entities.MyCharacter.IsAlive)
            return;

        inputCircleDisplay.Show(vec2ScreenPosition);

        onPress?.Invoke(vec2ScreenPosition);
    }

    private void OnDirectionKeyRelease(Vector2 vec2ScreenPosition)
    {
        inputCircleDisplay.Hide();

        onRelease?.Invoke(vec2ScreenPosition);
    }

    private void OnDirectionKeyHold(Vector2 vec2ScreenPosition)
    {
        if (!Entities.MyCharacter.IsAlive)
            return;

        inputCircleDisplay.OnTouchHold(vec2ScreenPosition);

        onHold?.Invoke(vec2ScreenPosition);
    }
    #endregion
}
