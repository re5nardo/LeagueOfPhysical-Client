using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System;

public class SubGameTimeSyncController : LOPMonoSyncControllerBase<SubGameTimeSyncData>
{
    public override SyncScope SyncScope { get; protected set; } = SyncScope.Global;

    public override SubGameTimeSyncData GetSyncData()
    {
        throw new NotImplementedException("[SubGameTimeSyncController] GetSyncData");
    }

    public override void OnSync(SubGameTimeSyncData value)
    {
        SceneDataContainer.Get<GameData>().subGameTime = value.time;
    }
}
