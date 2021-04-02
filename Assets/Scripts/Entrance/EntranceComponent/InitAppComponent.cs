using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitAppComponent : EntranceComponent
{
    protected override void OnUpdate()
    {
        base.OnUpdate();

        IsSuccess = LOP.Application.IsInitialized;
    }

    public override void OnStart()
    {
        Entrance.Instance.stateText.text = "Application 초기화중입니다.";
    }
}
