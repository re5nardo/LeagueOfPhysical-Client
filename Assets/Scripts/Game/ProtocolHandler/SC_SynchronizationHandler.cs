using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkModel.Mirror;

public class SC_SynchronizationHandler
{
    public static void Handle(SC_Synchronization synchronization)
    {
        synchronization.listSnap?.ForEach(snap =>
        {
            SceneMessageBroker.Publish(snap.GetType(), snap);
        });
    }
}
