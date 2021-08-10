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

    private EntityTransformSnap lastEntityTransformSnap;

    private RoomProtocolDispatcher roomProtocolDispatcher = null;

    protected override void Awake()
    {
        base.Awake();

        fpm_Move = gameObject.AddComponent<FPM_Move>();
        fpm_Jump = gameObject.AddComponent<FPM_Jump>();

        TickPubSubService.AddSubscriber("EarlyTick", OnEarlyTick);

        roomProtocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();
        roomProtocolDispatcher[typeof(SC_Synchronization)] = OnSC_Synchronization;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        TickPubSubService.RemoveSubscriber("EarlyTick", OnEarlyTick);
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
                    lastEntityTransformSnap = entityTransformSnap;
                }
            }
        });
    }

    private void OnEarlyTick(int tick)
    {
        fpm_Jump.ProcessJumpInputData();
        fpm_Move.ProcessPlayerMoveInput();
    }

    private void Reconcile()
    {
        if (Entities.MyCharacter == null || lastEntityTransformSnap == null)
        {
            return;
        }

        //  Position
        var expectedPosition = lastEntityTransformSnap.position + lastEntityTransformSnap.velocity * (Game.Current.GameTime - lastEntityTransformSnap.GameTime);
        Entities.MyCharacter.Position = Vector3.Lerp(Entities.MyCharacter.Position, expectedPosition, 0.05f);

        //  Rotation
        var expectedRotation = lastEntityTransformSnap.rotation + lastEntityTransformSnap.angularVelocity * (Game.Current.GameTime - lastEntityTransformSnap.GameTime);
        Entities.MyCharacter.Rotation = Quaternion.Lerp(Quaternion.Euler(Entities.MyCharacter.Rotation), Quaternion.Euler(expectedRotation), 0.05f).eulerAngles;

        //  Velocity (input sequence를 기준으로 target들을 찾아서 비교해야 할 것 같음,, 우선 velocity pass)
        //Entities.MyCharacter.Velocity = Vector3.Lerp(Entities.MyCharacter.Velocity, lastEntityTransformSnap.velocity, 0.05f);

        //  AngularVelocity
        //Entities.MyCharacter.AngularVelocity = Vector3.Lerp(Entities.MyCharacter.AngularVelocity, lastEntityTransformSnap.angularVelocity, 0.05f);
    }

    private void LateUpdate()
    {
        Reconcile();
    }
}
