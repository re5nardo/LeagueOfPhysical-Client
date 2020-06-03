using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStatusSelectionUI : MonoBehaviour
{
	[SerializeField] private List<FirstStatusSelectionItemUI> firstStatusSelectionItemUIs = null;
	
	public void Toggle()
	{
		gameObject.SetActive(!gameObject.activeSelf);
	}

	public void Refresh()
	{
		firstStatusSelectionItemUIs.ForEach(x => x.Refresh());
	}
}
