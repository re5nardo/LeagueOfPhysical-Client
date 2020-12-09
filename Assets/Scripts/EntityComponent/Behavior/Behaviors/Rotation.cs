﻿using System.Collections;
using UnityEngine;
using GameFramework;

namespace Behavior
{
    public class Rotation : BehaviorBase
    {
        private Vector3 m_vec3Direction;

        // Angular speed in degrees per sec.
        private float m_fAngularSpeed = 360 * 2;

        public const float ANGULAR_SPEED = 360 * 2;

        #region BehaviorBase
        protected override bool OnBehaviorUpdate()
        {
            float toRotate = Vector3.SignedAngle(m_vec3Direction, Entity.Forward, Vector3.up);
            if (toRotate == 0)
            {
                return false;
            }

            Entity.AngularVelocity = new Vector3(0, (toRotate > 0 ? 1 : -1) * m_fAngularSpeed, 0);

            float rotated = toRotate * m_fAngularSpeed * DeltaTime;

            if (Util.Approximately(toRotate, rotated) || Mathf.Abs(toRotate) <= Mathf.Abs(rotated))
            {
                Entity.Rotation = Quaternion.LookRotation(m_vec3Direction).eulerAngles;

                return true;
            }
            else
            {
                Entity.Rotation = new Vector3(Entity.Rotation.x, (Entity.Rotation.y + rotated) % 360, Entity.Rotation.z);

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
    }
}
