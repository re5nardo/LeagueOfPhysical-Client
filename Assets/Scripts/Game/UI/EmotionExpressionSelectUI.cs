using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionExpressionSelectUI : MonoBehaviour
{
    [SerializeField] private EmotionExpressionSelectButton m_Button_Top = null;
    [SerializeField] private EmotionExpressionSelectButton m_Button_Right = null;
    [SerializeField] private EmotionExpressionSelectButton m_Button_Bottom = null;
    [SerializeField] private EmotionExpressionSelectButton m_Button_Left = null;
    [SerializeField] private UISpriteButton m_Button_Close = null;

    private void Awake()
    {
        m_Button_Top.onClicked += ButtonClickHandler;
        m_Button_Right.onClicked += ButtonClickHandler;
        m_Button_Bottom.onClicked += ButtonClickHandler;
        m_Button_Left.onClicked += ButtonClickHandler;
        m_Button_Close.onClicked = () =>
        {
            Hide();
        };
    }

    private void OnDestroy()
    {
        m_Button_Top.onClicked -= ButtonClickHandler;
        m_Button_Right.onClicked -= ButtonClickHandler;
        m_Button_Bottom.onClicked -= ButtonClickHandler;
        m_Button_Left.onClicked -= ButtonClickHandler;
        m_Button_Close.onClicked = null;
    }

    private void ButtonClickHandler(int nEmotionExpressionID)
    {
		RoomNetwork.Instance.Send(new CS_RequestEmotionExpression(nEmotionExpressionID), PhotonNetwork.masterClient.ID);

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

    public void SetData(int nTop, int nRight, int nBottom, int nLeft)
    {
        m_Button_Top.SetData(nTop);
        m_Button_Right.SetData(nRight);
        m_Button_Bottom.SetData(nBottom);
        m_Button_Left.SetData(nLeft);
    }
}
