using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private CameraController m_CameraController = null;
    [SerializeField] private EmotionExpressionSelector m_EmotionExpressionSelector = null;
    [SerializeField] private TextureButton m_ButtonEmotionExpression = null;
    [SerializeField] private PlayerInputController m_PlayerInputController = null;

    public CameraController             CameraController            { get { return m_CameraController; } }
    public EmotionExpressionSelector    EmotionExpressionSelector   { get { return m_EmotionExpressionSelector; } }
    public PlayerInputController        PlayerInputController       { get { return m_PlayerInputController; } }

    public void Initialize()
    {
        m_ButtonEmotionExpression.button.onClick.AddListener(() =>
        {
            EmotionExpressionSelector.Show();
        });
    }

    public void Clear()
    {
        m_ButtonEmotionExpression.button.onClick.RemoveAllListeners();
    }

    public Canvas GetTopMostCanvas()
    {
        var canvases = GameObject.FindObjectsOfType<Canvas>();

        return canvases[canvases.Length - 1];
    }
}
