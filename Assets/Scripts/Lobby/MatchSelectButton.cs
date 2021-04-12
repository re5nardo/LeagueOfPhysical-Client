using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class MatchSelectButton : MonoBehaviour
{
    [SerializeField] private RawImage icon = null;
    [SerializeField] private TextMeshProUGUI subGameName = null;
    [SerializeField] private TextMeshProUGUI mapName = null;

    private void Start()
    {
        Refresh();

        var matchSelectData = SceneDataContainer.Get<MatchSelectData>();
        matchSelectData.currentMatchSetting.Subscribe(_ => Refresh()).AddTo(this);
    }

    private void Refresh()
    {
        var matchSelectData = SceneDataContainer.Get<MatchSelectData>();

        subGameName.text = matchSelectData.currentMatchSetting.Value.subGameId;
        mapName.text = matchSelectData.currentMatchSetting.Value.mapId;
    }
}
