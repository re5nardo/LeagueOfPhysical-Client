using System.Collections;
using UnityEngine;
using GameFramework;
using EntityCommand;

namespace Behavior
{
    public class Move : BehaviorBase
    {
        private Vector3 m_vec3Destination;

        #region BehaviorBase
        protected override void OnBehaviorStart()
        {
            base.OnBehaviorStart();

            Entity.SendCommandToViews(new AnimatorSetBool("Move", true));
        }

        protected override bool OnBehaviorUpdate()
        {
            return true;
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
        }
        #endregion

        public void SetDestination(Vector3 vec3Destination)
        {
            m_vec3Destination = vec3Destination;
        }

        public Vector3 GetDestination()
        {
            return m_vec3Destination;
        }
    }
}
