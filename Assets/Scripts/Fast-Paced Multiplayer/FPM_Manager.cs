using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UniRx;
using Entity;

//  Fast-Paced Multiplayer (FPM)
//  https://www.gabrielgambetta.com/client-server-game-architecture.html

public class FPM_Manager : MonoSingleton<FPM_Manager>
{
    private long sequence = 0;
    private PlayerMoveInput playerMoveInput = null;

    protected override void Awake()
    {
        base.Awake();

        MessageBroker.Default.Receive<PlayerMoveInput>().Subscribe(OnPlayerMoveInput).AddTo(this);

        TickPubSubService.AddSubscriber("EarlyTick", OnEarlyTick);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        TickPubSubService.RemoveSubscriber("EarlyTick", OnEarlyTick);
    }

    private void OnEarlyTick(int tick)
    {
        if (playerMoveInput == null)
            return;

        playerMoveInput.tick = Game.Current.CurrentTick;
        playerMoveInput.sequence = sequence++;
        playerMoveInput.entityID = Entities.MyCharacter.EntityID;
        playerMoveInput.position = Entities.MyCharacter.Position;
        playerMoveInput.rotation = Entities.MyCharacter.Rotation;

        CS_NotifyMoveInputData notifyMoveInputData = new CS_NotifyMoveInputData();
        notifyMoveInputData.m_PlayerMoveInput = playerMoveInput;

        //  Send to server (우선 서버에 전송)
        RoomNetwork.Instance.Send(notifyMoveInputData, PhotonNetwork.masterClient.ID, bInstant: true);

        //  클라이언트 선처리 (서버에 도달했을 때 예측해서)
        var Entity = Entities.Get(playerMoveInput.entityID);

        if (playerMoveInput.inputType == PlayerMoveInput.InputType.Hold)
        {
            //if (CanMove())
            {
                var behaviorController = Entity.GetEntityComponent<BehaviorController>();
                behaviorController.Move(Entity.Position + playerMoveInput.inputData.ToVector3().normalized * Game.Current.TickInterval * 5 * (Entity as Character).MovementSpeed);
            }
        }
        else if (playerMoveInput.inputType == PlayerMoveInput.InputType.Release)
        {
            var behaviorController = Entity.GetEntityComponent<BehaviorController>();
            behaviorController.StopBehavior(Define.MasterData.BehaviorID.MOVE);
        }

        playerMoveInput = null;
    }

    public void OnPlayerMoveInput(PlayerMoveInput playerMoveInput)
    {
        //  가장 마지막 인풋만 처리한다. (유입되는 모든 인풋을 처리하면 update 인터벌과 tick 인터벌의 차이로 인해 인풋이 밀리는 결과 초래 - update가 1초 tick이 2초마다 수행되면 2배만큼 인풋이 늦게 처리됨)
        this.playerMoveInput = playerMoveInput;
    }
}
