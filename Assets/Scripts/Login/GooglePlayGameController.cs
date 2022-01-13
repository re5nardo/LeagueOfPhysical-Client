using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GameFramework;
using System;

public class GooglePlayGameController : MonoSingleton<GooglePlayGameController>
{
    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    private void Initialize()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public static void Login(Action<bool, string> onResult)
    {
        Instance.LoginInternal(onResult);
    }

    private void LoginInternal(Action<bool, string> onResult)
    {
        Social.localUser.Authenticate((success, resaon) =>
        {
            if (success)
            {
                Debug.Log($"Succeed in authenticating. userName: {Social.localUser.userName}");
            }
            else
            {
                Debug.LogError($"Fail to authenticate. resaon: {resaon}");
            }

            onResult?.Invoke(success, resaon);
        });
    }

    public static void Logout()
    {
        Instance.LogoutInternal();
    }

    private void LogoutInternal()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }
}
