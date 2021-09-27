using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private CameraController cameraController = null;
    [SerializeField] private EmotionExpressionSelector emotionExpressionSelector = null;
    [SerializeField] private TextureButton emotionExpressionButton = null;
    [SerializeField] private PlayerInputController playerInputController = null;

    public CameraController CameraController => cameraController;
    public EmotionExpressionSelector EmotionExpressionSelector => emotionExpressionSelector;
    public PlayerInputController PlayerInputController => playerInputController;

    public Canvas TopMostCanvas => ManagedCanvasManager.Instance.GetTopMost(CanvasLayer.Contents).Canvas;
    public Canvas HealthBarCanvas => (ManagedCanvasManager.Instance.Get(CanvasLayer.Contents, "Canvas - 100") ?? ManagedCanvasManager.Instance.GetTopMost(CanvasLayer.Contents)).Canvas;
    public Canvas FloatingItemCanvas => (ManagedCanvasManager.Instance.Get(CanvasLayer.Contents, "Canvas - 400") ?? ManagedCanvasManager.Instance.GetTopMost(CanvasLayer.Contents)).Canvas;

    public void Initialize()
    {
        emotionExpressionButton.button.onClick.AddListener(() =>
        {
            EmotionExpressionSelector.Show();
        });
    }

    public void Clear()
    {
        emotionExpressionButton.button.onClick.RemoveAllListeners();
    }
}
