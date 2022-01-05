using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class TickSyncController : LOPMonoSyncControllerBase<TickSyncData>
{
    public override SyncScope SyncScope { get; protected set; } = SyncScope.Global;

    public override TickSyncData GetSyncData()
    {
        return new TickSyncData(Game.Current.CurrentTick);
    }

    public override void OnSync(SyncDataEntry value)
    {
        var tickSyncData = value.data as TickSyncData;

        SceneDataContainer.Get<GameData>().tick = tickSyncData.tick;
    }
}
