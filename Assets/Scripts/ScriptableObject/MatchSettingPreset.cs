using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MatchSettingPreset", menuName = "ScriptableObjects/MatchSettingPreset", order = 1)]
public class MatchSettingPreset : ScriptableObjectWrapper<MatchSettingPreset>
{
    public string title;
    public string description;
    public MatchSetting matchSetting;
}
