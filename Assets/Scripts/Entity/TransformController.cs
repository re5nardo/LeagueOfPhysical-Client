using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using NetworkModel.Mirror;
using GameFramework;

public class TransformController : MonoBehaviour
{
    private LOPMonoEntityBase entity;
    private RoomProtocolDispatcher roomProtocolDispatcher;
    private EntityTransformSnap entityTransformSnap = new EntityTransformSnap();
    private List<EntityTransformSnap> entityTransformSnaps = new List<EntityTransformSnap>();
    private AverageQueue latencies = new AverageQueue();

    private void Awake()
    {
        entity = GetComponent<LOPMonoEntityBase>();

        roomProtocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();
        roomProtocolDispatcher[typeof(SC_Synchronization)] = OnSC_Synchronization;

        TickPubSubService.AddSubscriber("LateTickEnd", OnLateTickEnd);
    }

    private void OnDestroy()
    {
        TickPubSubService.RemoveSubscriber("LateTickEnd", OnLateTickEnd);
    }

    private void OnSC_Synchronization(IMessage msg)
    {
        if (entity.HasAuthority)
        {
            return;
        }

        SC_Synchronization synchronization = msg as SC_Synchronization;

        synchronization.listSnap?.ForEach(snap =>
        {
            if (snap is EntityTransformSnap entityTransformSnap && entityTransformSnap.entityId == entity.EntityID)
            {
                entityTransformSnaps.Add(entityTransformSnap);

                if (entityTransformSnaps.Count > 100)
                {
                    entityTransformSnaps.RemoveRange(0, entityTransformSnaps.Count - 100);
                }

                latencies.Add((float)(Mirror.NetworkTime.time - entityTransformSnap.GameTime));
            }
        });
    }

    private void OnLateTickEnd(int tick)
    {
        if (entity.HasAuthority)
        {
            var synchronization = ObjectPool.Instance.GetObject<CS_Synchronization>();
            synchronization.listSnap.Add(entityTransformSnap.Set(entity));

            RoomNetwork.Instance.Send(synchronization, 0, instant: true);
        }
    }

    private void LateUpdate()
    {
        if (!entity.HasAuthority)
        {
            SyncTransform();
        }
    }

    private void SyncTransform()
    {
        if (entityTransformSnaps.Count == 0)
        {
            return;
        }

        float syncTime = (float)Mirror.NetworkTime.time - latencies.Average - 0.01f;

        EntityTransformSnap before = entityTransformSnaps.FindLast(x => x.GameTime <= syncTime);
        EntityTransformSnap next = entityTransformSnaps.Find(x => x.GameTime >= syncTime);

        if (before != null && next != null)
        {
            float t = (before.GameTime == next.GameTime) ? 0 : (syncTime - before.GameTime) / (next.GameTime - before.GameTime);

            entity.Position = Vector3.Lerp(before.position, next.position, t);
            entity.Rotation = Quaternion.Lerp(Quaternion.Euler(before.rotation), Quaternion.Euler(next.rotation), t).eulerAngles;
            entity.Velocity = Vector3.Lerp(before.velocity, next.velocity, t);
            entity.AngularVelocity = Vector3.Lerp(before.angularVelocity, next.angularVelocity, t);
        }
        else if (before != null)
        {
            float elapsed = syncTime - before.GameTime;

            entity.Position = before.position + before.velocity * elapsed;
            entity.Rotation = before.rotation;
            entity.Velocity = before.velocity;
            entity.AngularVelocity = before.angularVelocity;
        }
    }
}
