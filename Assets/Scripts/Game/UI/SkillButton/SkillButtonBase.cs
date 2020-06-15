using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SkillButtonBase : MonoBehaviour
{
	public RawImage textureIcon = null;
    public event Action<int> onClicked = null;

	protected int skillID = -1;

	public void SetSkillID(int skillID)
	{
		this.skillID = skillID;
	}

	#region Event Handler
	public virtual void OnClicked()
	{
		onClicked?.Invoke(skillID);
	}
	#endregion
}
