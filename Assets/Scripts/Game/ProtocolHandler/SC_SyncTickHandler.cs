using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using NetworkModel.Mirror;

public class SC_SyncTickHandler
{
    public static void Handle(IMessage msg)
    {
        SC_SyncTick syncTick = msg as SC_SyncTick;

        Game.Current.SetSyncTick(syncTick.tick);
    }
}
