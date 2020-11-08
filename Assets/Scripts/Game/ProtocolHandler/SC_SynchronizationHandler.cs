using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class SC_SynchronizationHandler
{
    public static void Handle(IPhotonEventMessage msg)
    {
        SC_Synchronization synchronization = msg as SC_Synchronization;

        SynchronizationManager.Handle(synchronization.snaps);
    }
}
