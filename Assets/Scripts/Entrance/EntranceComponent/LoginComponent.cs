using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System;

public class LoginComponent : EntranceComponentBase
{
    private LoginResult? loginResult;

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
        this.loginResult = loginResult;

        PopupManager.GetPopup<LoginPopup>()?.Hide();
    }

    public override async Task OnBeforeExecute()
    {
        if (!LoginController.TryAutoLogin())
        {
            PopupManager.Show<LoginPopup>();
        }
    }

    public override async Task OnExecute()
    {
        await UniTask.WaitUntil(() => loginResult.HasValue);

        if (!loginResult.Value.success)
        {
            throw new Exception(loginResult.Value.reason);
        }
    }
}
