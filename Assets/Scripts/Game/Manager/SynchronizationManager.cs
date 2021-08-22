using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class SynchronizationManager : MonoSingleton<SynchronizationManager>
{
    public static void Handle(List<ISnap> snaps)
    {
        Instance.HandleInternal(snaps);
    }

    private void HandleInternal(List<ISnap> snaps)
    {
        if (snaps == null)
            return;

        foreach (var snap in snaps)
        {
            if (snap == null)
                continue;
        }
    }
}
