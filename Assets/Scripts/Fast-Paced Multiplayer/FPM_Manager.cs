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
    private JumpInputData jumpInputData = null;

    private List<TickNSequence> clientTickNSequence = new List<TickNSequence>();
    private List<TickNSequence> serverTickNSequence = new List<TickNSequence>();
    private List<TransformHistory> transformHistories = new List<TransformHistory>();
    private List<EntityTransformSnap> entityTransformSnaps = new List<EntityTransformSnap>();

    private Vector3 earlyTickPosition;
    private Vector3 earlyTickRotation;

    private Vector3? reconcilePosition;

    private RoomProtocolDispatcher roomProtocolDispatcher = null;

    protected override void Awake()
    {
        base.Awake();

        MessageBroker.Default.Receive<PlayerMoveInput>().Subscribe(OnPlayerMoveInput).AddTo(this);
        MessageBroker.Default.Receive<JumpInputData>().Subscribe(OnJumpInputData).AddTo(this);

        TickPubSubService.AddSubscriber("EarlyTick", OnEarlyTick);
        TickPubSubService.AddSubscriber("LateTickEnd", OnLateTickEnd);

        roomProtocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();
        roomProtocolDispatcher[typeof(SC_ProcessInputData)] = OnSC_ProcessInputData;
        roomProtocolDispatcher[typeof(SC_Synchronization)] = OnSC_Synchronization;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        TickPubSubService.RemoveSubscriber("EarlyTick", OnEarlyTick);
        TickPubSubService.RemoveSubscriber("LateTickEnd", OnLateTickEnd);
    }

    private void OnPlayerMoveInput(PlayerMoveInput playerMoveInput)
    {
        //  가장 마지막 인풋만 처리한다. (유입되는 모든 인풋을 처리하면 update 인터벌과 tick 인터벌의 차이로 인해 인풋이 밀리는 결과 초래 - update가 1초 tick이 2초마다 수행되면 2배만큼 인풋이 늦게 처리됨)
        this.playerMoveInput = playerMoveInput;
    }

    private void OnJumpInputData(JumpInputData jumpInputData)
    {
        //  가장 마지막 인풋만 처리한다.
        this.jumpInputData = jumpInputData;
    }

    private void OnSC_ProcessInputData(IMessage msg)
    {
        SC_ProcessInputData processInputData = msg as SC_ProcessInputData;

        serverTickNSequence.Add(new TickNSequence(processInputData.tick, processInputData.sequence));
        if (serverTickNSequence.Count > 100)
        {
            serverTickNSequence.RemoveRange(0, serverTickNSequence.Count - 100);
        }
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

        ProcessJumpInputData();
        ProcessPlayerMoveInput();
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

    private void ProcessPlayerMoveInput()
    {
        if (playerMoveInput == null)
        {
            return;
        }

        clientTickNSequence.Add(new TickNSequence(Game.Current.CurrentTick, sequence));
        if (clientTickNSequence.Count > 100)
        {
            clientTickNSequence.RemoveRange(0, clientTickNSequence.Count - 100);
        }

        //  우선 서버에 전송
        playerMoveInput.tick = Game.Current.CurrentTick;
        playerMoveInput.sequence = sequence++;
        playerMoveInput.entityID = Entities.MyCharacter.EntityID;
        playerMoveInput.position = Entities.MyCharacter.Position;
        playerMoveInput.rotation = Entities.MyCharacter.Rotation;

        CS_NotifyMoveInputData notifyMoveInputData = new CS_NotifyMoveInputData();
        notifyMoveInputData.m_PlayerMoveInput = playerMoveInput;

        RoomNetwork.Instance.Send(notifyMoveInputData, PhotonNetwork.masterClient.ID, bInstant: true);

        //  클라에서 인풋 선 처리 (서버에 도달했을 때 예측해서)
        PredictMove();

        playerMoveInput = null;
    }

    private void ProcessJumpInputData()
    {
        if (jumpInputData == null)
        {
            return;
        }

        clientTickNSequence.Add(new TickNSequence(Game.Current.CurrentTick, sequence));
        if (clientTickNSequence.Count > 100)
        {
            clientTickNSequence.RemoveRange(0, clientTickNSequence.Count - 100);
        }

        //  우선 서버에 전송
        jumpInputData.tick = Game.Current.CurrentTick;
        jumpInputData.sequence = sequence++;
        jumpInputData.entityID = Entities.MyEntityID;

        CS_NotifyJumpInputData notifyJumpInputData = new CS_NotifyJumpInputData();
        notifyJumpInputData.jumpInputData = jumpInputData;

        RoomNetwork.Instance.Send(notifyJumpInputData, PhotonNetwork.masterClient.ID, bInstant: true);

        //  클라에서 인풋 선 처리 (서버에 도달했을 때 예측해서)
        PredictJump();
  
        jumpInputData = null;
    }

    private void PredictMove()
    {
        var entity = Entities.Get(playerMoveInput.entityID);

        if (playerMoveInput.inputType == PlayerMoveInput.InputType.Hold)
        {
            //if (CanMove())
            {
                var behaviorController = entity.GetEntityComponent<BehaviorController>();
                behaviorController.Move(entity.Position + playerMoveInput.inputData.ToVector3().normalized * Game.Current.TickInterval * 5 * (entity as Character).MovementSpeed);
            }
        }
        else if (playerMoveInput.inputType == PlayerMoveInput.InputType.Release)
        {
            var behaviorController = entity.GetEntityComponent<BehaviorController>();
            behaviorController.StopBehavior(Define.MasterData.BehaviorID.MOVE);
        }
    }

    private void PredictJump()
    {
        var entity = Entities.Get<MonoEntityBase>(jumpInputData.entityID);

        entity.ModelRigidbody.AddForce(Vector3.up * 1000, ForceMode.Impulse);
    }

    private void Reconcile()
    {
        if (entityTransformSnaps.Count == 0)
        {
            if (reconcilePosition.HasValue)
            {
                Entities.MyCharacter.Position = Vector3.Lerp(Entities.MyCharacter.Position, reconcilePosition.Value, 0.1f);
            }

            return;
        }

        //  서버에서 받은 마지막 snap을 사용하여 reconcile 한다.
        var entityTransformSnap = entityTransformSnaps[entityTransformSnaps.Count - 1];
        entityTransformSnaps.Clear();

        if (!serverTickNSequence.Exists(x => x.tick <= entityTransformSnap.Tick))
        {
            Debug.LogError("serverTickNSequence does not exist!");
            return;
        }

        var targets = serverTickNSequence.FindAll(x => x.tick <= entityTransformSnap.Tick);
        targets.Sort((x, y) =>
        {
            return x.sequence.CompareTo(y.sequence);
        });

        var target = targets[targets.Count - 1];
        int offset = entityTransformSnap.Tick - target.tick;

        if (!clientTickNSequence.Exists(x => x.sequence == target.sequence))
        {
            Debug.LogError("clientTickNSequence does not exist!");
            return;
        }

        var clientTarget = clientTickNSequence.Find(x => x.sequence == target.sequence);
        int clientTargetTick = clientTarget.tick + offset;

        if (clientTickNSequence.Exists(x => x.sequence > target.sequence))
        {
            var clientTargetNext = clientTickNSequence.Find(x => x.sequence > target.sequence);

            //clientTargetTick = Mathf.Min(clientTargetTick, clientTargetNext.tick - 1);
        }

        var historiesToApply = transformHistories.FindAll(x => x.tick > clientTargetTick);

        Vector3 sumOfPosition = Vector3.zero;
        Vector3 sumOfRotation = Vector3.zero;

        historiesToApply?.ForEach(history =>
        {
            sumOfPosition += history.positionChange;
            sumOfRotation += history.rotationChange;
        });

        //  조금 더 고도화를 해야 할 것 같은데...
        Entities.MyCharacter.Position = Vector3.Lerp(Entities.MyCharacter.Position, entityTransformSnap.position + sumOfPosition, 0.1f);    //  이 부분때문에 서로 밀 때 서버와 클라 위치가 많이 달라보이나?
        //Entities.MyCharacter.Position = entityTransformSnap.position + sumOfPosition;
        Entities.MyCharacter.Rotation = entityTransformSnap.rotation + sumOfRotation;

        reconcilePosition = entityTransformSnap.position + sumOfPosition;
    }
}

//  최종적으로 클라 서버 위치 동일하게 맞추는것 보장해야 하는데 위 0.25 어떻게??
//  Reconcile setting해놓고 값 일치할때까지 함수 호출되는 식으로 (스타트 코루틴처럼)
