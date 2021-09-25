using System.Collections.Generic;
using UnityEngine;
using Entity;
using EntityMessage;

namespace Behavior
{
    public class MeleeAttack : BehaviorBase
    {
        #region ClassParams
        private float m_fLifespan;
        private float m_fAttackTime;
        private float m_fMaxHalfAngle = 45f;
        private float m_fRadius = 2f;
        #endregion

        #region BehaviorBase
        protected override void OnBehaviorStart()
        {
            base.OnBehaviorStart();

			Entity.MessageBroker.Publish(new AnimatorSetTrigger("Attack"));
		}

        protected override bool OnBehaviorUpdate()
        {
            return true;
        }

        public override void Initialize(BehaviorParam behaviorParam)
        {
            base.Initialize(behaviorParam);

            m_fLifespan = MasterData.Lifespan;
            m_fAttackTime = float.Parse(MasterData.ClassParams.Find(x => x.Contains("AttackTime")).Split(':')[1]);
        }
        #endregion
    }
}
