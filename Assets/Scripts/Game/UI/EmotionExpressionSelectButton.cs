using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmotionExpressionSelectButton : MonoBehaviour
{
	[SerializeField] private UITextureButton uiTextureButton = null;

	public event Action<int> onClicked = null;

    private int m_nEmotionExpressionID = -1;

    public void SetData(int nEmotionExpressionID)
    {
        m_nEmotionExpressionID = nEmotionExpressionID;

        MasterData.EmotionExpression master = MasterDataManager.Instance.GetMasterData<MasterData.EmotionExpression>(nEmotionExpressionID);

		uiTextureButton.textureImage.texture = Resources.Load<Texture>(master.ResID);
    }

    public int GetEmotionExpressionID()
    {
        return m_nEmotionExpressionID;
    }

	private void Awake()
	{
		uiTextureButton.onClicked += OnUITextureButtonClicked;
	}

	private void OnDestroy()
	{
		uiTextureButton.onClicked -= OnUITextureButtonClicked;
	}

	private void OnUITextureButtonClicked()
	{
		onClicked?.Invoke(m_nEmotionExpressionID);
	}
}
