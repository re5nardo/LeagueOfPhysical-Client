using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using NetworkModel.Mirror;

public class SC_SynchronizationHandler
{
    public static void Handle(IMessage msg)
    {
        SC_Synchronization synchronization = msg as SC_Synchronization;

        SynchronizationManager.Handle(synchronization.listSnap);
    }
}
