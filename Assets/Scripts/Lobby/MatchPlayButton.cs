using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MatchPlayButton : MonoBehaviour
{
    [SerializeField] private Button playButton = null;

    private void Start()
    {
        Refresh();

        var matchSelectData = SceneDataContainer.Get<MatchSelectData>();
        matchSelectData.currentMatchSetting.Subscribe(setting => Refresh()).AddTo(this);

        playButton.onClick.AsObservable().Subscribe(_ => Lobby.MessageBroker.Publish("OnMatchPlayButtonClicked")).AddTo(this);
    }

    private void Refresh()
    {
    }
}
