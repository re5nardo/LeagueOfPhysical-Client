using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UniRx;
using Entity;

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

        clientTickNSequences.Add(new TickNSequence(Game.Current.CurrentTick, sequence));
        if (clientTickNSequences.Count > 100)
        {
            clientTickNSequences.RemoveRange(0, clientTickNSequences.Count - 100);
        }

        //  �켱 ������ ����
        playerMoveInput.tick = Game.Current.CurrentTick;
        playerMoveInput.sequence = sequence++;
        playerMoveInput.entityID = Entities.MyEntityID;

        CS_NotifyMoveInputData notifyMoveInputData = new CS_NotifyMoveInputData();
        notifyMoveInputData.m_PlayerMoveInput = playerMoveInput;

        RoomNetwork.Instance.Send(notifyMoveInputData, PhotonNetwork.masterClient.ID, bInstant: true);

        //  Ŭ�󿡼� ��ǲ �� ó�� (������ �������� �� �����ؼ�)
        Predict();

        playerMoveInput = null;
    }

    private void Predict()
    {
        var entity = Entities.Get<Character>(playerMoveInput.entityID);

        if (playerMoveInput.inputType == PlayerMoveInput.InputType.Hold)
        {
            //if (CanMove())
            {
                var behaviorController = entity.GetEntityComponent<BehaviorController>();
                behaviorController.Move(entity.Position + playerMoveInput.inputData.ToVector3().normalized * Game.Current.TickInterval * 5 * entity.MovementSpeed);
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
        //  ���� ������ ��ǲ�� ó���Ѵ�. (���ԵǴ� ��� ��ǲ�� ó���ϸ� update ���͹��� tick ���͹��� ���̷� ���� ��ǲ�� �и��� ��� �ʷ� - update�� 1�� tick�� 2�ʸ��� ����Ǹ� 2�踸ŭ ��ǲ�� �ʰ� ó����)
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

    public void Reconcile(EntityTransformSnap entityTransformSnap, ref Vector3 sumOfPosition, ref Vector3 sumOfRotation)
    {
        if (!serverTickNSequences.Exists(x => x.tick <= entityTransformSnap.Tick))
        {
            //Debug.LogError("serverTickNSequences does not exist!");   //  �ܺο� ���� ����� pos ���� ���õǴ°� �ƴѰ� �̷���??
            return;
        }

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
        int clientTargetTick = clientTarget.tick + offset;

        if (clientTickNSequences.Exists(x => x.sequence == target.sequence + 1))
        {
            var clientTargetNext = clientTickNSequences.Find(x => x.sequence == target.sequence + 1);

            clientTargetTick = Mathf.Min(clientTargetTick, clientTargetNext.tick - 1);
        }

        var historiesToApply = FPM_Manager.Instance.TransformHistories.FindAll(x => x.tick > clientTargetTick);
        foreach (var history in historiesToApply)
        {
            sumOfPosition += history.positionChange.XZ();
            sumOfRotation += history.rotationChange;
        }
    }
}
