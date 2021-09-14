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

    private RoomProtocolDispatcher roomProtocolDispatcher = null;

    private void Awake()
    {
        MessageBroker.Default.Receive<JumpInputData>().Subscribe(OnJumpInputData).AddTo(this);

        roomProtocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();
        roomProtocolDispatcher[typeof(SC_ProcessInputData)] = OnSC_ProcessInputData;
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

            //  �켱 ������ ����
            jumpInputData.tick = Game.Current.CurrentTick;
            jumpInputData.sequence = sequence++;
            jumpInputData.entityId = Entities.MyEntityID;

            CS_NotifyJumpInputData notifyJumpInputData = new CS_NotifyJumpInputData();
            notifyJumpInputData.jumpInputData = jumpInputData;

            RoomNetwork.Instance.Send(notifyJumpInputData, 0, instant: true);

            //  Ŭ�󿡼� ��ǲ �� ó�� (������ �������� �� �����ؼ�)
            Predict();
        }

        jumpInputData = null;
    }

    private void Predict()
    {
        var entity = Entities.Get<LOPMonoEntityBase>(jumpInputData.entityId);

        var behaviorController = entity.GetEntityComponent<BehaviorController>();
        behaviorController.Jump();
    }

    private void OnJumpInputData(JumpInputData jumpInputData)
    {
        //  ���� ������ ��ǲ�� ó���Ѵ�.
        this.jumpInputData = jumpInputData;
    }

    private void OnSC_ProcessInputData(IMessage msg)
    {
        SC_ProcessInputData processInputData = msg as SC_ProcessInputData;

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
