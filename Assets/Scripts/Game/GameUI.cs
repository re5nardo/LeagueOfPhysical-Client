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
        return ManagedCanvasManager.Instance.GetTopMost(CanvasLayer.Contents).Canvas;
    }

    public Canvas GetHealthBarCanvas()
    {
        var target = ManagedCanvasManager.Instance.Get(CanvasLayer.Contents, "Canvas - 100") ?? ManagedCanvasManager.Instance.GetTopMost(CanvasLayer.Contents);

        return target.Canvas;
    }

    public Canvas GetFloatingItemCanvas()
    {
        var target = ManagedCanvasManager.Instance.Get(CanvasLayer.Contents, "Canvas - 400") ?? ManagedCanvasManager.Instance.GetTopMost(CanvasLayer.Contents);

        return target.Canvas;
    }
}
