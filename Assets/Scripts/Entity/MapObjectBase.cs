using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public abstract class MapObjectBase : LOPMonoEntityBase
    {
        [SerializeField] private GameObject model = null;

        protected override void Awake()
        {
            base.Awake();

            EntityCreationData entityCreationData = new EntityCreationData();
            entityCreationData.entityId = EntityManager.Instance.GenerateLocalEntityID();
            entityCreationData.position = model.transform.position;
            entityCreationData.rotation = model.transform.rotation.eulerAngles;
            entityCreationData.velocity = Vector3.zero;
            entityCreationData.angularVelocity = Vector3.zero;
            entityCreationData.entityType = EntityType.MapObject;
            entityCreationData.entityRole = EntityRole.NPC;
            entityCreationData.ownerId = "local";

            Initialize(entityCreationData);

            EntityManager.Instance.RegisterEntity(this);
        }

        protected override void InitEntity()
        {
            base.InitEntity();

            Rigidbody.isKinematic = true;
            Rigidbody.useGravity = false;
        }

        protected override void InitEntityComponents()
        {
            base.InitEntityComponents();

            EntityBasicView = AttachEntityComponent<EntityBasicView>();
        }

        protected override void OnInitialize(EntityCreationData entityCreationData)
        {
            base.OnInitialize(entityCreationData);

            EntityBasicView.SetModel(model);
        }
    }
}
