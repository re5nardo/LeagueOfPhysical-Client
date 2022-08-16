using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System;

public class IntroComponent : EntranceComponentBase
{
    [SerializeField] private GameObject goCI;
    [SerializeField] private GameObject goIntro;

    public override async Task OnExecute()
    {
        goCI.SetActive(true);
        goIntro.SetActive(false);

        await UniTask.Delay(TimeSpan.FromSeconds(2), ignoreTimeScale: false);

        goCI.SetActive(false);
        goIntro.SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(2), ignoreTimeScale: false);

        goIntro.SetActive(false);
    }
}
