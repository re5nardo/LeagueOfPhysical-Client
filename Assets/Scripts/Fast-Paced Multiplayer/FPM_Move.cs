using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UniRx;
using Entity;
using NetworkModel.Mirror;

public class FPM_Move : MonoBehaviour
{
    private long sequence = 0;
    private PlayerMoveInput playerMoveInput = null;

    private List<TickNSequence> clientTickNSequences = new List<TickNSequence>();
    private List<TickNSequence> serverTickNSequences = new List<TickNSequence>();

    private RoomProtocolDispatcher roomProtocolDispatcher = null;

    private void Awake()
    {
        MessageBroker.Default.Receive<PlayerMoveInput>().Subscribe(OnPlayerMoveInput).AddTo(this);

        roomProtocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();
        roomProtocolDispatcher[typeof(SC_ProcessInputData)] = OnSC_ProcessInputData;
    }

    public void ProcessPlayerMoveInput()
    {
        if (playerMoveInput == null)
        {
            return;
        }

        if (CanMove())
        {
            clientTickNSequences.Add(new TickNSequence(Game.Current.CurrentTick, sequence));
            if (clientTickNSequences.Count > 100)
            {
                clientTickNSequences.RemoveRange(0, clientTickNSequences.Count - 100);
            }

            //  우선 서버에 전송
            playerMoveInput.tick = Game.Current.CurrentTick;
            playerMoveInput.sequence = sequence++;
            playerMoveInput.entityId = Entities.MyEntityID;

            CS_NotifyMoveInputData notifyMoveInputData = new CS_NotifyMoveInputData();
            notifyMoveInputData.playerMoveInput = playerMoveInput;

            RoomNetwork.Instance.Send(notifyMoveInputData, 0, instant: true);

            //  클라에서 인풋 선 처리 (서버에 도달했을 때 예측해서)
            Predict();
        }

        playerMoveInput = null;
    }

    private void Predict()
    {
        var entity = Entities.Get<Character>(playerMoveInput.entityId);

        if (playerMoveInput.inputType == PlayerMoveInput.InputType.Hold)
        {
            //if (CanMove())
            {
                var behaviorController = entity.GetEntityComponent<BehaviorController>();
                behaviorController.Move(entity.Position + playerMoveInput.inputData.normalized * Game.Current.TickInterval * 5 * entity.FactoredMovementSpeed);
            }
        }
        else if (playerMoveInput.inputType == PlayerMoveInput.InputType.Release)
        {
            var behaviorController = entity.GetEntityComponent<BehaviorController>();
            behaviorController.StopBehavior(Define.MasterData.BehaviorID.MOVE);
        }
    }

    private void OnPlayerMoveInput(PlayerMoveInput playerMoveInput)
    {
        //  가장 마지막 인풋만 처리한다. (유입되는 모든 인풋을 처리하면 update 인터벌과 tick 인터벌의 차이로 인해 인풋이 밀리는 결과 초래 - update가 1초 tick이 2초마다 수행되면 2배만큼 인풋이 늦게 처리됨)
        this.playerMoveInput = playerMoveInput;
    }

    private void OnSC_ProcessInputData(IMessage msg)
    {
        SC_ProcessInputData processInputData = msg as SC_ProcessInputData;

        if (processInputData.type == "move")
        {
            serverTickNSequences.Add(new TickNSequence(processInputData.tick, processInputData.sequence));
            if (serverTickNSequences.Count > 100)
            {
                serverTickNSequences.RemoveRange(0, serverTickNSequences.Count - 100);
            }
        }
    }

    private bool CanMove()
    {
        return true;
    }
}
