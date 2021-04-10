using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class MatchingWaitingView : MonoSingleton<MatchingWaitingView>
{
    [SerializeField] private TextMeshProUGUI title = null;
    [SerializeField] private TextMeshProUGUI tip = null;
    [SerializeField] private Button cancelButton = null;

    private void Start()
    {
        cancelButton.onClick.AsObservable().Subscribe(_ =>
        {
            Lobby.MessageBroker.Publish("OnCancelMatchingButtonClicked");
        })
        .AddTo(this);
    }

    public static void Show()
    {
        Instance.gameObject.SetActive(true);

        Instance.Refresh();
    }

    public static void Hide()
    {
        Instance.gameObject.SetActive(false);
    }

    private void Refresh()
    {
        title.text = "플레이어를 찾는 중";
        tip.text = "Tip 영역입니다! 오늘 하루도 좋은 하루 보내세요!";
    }

    public static MatchingWaitingView CreateInstance()
    {
        return Instantiate(Resources.Load<MatchingWaitingView>("UI/MatchingWaitingView"));
    }
}
