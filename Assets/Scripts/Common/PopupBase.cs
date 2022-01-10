using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
    public PopupBase Show()
    {
        gameObject.SetActive(true);

        OnShown();

        return this;
    }

    public PopupBase Hide()
    {
        gameObject.SetActive(false);

        OnHided();

        return this;
    }

    protected virtual void OnShown() { }
    protected virtual void OnHided() { }
}
