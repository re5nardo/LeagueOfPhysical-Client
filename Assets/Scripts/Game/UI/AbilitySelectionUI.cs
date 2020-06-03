using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelectionUI : MonoBehaviour
{
	[SerializeField] private List<AbilitySelectionItemUI> abilitySelectionItemUIs = null;

	private void Awake()
	{
		abilitySelectionItemUIs.ForEach(x => x.onClicked += OnAbilitySelectionItemUIClicked);
	}

	private void OnDestroy()
	{
		abilitySelectionItemUIs.ForEach(x => x.onClicked -= OnAbilitySelectionItemUIClicked);
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public void SetAbilities(List<int> abilityIDs)
	{
		for(int i = 0; i < abilityIDs.Count; ++i)
		{
			abilitySelectionItemUIs[i].SetAbility(abilityIDs[i]);
		}
	}

	private void OnAbilitySelectionItemUIClicked(int abilityID)
	{
		CS_AbilitySelection abilitySelection = new CS_AbilitySelection(EntityManager.Instance.GetMyEntityID(), abilityID);

		RoomNetwork.Instance.Send(abilitySelection, PhotonNetwork.masterClient.ID);

		Hide();
	}
}
