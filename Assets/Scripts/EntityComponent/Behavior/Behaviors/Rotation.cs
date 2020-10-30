using System.Collections;
using UnityEngine;
using GameFramework;

namespace Behavior
{
    public class Rotation : BehaviorBase
    {
        private Vector3 m_vec3Direction;

        // Angular speed in degrees per sec.
        public const float ANGULAR_SPEED = 360 * 2;

        #region BehaviorBase
        protected override bool OnBehaviorUpdate()
        {
            return true;
        }

        public override void SetData(int nBehaviorMasterID, params object[] param)
        {
            base.SetData(nBehaviorMasterID);

            if (param[0] is SerializableVector3)
            {
                m_vec3Direction = ((SerializableVector3)param[0]).ToVector3();
            }
            else if (param[0] is Vector3)
            {
                m_vec3Direction = (Vector3)param[0];
            }
        }
        #endregion

        public void SetDirection(Vector3 vec3Direction)
        {
            m_vec3Direction = vec3Direction;
        }

        private int GetRotationSign(Vector3 cur, Vector3 dest)
        {
            cur = new Vector3(cur.x, 0, cur.z);
            dest = new Vector3(dest.x, 0, dest.z);

            return Vector3.Cross(cur, dest).y > 0 ? 1 : -1;
        }
    }
}
