using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkModel.Mirror;
using GameFramework;

public class EmotionExpressionSelector : MonoBehaviour
{
    [SerializeField] private EmotionExpressionSelectButton button_Top = null;
    [SerializeField] private EmotionExpressionSelectButton button_Right = null;
    [SerializeField] private EmotionExpressionSelectButton button_Bottom = null;
    [SerializeField] private EmotionExpressionSelectButton button_Left = null;
    [SerializeField] private TextureButton button_Close = null;

    private void Awake()
    {
        button_Top.onClicked += ButtonClickHandler;
        button_Right.onClicked += ButtonClickHandler;
        button_Bottom.onClicked += ButtonClickHandler;
        button_Left.onClicked += ButtonClickHandler;
        button_Close.button.onClick.AddListener(Hide);
    }

    private void OnDestroy()
    {
        button_Top.onClicked -= ButtonClickHandler;
        button_Right.onClicked -= ButtonClickHandler;
        button_Bottom.onClicked -= ButtonClickHandler;
        button_Left.onClicked -= ButtonClickHandler;
        button_Close.button.onClick.RemoveListener(Hide);
    }

    private void ButtonClickHandler(int emotionExpressionID)
    {
        using var disposer = PoolObjectDisposer<CS_RequestEmotionExpression>.Get();
        var requestEmotionExpression = disposer.PoolObject;
        requestEmotionExpression.entityId = Entities.MyEntityID;
        requestEmotionExpression.emotionExpressionId = emotionExpressionID;

        RoomNetwork.Instance.Send(requestEmotionExpression, 0);

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetData(int top, int right, int bottom, int left)
    {
        button_Top.SetData(top);
        button_Right.SetData(right);
        button_Bottom.SetData(bottom);
        button_Left.SetData(left);
    }
}
