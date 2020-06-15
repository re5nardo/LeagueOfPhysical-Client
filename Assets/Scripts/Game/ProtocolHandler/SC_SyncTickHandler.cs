using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class SC_SyncTickHandler : IHandler<IPhotonEventMessage>
{
    public void Handle(IPhotonEventMessage msg)
    {
        SC_SyncTick syncTick = msg as SC_SyncTick;

        Game.Current.SetSyncTick(syncTick.tick);
    }
}
