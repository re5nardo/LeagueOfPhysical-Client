using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UISpriteButton : MonoBehaviour
{
    [SerializeField] protected Image sprImage = null;

    public Action onClicked = null;

    public virtual void OnClicked()
    {
		onClicked?.Invoke();
	}
}
