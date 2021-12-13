using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Entity;

public class FallingGame : SubGameBase
{
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

        entity.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    protected override IEnumerator OnInitialize()
    {
        foreach (var entity in Entities.GetAll<LOPMonoEntityBase>())
        {
            if (entity.EntityRole != EntityRole.Player)
            {
                continue;
            }

            entity.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        Physics.gravity *= LOP.Game.Current.GameManager.MapData.mapEnvironment.GravityFactor;

        yield break;
    }

    protected override IEnumerator OnFinalize()
    {
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
