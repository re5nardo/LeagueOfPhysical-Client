using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UniRx;
using Entity;

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

        clientTickNSequences.Add(new TickNSequence(Game.Current.CurrentTick, sequence));
        if (clientTickNSequences.Count > 100)
        {
            clientTickNSequences.RemoveRange(0, clientTickNSequences.Count - 100);
        }

        //  우선 서버에 전송
        jumpInputData.tick = Game.Current.CurrentTick;
        jumpInputData.sequence = sequence++;
        jumpInputData.entityID = Entities.MyEntityID;

        CS_NotifyJumpInputData notifyJumpInputData = new CS_NotifyJumpInputData();
        notifyJumpInputData.jumpInputData = jumpInputData;

        RoomNetwork.Instance.Send(notifyJumpInputData, PhotonNetwork.masterClient.ID, bInstant: true);

        //  클라에서 인풋 선 처리 (서버에 도달했을 때 예측해서)
        Predict();

        jumpInputData = null;
    }

    private void Predict()
    {
        var entity = Entities.Get<MonoEntityBase>(jumpInputData.entityID);

        entity.ModelRigidbody.AddForce(Vector3.up * 1000, ForceMode.Impulse);
    }

    private void OnJumpInputData(JumpInputData jumpInputData)
    {
        //  가장 마지막 인풋만 처리한다.
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

    public void Reconcile(EntityTransformSnap entityTransformSnap, ref Vector3 sumOfPosition)
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
            sumOfPosition += new Vector3(0, history.positionChange.y, 0);
        }
    }
}
