using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using NetworkModel.Mirror;

//  Fast-Paced Multiplayer (FPM)
//  https://www.gabrielgambetta.com/client-server-game-architecture.html

public class FPM_Manager : MonoSingleton<FPM_Manager>
{
    private FPM_Move fpm_Move = null;
    private FPM_Jump fpm_Jump = null;

    private List<TransformHistory> transformHistories = new List<TransformHistory>();
    public List<TransformHistory> TransformHistories => transformHistories;

    private List<EntityTransformSnap> entityTransformSnaps = new List<EntityTransformSnap>();

    private Vector3 earlyTickPosition;
    private Vector3 earlyTickRotation;
    private Vector3 earlyTickVelocity;

    private Vector3 positionGap;
    private Vector3 rotationGap;
    private Vector3 velocityGap;

    private const int RECONCILE_PROCESS = 4;    //  이 틱 기간동안 보정해준다.
    private int reconcileCount = 0;

    private RoomProtocolDispatcher roomProtocolDispatcher = null;

    protected override void Awake()
    {
        base.Awake();

        fpm_Move = gameObject.AddComponent<FPM_Move>();
        fpm_Jump = gameObject.AddComponent<FPM_Jump>();

        TickPubSubService.AddSubscriber("EarlyTick", OnEarlyTick);
        TickPubSubService.AddSubscriber("LateTickEnd", OnLateTickEnd);

        roomProtocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();
        roomProtocolDispatcher[typeof(SC_Synchronization)] = OnSC_Synchronization;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        TickPubSubService.RemoveSubscriber("EarlyTick", OnEarlyTick);
        TickPubSubService.RemoveSubscriber("LateTickEnd", OnLateTickEnd);
    }

    private void OnSC_Synchronization(IMessage msg)
    {
        SC_Synchronization synchronization = msg as SC_Synchronization;

        synchronization.listSnap?.ForEach(snap =>
        {
            if (snap is EntityTransformSnap entityTransformSnap)
            {
                if (entityTransformSnap.entityId == Entities.MyEntityID)
                {
                    entityTransformSnaps.Add(entityTransformSnap);
                }
            }
        });
    }

    private void OnEarlyTick(int tick)
    {
        if (Entities.MyCharacter != null)
        {
            earlyTickPosition = Entities.MyCharacter.Position;
            earlyTickRotation = Entities.MyCharacter.Rotation;
            earlyTickVelocity = Entities.MyCharacter.Velocity;
        }

        fpm_Jump.ProcessJumpInputData();
        fpm_Move.ProcessPlayerMoveInput();
    }

    private void OnLateTickEnd(int tick)
    {
        if (Entities.MyCharacter != null)
        {
            var positionChange = Entities.MyCharacter.Position - earlyTickPosition;
            var rotationChange = Entities.MyCharacter.Rotation - earlyTickRotation;
            var velocityChange = Entities.MyCharacter.Velocity - earlyTickVelocity;

            transformHistories.Add(new TransformHistory(Game.Current.CurrentTick, Entities.MyCharacter.Position, Entities.MyCharacter.Rotation, Entities.MyCharacter.Velocity, positionChange, rotationChange, velocityChange));
            if (transformHistories.Count > 100)
            {
                transformHistories.RemoveRange(0, transformHistories.Count - 100);
            }
        }

        Reconcile();
    }

    private void Reconcile()
    {
        if (entityTransformSnaps.Count > 0)
        {
            //  서버에서 받은 마지막 snap을 사용하여 reconcile 한다.
            var entityTransformSnap = entityTransformSnaps[entityTransformSnaps.Count - 1];
            entityTransformSnaps.Clear();

            Vector3 sumOfPosition = Vector3.zero;
            Vector3 sumOfRotation = Vector3.zero;
            Vector3 sumOfVelocity = Vector3.zero;

            fpm_Move.Reconcile(entityTransformSnap, ref sumOfPosition, ref sumOfRotation, ref sumOfVelocity);   //  rotation은 xz(move에만 변경되므로)
            fpm_Jump.Reconcile(entityTransformSnap, ref sumOfPosition, ref sumOfVelocity);

            var reconcilePosition = entityTransformSnap.position + sumOfPosition;
            var reconcileRotation = entityTransformSnap.rotation + sumOfRotation;
            var reconcileVelocity = entityTransformSnap.velocity + sumOfVelocity;

            positionGap = reconcilePosition - Entities.MyCharacter.Position;
            var reconcileForward = Quaternion.Euler(reconcileRotation) * Vector3.forward;
            rotationGap = new Vector3(0, Vector3.SignedAngle(Entities.MyCharacter.Forward, reconcileForward, Vector3.up), 0);
            velocityGap = reconcileVelocity - Entities.MyCharacter.Velocity;

            reconcileCount = RECONCILE_PROCESS;
        }

        if (reconcileCount > 0)
        {
            Entities.MyCharacter.Position += (positionGap / RECONCILE_PROCESS);
            Entities.MyCharacter.Rotation += (rotationGap / RECONCILE_PROCESS);
            Entities.MyCharacter.Velocity += (velocityGap / RECONCILE_PROCESS);

            reconcileCount--;
        }
    }
}
