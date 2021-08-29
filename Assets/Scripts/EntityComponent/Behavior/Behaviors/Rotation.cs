using System.Collections;
using UnityEngine;

namespace Behavior
{
    public class Rotation : BehaviorBase
    {
        private Vector3 m_vec3Direction;

        // Angular speed in degrees per sec.
        private float m_fAngularSpeed = 360 * 2;

        #region BehaviorBase
        protected override bool OnBehaviorUpdate()
        {
            if (Entity.EntityID != Entities.MyEntityID)
            {
                return true;
            }

            float toRotate = Vector3.SignedAngle(Entity.Forward, m_vec3Direction, Vector3.up);
            if (toRotate == 0)
            {
                return false;
            }

            var angularVelocity = new Vector3(0, (toRotate > 0 ? 1 : -1) * m_fAngularSpeed, 0);

            float rotated = angularVelocity.y * DeltaTime;

            if (Util.Approximately(toRotate, rotated) || Mathf.Abs(toRotate) <= Mathf.Abs(rotated))
            {
                Entity.Rotation = Quaternion.LookRotation(m_vec3Direction).eulerAngles;
                return false;
            }
            else
            {
                Entity.Rotation = new Vector3(Entity.Rotation.x, (Entity.Rotation.y + rotated) % 360, Entity.Rotation.z);
                return true;
            }
        }

        public override void SetData(int nBehaviorMasterID, params object[] param)
        {
            base.SetData(nBehaviorMasterID);

            m_vec3Direction = (Vector3)param[0];
        }
        #endregion

        public void SetDirection(Vector3 vec3Direction)
        {
            m_vec3Direction = vec3Direction;
        }

        public Vector3 GetDestination()
        {
            return Quaternion.LookRotation(m_vec3Direction).eulerAngles;
        }
    }
}
