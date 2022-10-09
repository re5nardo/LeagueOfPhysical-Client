using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Entity;

public partial class FlapWang : SubGameBase
{
    protected override IEnumerator OnInitialize()
    {
        foreach (var entity in Entities.GetAll<LOPMonoEntityBase>())
        {
            entity.Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

            if (entity.EntityRole == EntityRole.Player)
            {
                var behaviorController = entity.GetComponent<BehaviorController>();
                behaviorController.StartBehavior(new ContinuousMoveBehaviorParam(Define.MasterData.BehaviorId.ContinuousMove, Vector3.right));
            }
        }

        Physics.gravity *= LOP.Game.Current.MapData.mapEnvironment.GravityFactor;

        LOP.Game.Current.GameUI.CameraController.Camera.orthographic = true;
        LOP.Game.Current.GameUI.CameraController.Camera.orthographicSize = 15;
        LOP.Game.Current.GameUI.CameraController.Camera.transform.localPosition = new Vector3(8, 0, -20);

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Character"), LayerMask.NameToLayer("Character"), true);

        yield break;
    }

    protected override IEnumerator OnFinalize()
    {
        Physics.gravity /= LOP.Game.Current.MapData.mapEnvironment.GravityFactor;

        yield break;
    }

    protected override void OnGameStart()
    {
    }

    protected override void OnGameEnd()
    {
    }

    protected override void OnTick(int tick)
    {
    }

    protected override void OnEarlyTickEnd(int tick)
    {
    }
}
