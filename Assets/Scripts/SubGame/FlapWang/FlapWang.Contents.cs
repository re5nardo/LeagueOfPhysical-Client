using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using System;
using GameFramework;

public partial class FlapWang
{
    private void Start()
    {
        SceneMessageBroker.AddSubscriber<GameMessage.EntityRegister>(OnEntityRegister);
        SceneMessageBroker.AddSubscriber<EntityMessage.ModelTriggerEnter>(OnModelTriggerEnter);
    }

    private void OnDestroy()
    {
        SceneMessageBroker.RemoveSubscriber<GameMessage.EntityRegister>(OnEntityRegister);
        SceneMessageBroker.RemoveSubscriber<EntityMessage.ModelTriggerEnter>(OnModelTriggerEnter);
    }

    private void OnEntityRegister(GameMessage.EntityRegister message)
    {
        var entity = Entities.Get<LOPMonoEntityBase>(message.entityId);

        entity.Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

        if (entity.EntityRole == EntityRole.Player)
        {
            var behaviorController = entity.GetComponent<BehaviorController>();
            behaviorController.StartBehavior(new ContinuousMoveBehaviorParam(Define.MasterData.BehaviorId.ContinuousMove, Vector3.right));

            entity.Rotation = new Vector3(0, 90, 0);
        }
    }

    private void OnModelTriggerEnter(EntityMessage.ModelTriggerEnter message)
    {
        var entity = Entities.Get<LOPMonoEntityBase>(message.entityId);

        if (entity.EntityRole != EntityRole.Player
            || !(entity is Character character)
            || !character.IsAlive
            || character.HasStatusEffect(StatusEffect.Invincible)
            )
        {
            return;
        }

        Action<Behavior.BehaviorBase> onDieEnd = behavior =>
        {
            Resurrect(character);
        };

        Die(character, onDieEnd);
    }

    private void Die(Character character, Action<Behavior.BehaviorBase> onDieEnd)
    {
        character.Blackboard.Set("diePosition", character.Position);
        character.HP = 0;

        character.BehaviorController.StartBehavior(new BehaviorParam(Define.MasterData.BehaviorId.Die), onDieEnd);

        if (character.EntityID == Entities.MyEntityID)
        {
            LOP.Game.Current.GameUI.CameraController.FollowTarget = false;
        }
    }

    private void Resurrect(Character character)
    {
        //  Transform
        character.Position = character.Blackboard.Get<Vector3>("diePosition", true);
        character.Rotation = new Vector3(0, 90, 0);
        character.Velocity = Vector3.zero;

        //  HP
        character.HP = character.MaximumHP;

        //  ContinuousMove
        character.BehaviorController.StartBehavior(new ContinuousMoveBehaviorParam(Define.MasterData.BehaviorId.ContinuousMove, Vector3.right));

        //  Invincible
        character.StateController.StartState(new BasicStateParam(Define.MasterData.StateId.Invincible, 2));

        if (character.EntityID == Entities.MyEntityID)
        {
            LOP.Game.Current.GameUI.CameraController.FollowTarget = true;
        }
    }
}
