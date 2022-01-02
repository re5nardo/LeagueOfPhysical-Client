using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class GameStateSyncController : LOPMonoSyncControllerBase<GameStateSyncData>
{
    public override SyncScope SyncScope { get; protected set; } = SyncScope.Global;

    public override GameStateSyncData GetSyncData()
    {
        return new GameStateSyncData(LOP.Game.Current.GameStateMachine.CurrentState.GetType().Name);
    }

    public override void OnSync(SyncDataEntry value)
    {
        var gameStateSyncData = value.data as GameStateSyncData;

        SceneDataContainer.Get<GameData>().gameState = gameStateSyncData.state;
    }
}
