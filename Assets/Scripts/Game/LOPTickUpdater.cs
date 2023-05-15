using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class LOPTickUpdater : TickUpdater
{
    protected override void OnUpdateElapsedTime()
    {
        var expected = ElapsedTime + Time.deltaTime;

        ElapsedTime = Mathf.Lerp((float)expected, (float)Mirror.NetworkTime.time, 0.25f);
    }
}
