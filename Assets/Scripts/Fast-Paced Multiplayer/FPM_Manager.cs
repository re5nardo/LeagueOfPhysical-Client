using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;

//  Fast-Paced Multiplayer (FPM)
//  https://www.gabrielgambetta.com/client-server-game-architecture.html

public class FPM_Manager : MonoSingleton<FPM_Manager>, ITickable
{
    private Queue<PlayerMoveInput> playerMoveInputs = new Queue<PlayerMoveInput>();         //  플레이어 인풋
    private List<PlayerTransformInput> pendingInputs = new List<PlayerTransformInput>();    //  인풋에 의해 position & rotation 변화 값

    private long sequence = 0;
    private long lastInputConfirm = -1;
    private int lastProcessTick = -1;

    public Vector3 PendingPosition
    {
        get
        {
            var value = Vector3.zero;
            pendingInputs.ForEach(pending =>
            {
                value += pending.positionOffset;
            });

            return value;
        }
    }

    public Vector3 PendingRotation
    {
        get
        {
            var value = Vector3.zero;
            pendingInputs.ForEach(pending =>
            {
                value += pending.rotationOffset;
            });

            return value;
        }
    }

    public void Tick(int tick)
    {
        if (playerMoveInputs.Count == 0)
            return;

        var playerMoveInput = playerMoveInputs.Dequeue();

        var myCharacter = EntityManager.Instance.GetMyCharacter();

        AddPendingPositionInput(myCharacter.Position, playerMoveInput.inputData.ToVector3().normalized * Game.Current.TickInterval * myCharacter.MovementSpeed);

        float dest_y = Quaternion.LookRotation(playerMoveInput.inputData).eulerAngles.y;
        float mine_y = myCharacter.Rotation.y;

        AddPendingRotationInput(myCharacter.Rotation, new Vector3(0, dest_y - mine_y, 0));
    }

    public void ProcessInput(PlayerMoveInput playerMoveInput)
    {
        if (lastProcessTick == Game.Current.CurrentTick)
        {
            return;
        }

        var myCharacter = EntityManager.Instance.GetMyCharacter();

        playerMoveInput.tick = Game.Current.CurrentTick;
        playerMoveInput.sequence = sequence++;
        playerMoveInput.entityID = myCharacter.EntityID;
        playerMoveInput.position = myCharacter.Position;
        playerMoveInput.rotation = myCharacter.Rotation;

        CS_NotifyMoveInputData notifyMoveInputData = new CS_NotifyMoveInputData();
        notifyMoveInputData.m_PlayerMoveInput = playerMoveInput;

        //  Send to server (우선 서버에 전송)
        RoomNetwork.Instance.Send(notifyMoveInputData, PhotonNetwork.masterClient.ID, bInstant: true);

        //  클라이언트 선처리 (서버에 도달했을 때 예측해서)
        if (playerMoveInput.inputType == PlayerMoveInput.InputType.Press || playerMoveInput.inputType == PlayerMoveInput.InputType.Hold)
        {
            StartCoroutine(AddPlayerMoveInput(playerMoveInput, 0.03f/*latency*/));
        }
        else if (playerMoveInput.inputType == PlayerMoveInput.InputType.Release)
        {
            //  Stop move behavior
            //  ...
        }

        lastProcessTick = Game.Current.CurrentTick;
    }
    
    private IEnumerator AddPlayerMoveInput(PlayerMoveInput playerMoveInput, float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        playerMoveInputs.Enqueue(playerMoveInput);
    }

    private void AddPendingPositionInput(Vector3 position, Vector3 offset)
    {
        var myCharacter = EntityManager.Instance.GetMyCharacter();

        PlayerTransformInput playerTransformInput = new PlayerTransformInput();

        playerTransformInput.tick = Game.Current.CurrentTick;
        playerTransformInput.sequence = sequence++;
        playerTransformInput.entityID = myCharacter.EntityID;
        playerTransformInput.position = position;
        playerTransformInput.positionOffset = offset;
        playerTransformInput.rotation = myCharacter.Rotation;
        playerTransformInput.rotationOffset = Vector3.zero;

        pendingInputs.Add(playerTransformInput);
    }

    private void AddPendingRotationInput(Vector3 rotation, Vector3 offset)
    {
        var myCharacter = EntityManager.Instance.GetMyCharacter();

        PlayerTransformInput playerTransformInput = new PlayerTransformInput();

        playerTransformInput.tick = Game.Current.CurrentTick;
        playerTransformInput.sequence = sequence++;
        playerTransformInput.entityID = myCharacter.EntityID;
        playerTransformInput.position = myCharacter.Position;
        playerTransformInput.positionOffset = Vector3.zero;
        playerTransformInput.rotation = rotation;
        playerTransformInput.rotationOffset = offset;

        pendingInputs.Add(playerTransformInput);
    }

    public void OnPlayerMoveInputResponse(SC_PlayerMoveInputResponse playerMoveInputResponse)
    {
        Reconcile(playerMoveInputResponse);

        lastInputConfirm = playerMoveInputResponse.m_lLastProcessedSequence;
    }

    #region Reconcile
    private void Reconcile(SC_PlayerMoveInputResponse playerMoveInputResponse)
    {
        //  해당 틱에서 처리되었을 position & rotation pending input 제거하기 위해 +2
        pendingInputs.RemoveAll(pending => pending.sequence <= (playerMoveInputResponse.m_lLastProcessedSequence + 2));
    }
    #endregion
}
