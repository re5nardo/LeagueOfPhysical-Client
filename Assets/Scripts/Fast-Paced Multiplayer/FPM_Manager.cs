using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;

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

    private Vector3? reconcilePosition;

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

        synchronization.snaps?.ForEach(snap =>
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
        Reconcile();    //  여기 타이밍 맞나??

        if (Entities.MyCharacter != null)
        {
            earlyTickPosition = Entities.MyCharacter.Position;
            earlyTickRotation = Entities.MyCharacter.Rotation;
        }

        fpm_Jump.ProcessJumpInputData();
        fpm_Move.ProcessPlayerMoveInput();
    }

    private void OnLateTickEnd(int tick)
    {
        if (Entities.MyCharacter == null)
        {
            return;
        }

        var positionChange = Entities.MyCharacter.Position - earlyTickPosition;
        var rotationChange = Entities.MyCharacter.Rotation - earlyTickRotation;

        transformHistories.Add(new TransformHistory(Game.Current.CurrentTick, Entities.MyCharacter.Position, Entities.MyCharacter.Rotation, positionChange, rotationChange));
        if (transformHistories.Count > 100)
        {
            transformHistories.RemoveRange(0, transformHistories.Count - 100);
        }
    }

    private void Reconcile()
    {
        if (entityTransformSnaps.Count == 0)
        {
            if (reconcilePosition.HasValue)
            {
                Entities.MyCharacter.Position = Vector3.Lerp(Entities.MyCharacter.Position, reconcilePosition.Value, 0.1f); //  reconcilePosition + histories 해주고 lerp 해줘야..?
            }

            return;
        }

        //  서버에서 받은 마지막 snap을 사용하여 reconcile 한다.
        var entityTransformSnap = entityTransformSnaps[entityTransformSnaps.Count - 1];
        entityTransformSnaps.Clear();

        Vector3 sumOfPosition = Vector3.zero;
        Vector3 sumOfRotation = Vector3.zero;

        fpm_Move.Reconcile(entityTransformSnap, ref sumOfPosition, ref sumOfRotation);
        fpm_Jump.Reconcile(entityTransformSnap, ref sumOfPosition);

        //  조금 더 고도화를 해야 할 것 같은데...
        Entities.MyCharacter.Position = Vector3.Lerp(Entities.MyCharacter.Position, entityTransformSnap.position + sumOfPosition, 0.1f);    //  이 부분때문에 서로 밀 때 서버와 클라 위치가 많이 달라보이나?
        //Entities.MyCharacter.Position = entityTransformSnap.position + sumOfPosition;
        Entities.MyCharacter.Rotation = entityTransformSnap.rotation + sumOfRotation;

        reconcilePosition = entityTransformSnap.position + sumOfPosition;
    }
}

