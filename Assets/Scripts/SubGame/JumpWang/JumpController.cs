using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;

namespace JumpWangController
{
    public class JumpController : MonoBehaviour
    {
        private JumpInput? jumpInput = null;

        private void Awake()
        {
            SceneMessageBroker.AddSubscriber<TickMessage.EarlyTick>(OnEarlyTick);
            SceneMessageBroker.AddSubscriber<JumpInput>(OnJumpInput);
        }

        private void OnDestroy()
        {
            SceneMessageBroker.RemoveSubscriber<TickMessage.EarlyTick>(OnEarlyTick);
            SceneMessageBroker.RemoveSubscriber<JumpInput>(OnJumpInput);
        }

        private void OnEarlyTick(TickMessage.EarlyTick message)
        {
            ProcessJumpInput();
        }

        private void OnJumpInput(JumpInput jumpInput)
        {
            //  가장 마지막 인풋만 처리한다.
            this.jumpInput = jumpInput;
        }

        private void ProcessJumpInput()
        {
            if (jumpInput == null)
            {
                return;
            }

            if (CanJump())
            {
                var entity = Entities.Get<LOPMonoEntityBase>(Entities.MyEntityID);

                entity.BehaviorController.Jump(jumpInput.Value.normalizedPower, jumpInput.Value.direction, Behavior.Jump.JumpType.AddForce);
            }

            jumpInput = null;
        }

        private bool CanJump()
        {
            return Entities.MyCharacter.IsGrounded;
        }
    }
}
