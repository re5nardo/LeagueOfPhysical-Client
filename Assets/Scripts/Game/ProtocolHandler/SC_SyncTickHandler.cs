using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using NetworkModel.Mirror;

public class SC_SyncTickHandler
{
    public static void Handle(SC_SyncTick syncTick)
    {
        Game.Current.SetSyncTick(syncTick.tick);
    }
}
