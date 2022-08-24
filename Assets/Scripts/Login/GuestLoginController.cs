using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework;

public class GuestLoginController : MonoSingleton<GuestLoginController>
{
    public static void Login(Action<bool, string, string> onResult)
    {
        onResult?.Invoke(true, "always success", string.IsNullOrEmpty(LOPSettings.Get().customId) ? SystemInfo.deviceUniqueIdentifier : LOPSettings.Get().customId);
    }

    public static void Logout()
    {
    }
}
