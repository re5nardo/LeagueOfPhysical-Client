using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System;
using UnityEngine.SceneManagement;

public class LoginController : MonoSingleton<LoginController>
{
    private const string LOGIN_TYPE_KEY = "LOGIN_TYPE_KEY";

    private void Start()
    {
        DontDestroyOnLoad(this);

        AppMessageBroker.AddSubscriber<LogoutMessage>(OnLogout);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        AppMessageBroker.RemoveSubscriber<LogoutMessage>(OnLogout);
    }

    public static bool TryAutoLogin()
    {
        return Instance.TryAutoLoginInternal();
    }

    public bool TryAutoLoginInternal()
    {
        if (!PlayerPrefs.HasKey(LOGIN_TYPE_KEY))
        {
            return false;
        }

        var loginType = PlayerPrefs.GetString(LOGIN_TYPE_KEY).Parse<LoginType>();
        Login(loginType);

        return true;
    }

    public static void Login(LoginType loginType)
    {
        Instance.LoginInternal(loginType);
    }

    public void LoginInternal(LoginType loginType)
    {
        switch (loginType)
        {
            case LoginType.Guest:
                GuestLoginController.Login(OnGuestLoginResult);
                break;

            case LoginType.GooglePlayGame:
                GooglePlayGameController.Login(OnGooglePlayGameLoginResult);
                break;

            case LoginType.GameCenter:
                throw new NotImplementedException();
        }
    }

    private void OnGuestLoginResult(bool success, string reason, string id)
    {
        if (success)
        {
            PlayerPrefs.SetString(LOGIN_TYPE_KEY, nameof(LoginType.Guest));
            LOP.Application.UserId = id;
        }

        SceneMessageBroker.Publish(new LoginResult
        {
            success = success,
            reason = reason,
        });
    }

    private void OnGooglePlayGameLoginResult(bool success, string reason)
    {
        if (success)
        {
            PlayerPrefs.SetString(LOGIN_TYPE_KEY, nameof(LoginType.GooglePlayGame));
            LOP.Application.UserId = Social.localUser.id;
        }

        SceneMessageBroker.Publish(new LoginResult
        {
            success = success,
            reason = reason,
        });
    }

    private void OnLogout(LogoutMessage message)
    {
        if (!PlayerPrefs.HasKey(LOGIN_TYPE_KEY))
        {
            return;
        }

        var loginType = PlayerPrefs.GetString(LOGIN_TYPE_KEY).Parse<LoginType>();
        switch (loginType)
        {
            case LoginType.Guest:
                GuestLoginController.Logout();
                break;

            case LoginType.GooglePlayGame:
                GooglePlayGameController.Logout();
                break;

            case LoginType.GameCenter:
                throw new NotImplementedException();
        }

        PlayerPrefs.DeleteKey(LOGIN_TYPE_KEY);

        SceneManager.LoadScene("Entrance");
    }
}

public enum LoginType
{
    Guest = 0,
    GooglePlayGame = 1,
    GameCenter = 2,
}

public struct LoginResult
{
    public bool success;
    public string reason;

    public LoginResult(bool success, string reason)
    {
        this.success = success;
        this.reason = reason;
    }
}

public struct GooglePlayGameLoginResult
{
    public bool success;
    public string reason;

    public GooglePlayGameLoginResult(bool success, string reason)
    {
        this.success = success;
        this.reason = reason;
    }
}

public struct GuestLoginResult
{
    public bool success;
    public string reason;
    public string id;

    public GuestLoginResult(bool success, string reason, string id)
    {
        this.success = success;
        this.reason = reason;
        this.id = id;
    }
}

public struct LogoutMessage { }