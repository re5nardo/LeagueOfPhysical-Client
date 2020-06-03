using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilitySelectionItemUI : MonoBehaviour
{
	[SerializeField] private UITextureButton uiTextureButton = null;

	public event Action<int> onClicked = null;

	private int abilityID = -1;

	public void SetAbility(int abilityID)
	{
		this.abilityID = abilityID;

		MasterData.Ability master = MasterDataManager.Instance.GetMasterData<MasterData.Ability>(abilityID);

		uiTextureButton.textureImage.texture = Resources.Load<Texture>(master.IconResID);
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
		onClicked?.Invoke(abilityID);
	}
}
