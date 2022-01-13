using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using GameFramework;

[RequireComponent(typeof(Button))]
public class LogoutButton : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AsObservable().Subscribe(_ =>
        {
            AppMessageBroker.Publish<LogoutMessage>();
        })
        .AddTo(this)
        ;
    }
}
