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

    public List<TransformHistory> TransformHistories { get; set; } = new List<TransformHistory>();

    public List<TransformHistory> reconcileHistories = new List<TransformHistory>();

    private Vector3 positionGap;
    private Vector3 rotationGap;

    private Vector3 lastPosition;
    private Vector3 lastRotation;
    private Vector3 lastVelocity;
    private Vector3 lastAngularVelocity;

    private const int RECONCILE_PROCESS = 6;    //  이 프레임 기간동안 보정해준다.
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

                    reconcileCount = RECONCILE_PROCESS;
                }
            }
        });
    }

    private void OnEarlyTick(int tick)
    {
        fpm_Jump.ProcessJumpInputData();
        fpm_Move.ProcessPlayerMoveInput();
    }

    private void OnLateTickEnd(int tick)
    {
        if (Entities.MyCharacter != null)
        {
            var positionChange = Entities.MyCharacter.Position - lastPosition;
            var rotationChange = Entities.MyCharacter.Rotation - lastRotation;
            var velocityChange = Entities.MyCharacter.Velocity - lastVelocity;
            var angularVelocityChange = Entities.MyCharacter.AngularVelocity - lastAngularVelocity;

            var reconcile = reconcileHistories.Find(x => x.tick == tick);
            if (reconcile != null)
            {
                positionChange -= reconcile.positionChange;
                rotationChange -= reconcile.rotationChange;
                velocityChange -= reconcile.velocityChange;
                angularVelocityChange -= reconcile.angularVelocityChange;
            }

            TransformHistories.Add(new TransformHistory(tick, positionChange, rotationChange, velocityChange, angularVelocityChange));
            if (TransformHistories.Count > 100)
            {
                TransformHistories.RemoveRange(0, TransformHistories.Count - 100);
            }

            lastPosition = Entities.MyCharacter.Position;
            lastRotation = Entities.MyCharacter.Rotation;
            lastVelocity = Entities.MyCharacter.Velocity;
            lastAngularVelocity = Entities.MyCharacter.AngularVelocity;
        }
    }

    private void Reconcile()
    {
        reconcileHistories.RemoveAll(x => x.tick < Game.Current.CurrentTick);

        if (reconcileCount > 0)
        {
            int targetTick = Game.Current.CurrentTick + 1;
            var targetHistory = reconcileHistories.Find(x => x.tick == targetTick);
            if (targetHistory == null)
            {
                TransformHistory transformHistory = new TransformHistory(targetTick, default, default, default, default);

                reconcileHistories.Add(transformHistory);

                targetHistory = transformHistory;
            }

            var pos = positionGap / RECONCILE_PROCESS;
            Entities.MyCharacter.Position += pos;
            targetHistory.positionChange += pos;

            var rot = rotationGap / RECONCILE_PROCESS;
            Entities.MyCharacter.Rotation += rot;
            targetHistory.rotationChange += rot;

            reconcileCount--;
        }
    }

    private void LateUpdate()
    {
        Reconcile();
    }
}
