using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmotionExpressionSelectButton : MonoBehaviour
{
	[SerializeField] private TextureButton textureButton = null;

	public event Action<int> onClicked = null;

    private int emotionExpressionID = -1;

    public void SetData(int nEmotionExpressionID)
    {
        emotionExpressionID = nEmotionExpressionID;

        MasterData.EmotionExpression master = MasterDataManager.Instance.GetMasterData<MasterData.EmotionExpression>(nEmotionExpressionID);

        textureButton.texture.texture = Resources.Load<Texture>(master.ResID);
    }

    public int GetEmotionExpressionID()
    {
        return emotionExpressionID;
    }

	private void Awake()
	{
        textureButton.button.onClick.AddListener(OnUITextureButtonClicked);
	}

	private void OnDestroy()
	{
        textureButton.button.onClick.RemoveListener(OnUITextureButtonClicked);
	}

	private void OnUITextureButtonClicked()
	{
		onClicked?.Invoke(emotionExpressionID);
	}
}
