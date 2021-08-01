using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UniRx;

public class MatchSelectData : MonoBehaviour
{
    public ReactiveProperty<MatchSetting> currentMatchSetting = new ReactiveProperty<MatchSetting>();

    private void Awake()
    {
        Initialize();
    }

    protected void Initialize()
    {
        var initValue = MatchSettingPreset.Get(x => x.matchSetting.matchType == MatchType.Friendly);

        currentMatchSetting.Value = initValue.matchSetting;
    }
}
