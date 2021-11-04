using System.Collections.Generic;
using UnityEngine;
using Entity;

namespace Behavior
{
    public class Die : BehaviorBase
    {
        #region BehaviorBase
        protected override void OnBehaviorStart()
        {
            base.OnBehaviorStart();

            Entity.Rigidbody.detectCollisions = false;
        }

        protected override bool OnBehaviorUpdate()
        {
            return CurrentUpdateTime < MasterData.lifespan;
        }

        protected override void OnBehaviorEnd()
        {
            base.OnBehaviorEnd();

            Entity.Rigidbody.detectCollisions = true;
        }
        #endregion
    }
}
