using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitAppComponent : EntranceComponent
{
    public override void OnStart()
    {
        StartCoroutine(Body());
    }

    private IEnumerator Body()
    {
        logger?.Invoke("Application 초기화중입니다.");

        yield return new WaitUntil(() => LOP.Application.IsInitialized);

        IsSuccess = true;
    }
}
