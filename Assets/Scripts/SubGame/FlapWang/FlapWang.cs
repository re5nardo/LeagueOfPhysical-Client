using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Entity;

public class FlapWang : SubGameBase
{
    [SerializeField] private FlapWangUI flapWangUI = null;

    private void Start()
    {
        SceneMessageBroker.AddSubscriber<GameMessage.EntityRegister>(OnEntityRegister);
    }

    private void OnDestroy()
    {
        SceneMessageBroker.RemoveSubscriber<GameMessage.EntityRegister>(OnEntityRegister);
    }

    private void OnEntityRegister(GameMessage.EntityRegister message)
    {
        var entity = Entities.Get<LOPMonoEntityBase>(message.entityId);

        entity.Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

        if (entity.EntityRole == EntityRole.Player)
        {
            var behaviorController = entity.GetComponent<BehaviorController>();
            behaviorController.StartBehavior(new ContinuousMoveBehaviorParam(Define.MasterData.BehaviorID.CONTINUOUS_MOVE, Vector3.right));
        }
    }

    protected override IEnumerator OnInitialize()
    {
        yield return SceneManager.LoadSceneAsync(LOP.Game.Current.GameManager.MapData.sceneName, LoadSceneMode.Additive);

        foreach (var entity in Entities.GetAll<LOPMonoEntityBase>())
        {
            entity.Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

            if (entity.EntityRole == EntityRole.Player)
            {
                var behaviorController = entity.GetComponent<BehaviorController>();
                behaviorController.StartBehavior(new ContinuousMoveBehaviorParam(Define.MasterData.BehaviorID.CONTINUOUS_MOVE, Vector3.right));
            }
        }

        Physics.gravity *= LOP.Game.Current.GameManager.MapData.mapEnvironment.GravityFactor;

        LOP.Game.Current.GameUI.CameraController.Camera.orthographic = true;
        LOP.Game.Current.GameUI.CameraController.Camera.orthographicSize = 15;
        LOP.Game.Current.GameUI.CameraController.Camera.transform.localPosition = new Vector3(8, 0, -20);
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
