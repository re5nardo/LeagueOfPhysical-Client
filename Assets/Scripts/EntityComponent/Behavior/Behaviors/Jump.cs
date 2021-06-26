using System.Collections;
using UnityEngine;

namespace Behavior
{
    public class Jump : BehaviorBase
    {
        #region BehaviorBase
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

            Entity.ModelRigidbody.AddForce(Vector3.up * 1000, ForceMode.Impulse);

            return false;
        }
        #endregion
    }
}
