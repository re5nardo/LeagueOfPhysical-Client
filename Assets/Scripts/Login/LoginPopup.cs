using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;

public class LoginPopup : PopupBase
{
    [SerializeField] private Button gpgsLoginButton;
    [SerializeField] private Button gamecenterLoginButton;
    [SerializeField] private Button guestLoginButton;

    private void Awake()
    {
        gpgsLoginButton.onClick.AsObservable().Subscribe(_ => LoginController.Login(LoginType.GooglePlayGame)).AddTo(this);
        gamecenterLoginButton.onClick.AsObservable().Subscribe(_ => throw new NotImplementedException()).AddTo(this);
        guestLoginButton.onClick.AsObservable().Subscribe(_ => LoginController.Login(LoginType.Guest)).AddTo(this);
    }

    protected override void OnShown()
    {
        base.OnShown();

        gpgsLoginButton.gameObject.SetActive(false);
        gamecenterLoginButton.gameObject.SetActive(false);
        guestLoginButton.gameObject.SetActive(false);

#if !UNITY_EDITOR && UNITY_ANDROID
        gpgsLoginButton.gameObject.SetActive(true);
#elif !UNITY_EDITOR && UNITY_IOS
        guestLoginButton.gameObject.SetActive(true);
#endif
        guestLoginButton.gameObject.SetActive(true);
    }
}
