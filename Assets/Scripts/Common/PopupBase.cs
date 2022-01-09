using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);

        OnShown();
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        OnHided();
    }

    protected virtual void OnShown() { }
    protected virtual void OnHided() { }
}
