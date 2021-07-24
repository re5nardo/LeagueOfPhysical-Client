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

    public void Reconcile(EntityTransformSnap entityTransformSnap, ref Vector3 sumOfPosition, ref Vector3 sumOfRotation, ref Vector3 sumOfVelocity)
    {
        int clientTargetTick = 0;

        if (serverTickNSequences.Exists(x => x.tick <= entityTransformSnap.Tick))
        {
            var targets = serverTickNSequences.FindAll(x => x.tick <= entityTransformSnap.Tick);
            targets.Sort((x, y) =>
            {
                return x.sequence.CompareTo(y.sequence);
            });

            var target = targets[targets.Count - 1];
            int offset = entityTransformSnap.Tick - target.tick;

            if (!clientTickNSequences.Exists(x => x.sequence == target.sequence))
            {
                Debug.LogError("clientTickNSequences does not exist!");
                return;
            }

            var clientTarget = clientTickNSequences.Find(x => x.sequence == target.sequence);
            clientTargetTick = clientTarget.tick + offset;

            if (clientTickNSequences.Exists(x => x.sequence == target.sequence + 1))
            {
                var clientTargetNext = clientTickNSequences.Find(x => x.sequence == target.sequence + 1);

                clientTargetTick = Mathf.Min(clientTargetTick, clientTargetNext.tick - 1);
            }
        }
        else
        {
            clientTargetTick = entityTransformSnap.Tick;
        }
        
        var historiesToApply = FPM_Manager.Instance.TransformHistories.FindAll(x => x.tick > clientTargetTick);
        foreach (var history in historiesToApply)
        {
            sumOfPosition += history.positionChange.XZ();
            sumOfRotation += history.rotationChange;
            sumOfVelocity += history.velocityChange.XZ();
        }
    }

    private bool CanMove()
    {
        return true;
    }
}
