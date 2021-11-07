using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UniRx;
using Entity;
using NetworkModel.Mirror;

public class FPM_Jump : MonoBehaviour
{
    private long sequence = 0;
    private JumpInputData jumpInputData = null;

    private List<TickNSequence> clientTickNSequences = new List<TickNSequence>();
    private List<TickNSequence> serverTickNSequences = new List<TickNSequence>();

    private void Awake()
    {
        SceneMessageBroker.AddSubscriber<JumpInputData>(OnJumpInputData);
        SceneMessageBroker.AddSubscriber<SC_ProcessInputData>(OnSC_ProcessInputData);
    }

    private void OnDestroy()
    {
        SceneMessageBroker.RemoveSubscriber<JumpInputData>(OnJumpInputData);
        SceneMessageBroker.RemoveSubscriber<SC_ProcessInputData>(OnSC_ProcessInputData);
    }

    public void ProcessJumpInputData()
    {
        if (jumpInputData == null)
        {
            return;
        }

        if (CanJump())
        {
            clientTickNSequences.Add(new TickNSequence(Game.Current.CurrentTick, sequence));
            if (clientTickNSequences.Count > 100)
            {
                clientTickNSequences.RemoveRange(0, clientTickNSequences.Count - 100);
            }

            //  우선 서버에 전송
            jumpInputData.tick = Game.Current.CurrentTick;
            jumpInputData.sequence = sequence++;
            jumpInputData.entityId = Entities.MyEntityID;

            CS_NotifyJumpInputData notifyJumpInputData = new CS_NotifyJumpInputData();
            notifyJumpInputData.jumpInputData = jumpInputData;

            RoomNetwork.Instance.Send(notifyJumpInputData, 0, instant: true);

            //  클라에서 인풋 선 처리 (서버에 도달했을 때 예측해서)
            Predict();
        }

        jumpInputData = null;
    }

    private void Predict()
    {
        var entity = Entities.Get<LOPMonoEntityBase>(jumpInputData.entityId);

        entity.BehaviorController.Jump(1, Vector3.up, Behavior.Jump.JumpType.AddForce);
    }

    private void OnJumpInputData(JumpInputData jumpInputData)
    {
        //  가장 마지막 인풋만 처리한다.
        this.jumpInputData = jumpInputData;
    }

    private void OnSC_ProcessInputData(SC_ProcessInputData processInputData)
    {
        if (processInputData.type == "jump")
        {
            serverTickNSequences.Add(new TickNSequence(processInputData.tick, processInputData.sequence));
            if (serverTickNSequences.Count > 100)
            {
                serverTickNSequences.RemoveRange(0, serverTickNSequences.Count - 100);
            }
        }
    }

    private bool CanJump()
    {
        return Entities.MyCharacter.IsGrounded;
    }
}
