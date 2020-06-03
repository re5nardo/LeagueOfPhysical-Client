using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class PopupInfo
{
    public PopupInfo(string strPopupName, PopupBase popupBase)
    {
        m_strPopupName = strPopupName;
        m_PopupBase = popupBase;
    }

    public string m_strPopupName = "";
    public PopupBase m_PopupBase = null;
}

public class PopupManager : MonoSingleton<PopupManager>
{
    private List<PopupInfo> m_listPopupInfo = new List<PopupInfo>();

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    public T Show<T>(string strPopupName) where T : PopupBase
    {
        T popup = GetPopup<T>(strPopupName);
        if (popup == null)
        {
            popup = CreatePopup<T>(strPopupName);
        }

        popup.Show();

        return popup;
    }

    public T GetPopup<T>(string strPopupName) where T : PopupBase
    {
        var found = m_listPopupInfo.FindLast((x) => x.m_strPopupName == strPopupName);

        if (found == null)
        {
            return null;
        }

        return found.m_PopupBase as T;
    }

    private T CreatePopup<T>(string strPopupName) where T : PopupBase
    {
        GameObject goPopup = Instantiate<GameObject>(Resources.Load<GameObject>("Popup/" + strPopupName));

        //goPopup.transform.SetParent(UIRoot.list[0].transform);
        //goPopup.transform.localPosition = Vector3.zero;
        //goPopup.transform.localRotation = Quaternion.identity;
        //goPopup.transform.localScale = Vector3.one;

        T popup = goPopup.GetComponent<T>();

        m_listPopupInfo.Add(new PopupInfo(strPopupName, popup));

        return popup;
    }
}
