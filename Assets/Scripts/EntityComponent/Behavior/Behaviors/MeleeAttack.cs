using System.Collections.Generic;
using UnityEngine;
using Entity;
using EntityCommand;

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

			Entity.SendCommandToViews(new AnimatorSetTrigger("Attack"));
		}

        protected override bool OnBehaviorUpdate()
        {
            if (LastUpdateTime < m_fAttackTime && m_fAttackTime <= CurrentUpdateTime)
            {
				//	Do nothing
			}

			return CurrentUpdateTime < m_fLifespan;
        }

        public override void SetData(int nBehaviorMasterID, params object[] param)
        {
            base.SetData(nBehaviorMasterID);

            m_fLifespan = MasterData.Lifespan;
            m_fAttackTime = float.Parse(MasterData.ClassParams.Find(x => x.Contains("AttackTime")).Split(':')[1]);
        }
        #endregion
    }
}
