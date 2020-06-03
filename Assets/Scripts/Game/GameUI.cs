using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private CameraController m_CameraController = null;
    [SerializeField] private EmotionExpressionSelectUI m_EmotionExpressionSelectUI = null;
    [SerializeField] private UISpriteButton m_ButtonEmotionExpression = null;
    [SerializeField] private FirstStatusSelectionUI m_FirstStatusSelectionUI = null;
    [SerializeField] private UISpriteButton m_ButtonFirstStatus = null;
    [SerializeField] private AbilitySelectionUI m_AbilitySelectionUI = null;
    [SerializeField] private PlayerInputUI m_PlayerInputUI = null;

    public CameraController             CameraController            { get { return m_CameraController; } }
    public EmotionExpressionSelectUI    EmotionExpressionSelectUI   { get { return m_EmotionExpressionSelectUI; } }
    public FirstStatusSelectionUI       FirstStatusSelectionUI      { get { return m_FirstStatusSelectionUI; } }
    public AbilitySelectionUI           AbilitySelectionUI          { get { return m_AbilitySelectionUI; } }
    public PlayerInputUI                PlayerInputUI               { get { return m_PlayerInputUI; } }

    public void Initialize()
    {
        m_ButtonEmotionExpression.onClicked = () =>
        {
            m_EmotionExpressionSelectUI.Show();
        };

        m_ButtonFirstStatus.onClicked = () =>
        {
            m_FirstStatusSelectionUI.Toggle();
        };
    }

    public void Clear()
    {
        m_ButtonEmotionExpression.onClicked = null;
        m_ButtonFirstStatus.onClicked = null;
    }

    public GameObject GetTopMostGameRoomPanel()
    {
        GameObject[] gameRoomPanels = GameObject.FindGameObjectsWithTag("GameRoomPanel");
        return gameRoomPanels[gameRoomPanels.Length - 1];
    }
}
