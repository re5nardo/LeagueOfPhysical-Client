using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class LoginComponent : MonoEnumerator
{
    private void Awake()
    {
        SceneMessageBroker.AddSubscriber<LoginResult>(OnLoginResult);
    }

    private void OnDestroy()
    {
        SceneMessageBroker.RemoveSubscriber<LoginResult>(OnLoginResult);
    }

    private void OnLoginResult(LoginResult loginResult)
    {
        IsSuccess = loginResult.success;

        PopupManager.GetPopup<LoginPopup>()?.Hide();
    }

    public override void OnBeforeExecute()
    {
        if (!LoginController.TryAutoLogin())
        {
            PopupManager.Show<LoginPopup>();
        }
    }
}
