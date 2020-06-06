﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using UnityEngine.UI;

public class FirstStatusSelectionItemUI : MonoBehaviour
{
	[SerializeField] private Text statusNameLabel = null;
	[SerializeField] private Text statusValueLabel = null;
	[SerializeField] private GameObject plusButton = null;
	[SerializeField] FirstStatusElement targetElement = FirstStatusElement.None;

	private void Start()
	{
		statusNameLabel.text = targetElement.ToString();

		Refresh();
	}

	public void Refresh()
	{
		statusValueLabel.text = string.Format("{0:#,##0}", GetStatusValue(targetElement));

		Character character = EntityManager.Instance.GetMyCharacter();
		plusButton.SetActive(character.IsSelectableFirstStatus);
	}

	private int GetStatusValue(FirstStatusElement target)
	{
		Character character = EntityManager.Instance.GetMyCharacter();
        CharacterStatusData characterStatusData = character.GetComponent<CharacterStatusData>();

		switch (target)
		{
			case FirstStatusElement.STR: return characterStatusData.STR;
			case FirstStatusElement.DEX: return characterStatusData.DEX;
			case FirstStatusElement.CON: return characterStatusData.CON;
			case FirstStatusElement.INT: return characterStatusData.INT;
			case FirstStatusElement.WIS: return characterStatusData.WIS;
			case FirstStatusElement.CHA: return characterStatusData.CHA;
		}

		Debug.LogError("target is invalid! target : " + target.ToString());
		return 0;
	}

	#region Event Handler
	public void OnPlusBtnClicked()
	{
		Character character = EntityManager.Instance.GetMyCharacter();
		if (!character.IsSelectableFirstStatus)
		{
			return;
		}

		RoomNetwork.Instance.Send(new CS_FirstStatusSelection(character.EntityID, targetElement), PhotonNetwork.masterClient.ID);
	}
	#endregion
}
