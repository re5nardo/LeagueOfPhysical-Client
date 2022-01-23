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

    public override void OnSync(GameStateSyncData value)
    {
        SceneDataContainer.Get<GameData>().gameState = value.state;
    }
}
