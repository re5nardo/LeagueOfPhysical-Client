using System.Collections;
using UnityEngine;

namespace Behavior
{
    public class Jump : BehaviorBase
    {
        private float normalizedPower;
        private Vector3 direction;

        #region BehaviorBase
        public override void Initialize(BehaviorParam behaviorParam)
        {
            base.Initialize(behaviorParam);

            var jumpBehaviorParam = behaviorParam as JumpBehaviorParam;

            normalizedPower = jumpBehaviorParam.normalizedPower;
            direction = jumpBehaviorParam.direction;
        }

        protected override void OnBehaviorStart()
        {
            base.OnBehaviorStart();

            //Entity.SendCommandToViews(new AnimatorSetBool("Jump", true));
        }

        protected override bool OnBehaviorUpdate()
        {
            if (Entity.EntityID != Entities.MyEntityID)
            {
                return true;
            }

            Entity.Rigidbody.AddForce(normalizedPower * direction.normalized * SubGameBase.Current.SubGameEnvironment.JumpPowerFactor, ForceMode.VelocityChange);

            return false;
        }
        #endregion
    }
}
