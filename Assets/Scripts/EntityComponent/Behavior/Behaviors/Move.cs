﻿using System.Collections;
using UnityEngine;
using GameFramework;
using EntityCommand;

namespace Behavior
{
    public class Move : BehaviorBase
    {
        private Vector3 m_vec3Destination;
        private int remainCount = 3;

        #region BehaviorBase
        protected override void OnBehaviorStart()
        {
            base.OnBehaviorStart();

            Entity.SendCommandToViews(new AnimatorSetBool("Move", true));
        }

        protected override bool OnBehaviorUpdate()
        {
            if (Entity.EntityID != Entities.MyEntityID)
            {
                return true;
            }

            Vector3 toMove = m_vec3Destination.XZ() - Entity.Position.XZ();

            if (Entity.Rigidbody.isKinematic)
            {
                Vector3 moved = toMove.normalized * Entity.FactoredMovementSpeed * DeltaTime;

                if (Util.Approximately(toMove.sqrMagnitude, moved.sqrMagnitude) || toMove.sqrMagnitude <= moved.sqrMagnitude)
                {
                    Entity.Position = m_vec3Destination;
                    return false;
                }
                else
                {
                    Entity.Position += moved;
                    return true;
                }
            }
            else
            {
                var xz = toMove.XZ().normalized * Entity.FactoredMovementSpeed;
                Entity.Rigidbody.velocity = new Vector3(xz.x, Entity.Rigidbody.velocity.y, xz.z);

                if (Entity.Rigidbody.velocity.XZ().magnitude >= Entity.FactoredMovementSpeed)
                {
                    xz = Entity.Rigidbody.velocity.XZ().normalized * Entity.FactoredMovementSpeed;

                    Entity.Rigidbody.velocity = new Vector3(xz.x, Entity.Rigidbody.velocity.y, xz.z);
                }

                return --remainCount > 0;
            }
        }

        protected override void OnBehaviorEnd()
        {
            base.OnBehaviorEnd();

            Entity.SendCommandToViews(new AnimatorSetBool("Move", false));
        }

        public override void SetData(int nBehaviorMasterID, params object[] param)
        {
            base.SetData(nBehaviorMasterID);

            m_vec3Destination = (Vector3)param[0];

            remainCount = 3;
        }
        #endregion

        public void SetDestination(Vector3 vec3Destination)
        {
            m_vec3Destination = vec3Destination;
            remainCount = 3;
        }

        public Vector3 GetDestination()
        {
            return m_vec3Destination;
        }
    }
}
