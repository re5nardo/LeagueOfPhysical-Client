using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class PopupManager : MonoSingleton<PopupManager>
{
    private List<PopupInfo> popupInfoList = new List<PopupInfo>();

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public static T Show<T>(string popupName = null) where T : PopupBase
    {
        return Instance.ShowInternal<T>(popupName);
    }

    private T ShowInternal<T>(string popupName = null) where T : PopupBase
    {
        T popup = GetPopup<T>(popupName);
        if (popup == null)
        {
            popup = CreatePopup<T>(popupName);
        }

        return popup.Show() as T;
    }

    public static T GetPopup<T>(string popupName = null) where T : PopupBase
    {
        return Instance.GetPopupInternal<T>(popupName);
    }

    private T GetPopupInternal<T>(string popupName = null) where T : PopupBase
    {
        var found = popupInfoList.FindLast(x => x.popupName == (popupName ?? typeof(T).Name));

        if (found == null)
        {
            return null;
        }

        return found.popupBase as T;
    }

    private T CreatePopup<T>(string popupName = null) where T : PopupBase
    {
        GameObject goPopup = Instantiate(Resources.Load<GameObject>($"Popup/{popupName ?? typeof(T).Name}"));

        //goPopup.transform.SetParent(UIRoot.list[0].transform);
        //goPopup.transform.localPosition = Vector3.zero;
        //goPopup.transform.localRotation = Quaternion.identity;
        //goPopup.transform.localScale = Vector3.one;

        T popup = goPopup.GetComponent<T>();

        popupInfoList.Add(new PopupInfo(popupName ?? typeof(T).Name, popup));

        return popup;
    }
}

public class PopupInfo
{
    public string popupName = "";
    public PopupBase popupBase = null;

    public PopupInfo(string popupName, PopupBase popupBase)
    {
        this.popupName = popupName;
        this.popupBase = popupBase;
    }
}