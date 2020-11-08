using UnityEngine;
using Entity;
using EntityCommand;

namespace Behavior
{
    public class RangeAttack : BehaviorBase
    {
        #region ClassParams
        private float m_fLifespan;
        private float m_fAttackTime;
        private int m_nTargetProjectileID;
        private float m_fTargetProjectileHeight;
        private float m_fTargetProjectileLifespan;
        #endregion

        #region BehaviorBase
        protected override void OnBehaviorStart()
        {
            base.OnBehaviorStart();

			Entity.SendCommandToViews(new AnimatorSetTrigger("Attack"));
		}

        protected override bool OnBehaviorUpdate()
        {
            return true;
        }

        public override void SetData(int nBehaviorMasterID, params object[] param)
        {
            base.SetData(nBehaviorMasterID);

            m_fLifespan = MasterData.Lifespan;
            m_fAttackTime = float.Parse(MasterData.ClassParams.Find(x => x.Contains("AttackTime")).Split(':')[1]);
            m_nTargetProjectileID = int.Parse(MasterData.ClassParams.Find(x => x.Contains("ProjectileID")).Split(':')[1]);
            m_fTargetProjectileHeight = float.Parse(MasterData.ClassParams.Find(x => x.Contains("ProjectileHeight")).Split(':')[1]);
            m_fTargetProjectileLifespan = float.Parse(MasterData.ClassParams.Find(x => x.Contains("ProjectileLifespan")).Split(':')[1]);
        }
        #endregion
    }
}
