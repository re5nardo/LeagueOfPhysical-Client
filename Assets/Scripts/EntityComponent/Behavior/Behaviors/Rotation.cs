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
            float dest_y = Quaternion.LookRotation(m_vec3Direction).eulerAngles.y;
            float mine_y = Entity.Rotation.y;

            float toRotate = dest_y - mine_y;
            if (Util.Approximately(toRotate, 0))
                return false;

            int sign = GetRotationSign(Entity.Forward, m_vec3Direction);
            if (sign > 0)
            {
                if (dest_y < mine_y)
                {
                    toRotate = (dest_y + 360) - mine_y;
                }
            }
            else
            {
                if (dest_y > mine_y)
                {
                    toRotate = dest_y - (mine_y + 360);
                }
            }

            Entity.AngularVelocity = new Vector3(0, sign * ANGULAR_SPEED, 0);

            float rotated = Entity.AngularVelocity.y * DeltaTime;

            var startRotation = Entity.Rotation;

            if (Util.Approximately(toRotate, rotated) || Mathf.Abs(toRotate) <= Mathf.Abs(rotated))
            {
                Entity.Rotation = Quaternion.LookRotation(m_vec3Direction).eulerAngles;

                FPM_Manager.Instance.AddRotationInput(startRotation, Entity.Rotation - startRotation);

                return false;
            }
            else
            {
                Entity.Rotation = new Vector3(Entity.Rotation.x, (Entity.Rotation.y + rotated) % 360, Entity.Rotation.z);

                FPM_Manager.Instance.AddRotationInput(startRotation, Entity.Rotation - startRotation);

                return true;
            }
        }

        protected override void OnBehaviorEnd()
        {
            base.OnBehaviorEnd();

            Entity.AngularVelocity = Vector3.zero;
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

        private int GetRotationSign(Vector3 cur, Vector3 dest)
        {
            cur = new Vector3(cur.x, 0, cur.z);
            dest = new Vector3(dest.x, 0, dest.z);

            return Vector3.Cross(cur, dest).y > 0 ? 1 : -1;
        }
    }
}
