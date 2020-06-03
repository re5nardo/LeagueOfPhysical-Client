using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Entity;

public class DirectionInputController : MonoBehaviour
{
    [SerializeField] private DirectionKey directionKey = null;
    [SerializeField] private InputDisplayUI inputDisplayUI = null;

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
        return directionKey.GetPressedPosition();
    }

    #region Event Handler
    private void OnDirectionKeyPress(Vector2 vec2ScreenPosition)
    {
        if (!EntityManager.Instance.GetMyCharacter().IsAlive)
            return;

        inputDisplayUI.Show(Camera.main.ScreenToWorldPoint(vec2ScreenPosition));

        onPress?.Invoke(vec2ScreenPosition);
    }

    private void OnDirectionKeyRelease(Vector2 vec2ScreenPosition)
    {
        inputDisplayUI.Hide();

        onRelease?.Invoke(vec2ScreenPosition);
    }

    private void OnDirectionKeyHold(Vector2 vec2ScreenPosition)
    {
        Character character = EntityManager.Instance.GetMyCharacter();
        if (!character.IsAlive)
            return;

        //	임시 코드.. 나중에 클래스 프로퍼티 or State로 빼서 처리해야 함
        if (character.GetComponent<Behavior.BehaviorBase>() != null)
            return;

        inputDisplayUI.OnTouchHold(Camera.main.ScreenToWorldPoint(vec2ScreenPosition));

        onHold?.Invoke(vec2ScreenPosition);
    }
    #endregion
}
