using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class RotateObject : MapObjectBase
    {
        [SerializeField] private Vector3 startRotation;
        [SerializeField] private Vector3 angularVelocity;
        [SerializeField] private float timeOffset;

        public override float MovementSpeed => 0;
        public override float FactoredMovementSpeed => MovementSpeed * LOP.Game.Current.GameManager.MapData.mapEnvironment.MoveSpeedFactor;

        protected override void InitEntity()
        {
            base.InitEntity();

            Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }

        protected override void OnInitialize(EntityCreationData entityCreationData)
        {
            base.OnInitialize(entityCreationData);

            AngularVelocity = angularVelocity;

            BehaviorController.StartBehavior(new ContinuousRotationBehaviorParam(Define.MasterData.BehaviorId.ContinuousRotation, startRotation, timeOffset));
        }
    }
}
