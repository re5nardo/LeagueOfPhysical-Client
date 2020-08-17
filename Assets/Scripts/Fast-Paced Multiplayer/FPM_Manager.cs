using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Entity;
using GameFramework;

//  Fast-Paced Multiplayer (FPM)
//  https://www.gabrielgambetta.com/client-server-game-architecture.html

public class FPM_Manager : MonoSingleton<FPM_Manager>
{
    private List<PlayerTransformInput> pendingInputs = new List<PlayerTransformInput>();

    private long sequence = 0;
    private long lastInputConfirm = -1;
    private int lastProcessTick = -1;

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
            //  delay (latency..)
            StartCoroutine(MoveAfterDelay(playerMoveInput.inputData, 0.02f));
        }
        else if (playerMoveInput.inputType == PlayerMoveInput.InputType.Release)
        {
            //  stop move behavior
            //  ...
        }

        lastProcessTick = Game.Current.CurrentTick;
    }
    
    private IEnumerator MoveAfterDelay(Vector3 input, float delay)
    {
        yield return new WaitForSeconds(delay);

        var myCharacter = EntityManager.Instance.GetMyCharacter();

        var behaviorController = myCharacter.GetComponent<BehaviorController>();
        behaviorController.MoveNRotation(myCharacter.Position + input.normalized * Game.Current.TickInterval * 3 * myCharacter.MovementSpeed);
    }

    public void AddMoveInput(Vector3 position, Vector3 offset)
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

    public void AddRotationInput(Vector3 rotation, Vector3 offset)
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
        Character character = EntityManager.Instance.GetMyCharacter();

        // Received the authoritative position of this client's entity
        character.Position = playerMoveInputResponse.m_Position;
        character.Rotation = playerMoveInputResponse.m_Rotation;
        
        int nIndex = 0;
        while (nIndex < pendingInputs.Count)
        {
            PlayerTransformInput input = pendingInputs[nIndex];

            if (input.sequence <= playerMoveInputResponse.m_lLastProcessedSequence)
            {
                // Already processed. Its effect is already taken into account into the world update
                // we just got, so we can drop it
                pendingInputs.RemoveAt(nIndex);
            }
            else
            {
                // Not processed by the server yet. Re-apply it
                character.Position += input.positionOffset;
                character.Rotation += input.rotationOffset;
                nIndex++;
            }
        }
    }
    #endregion
}
