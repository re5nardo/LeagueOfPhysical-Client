using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class InitAppComponent : EntranceComponentBase
{
    public override async Task OnBeforeExecute()
    {
        Entrance.Instance.stateText.text = "Application 초기화중입니다.";
    }

    public override async Task OnExecute()
    {
        await UniTask.WaitUntil(() => LOP.Application.IsInitialized);
    }
}
