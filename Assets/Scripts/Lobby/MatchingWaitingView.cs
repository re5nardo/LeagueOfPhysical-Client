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
        title.text = "�÷��̾ ã�� ��";
        tip.text = "Tip �����Դϴ�! ���� �Ϸ絵 ���� �Ϸ� ��������!";
    }

    public static MatchingWaitingView CreateInstance()
    {
        return Instantiate(Resources.Load<MatchingWaitingView>("UI/MatchingWaitingView"));
    }
}
