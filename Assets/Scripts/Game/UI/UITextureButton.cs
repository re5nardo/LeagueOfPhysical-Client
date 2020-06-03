using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UITextureButton : MonoBehaviour
{
    public RawImage textureImage = null;

    public event Action onClicked = null;

	#region Event Handler
	public void OnClicked()
    {
		onClicked?.Invoke();
	}
	#endregion
}
