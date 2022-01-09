using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitAppComponent : MonoEnumerator
{
    public override void OnBeforeExecute()
    {
        Entrance.Instance.stateText.text = "Application 초기화중입니다.";
    }

    public override IEnumerator OnExecute()
    {
        yield return new WaitUntil(() => LOP.Application.IsInitialized);

        IsSuccess = true;
    }
}
