using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using GameFramework;

namespace JumpWangController
{
    public class MoveController : MonoBehaviour
    {
        private MoveInput? moveInput = null;

        private void Awake()
        {
            SceneMessageBroker.AddSubscriber<TickMessage.EarlyTick>(OnEarlyTick);
            SceneMessageBroker.AddSubscriber<MoveInput>(OnMoveInput);
        }

        private void OnDestroy()
        {
            SceneMessageBroker.RemoveSubscriber<TickMessage.EarlyTick>(OnEarlyTick);
            SceneMessageBroker.RemoveSubscriber<MoveInput>(OnMoveInput);
        }

        private void OnEarlyTick(TickMessage.EarlyTick message)
        {
            ProcessMoveInput();
        }

        private void OnMoveInput(MoveInput moveInput)
        {
            //  가장 마지막 인풋만 처리한다.
            this.moveInput = moveInput;
        }

        private void ProcessMoveInput()
        {
            if (moveInput == null)
            {
                return;
            }

            if (CanMove())
            {
                var entity = Entities.Get<LOPMonoEntityBase>(Entities.MyEntityID);

                var behaviorController = entity.GetEntityComponent<BehaviorController>();
                behaviorController.Move(entity.Position + moveInput.Value.direction.normalized * Game.Current.TickInterval * entity.FactoredMovementSpeed);
            }

            moveInput = null;
        }

        private bool CanMove()
        {
            return Entities.MyCharacter.IsGrounded;
        }
    }
}
