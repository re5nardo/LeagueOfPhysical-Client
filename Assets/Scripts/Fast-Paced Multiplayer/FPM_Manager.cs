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
    private int lastProcessTick = -1;

    protected override void Awake()
    {
        base.Awake();

        MessageBroker.Default.Receive<PlayerMoveInput>().Subscribe(ProcessInput).AddTo(this);
    }

    public void ProcessInput(PlayerMoveInput playerMoveInput)
    {
        if (lastProcessTick == Game.Current.CurrentTick)
        {
            return;
        }

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
        if (playerMoveInput.inputType == PlayerMoveInput.InputType.Hold)
        {
            //StartCoroutine(AddPlayerMoveInput(playerMoveInput, LOP.Room.Instance.Latency));
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

        //if (CanMove())
        {
            Character character = Entities.Get<Character>(playerMoveInput.entityID);
            var behaviorController = character.GetComponent<BehaviorController>();
            behaviorController.Move(character.Position + playerMoveInput.inputData.ToVector3().normalized * Game.Current.TickInterval * 5 * character.MovementSpeed);
        }
    }
}
