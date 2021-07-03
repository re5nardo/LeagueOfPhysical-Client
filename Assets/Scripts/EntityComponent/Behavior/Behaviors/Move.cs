using System.Collections;
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

            if (Entity.ModelRigidbody.isKinematic)
            {
                Vector3 moved = toMove.normalized * Entity.MovementSpeed * DeltaTime * SubGameBase.Current.SubGameEnvironment.MoveSpeedFactor;

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
                var xz = toMove.XZ().normalized * Entity.MovementSpeed * SubGameBase.Current.SubGameEnvironment.MoveSpeedFactor;
                Entity.ModelRigidbody.velocity = new Vector3(xz.x, Entity.ModelRigidbody.velocity.y, xz.z);

                if (Entity.ModelRigidbody.velocity.XZ().magnitude >= Entity.MovementSpeed * SubGameBase.Current.SubGameEnvironment.MoveSpeedFactor)
                {
                    xz = Entity.ModelRigidbody.velocity.XZ().normalized * Entity.MovementSpeed * SubGameBase.Current.SubGameEnvironment.MoveSpeedFactor;

                    Entity.ModelRigidbody.velocity = new Vector3(xz.x, Entity.ModelRigidbody.velocity.y, xz.z);
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

            if (param[0] is SerializableVector3)
            {
                m_vec3Destination = ((SerializableVector3)param[0]).ToVector3();
            }
            else if (param[0] is Vector3)
            {
                m_vec3Destination = (Vector3)param[0];
            }

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

        public override void OnReceiveSynchronization(ISnap snap)
        {
            MoveSnap moveSnap = snap as MoveSnap;

            m_vec3Destination = moveSnap.destination;

            Entity.SendCommandToViews(new AnimatorSetBool("Move", true));
        }
    }
}
