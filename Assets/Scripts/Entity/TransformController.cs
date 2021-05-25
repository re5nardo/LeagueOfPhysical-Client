using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using Entity;

public class TransformController : MonoBehaviour
{
    private MonoEntityBase monoEntityBase = null;
    private MonoEntityBase MonoEntityBase => monoEntityBase ?? (monoEntityBase = GetComponent<MonoEntityBase>());
    
    private EntityTransformSynchronization transformSynchronization = null;
    private EntityTransformSynchronization TransformSynchronization => transformSynchronization ?? (transformSynchronization = GetComponent<EntityTransformSynchronization>());

    private float syncTime;
    private float offset = 0.06f;

    private void Awake()
    {
        DebugCommandPubSubService.AddSubscriber("TransformDelayFactor", OnTransformDelayFactor);
    }

    private void OnDestroy()
    {
        DebugCommandPubSubService.RemoveSubscriber("TransformDelayFactor", OnTransformDelayFactor);
    }

    private void OnTransformDelayFactor(object[] objects)
    {
        offset = (float)objects[0];
    }
  
    private void LateUpdate()
    {
        SyncedMovement();
    }

    private void SyncedMovement()
    {
        syncTime = Game.Current.GameTime - offset;

        EntityTransformSnap before = TransformSynchronization.entityTransformSnaps.FindLast(x => x.GameTime <= syncTime);
        EntityTransformSnap next = TransformSynchronization.entityTransformSnaps.Find(x => x.GameTime >= syncTime);

        if (before != null && next != null)
        {
            float t = (before.GameTime == next.GameTime) ? 0 : (syncTime - before.GameTime) / (next.GameTime - before.GameTime);
            if (float.IsNaN(t))
            {
                Debug.LogWarning($"syncTime : {syncTime}, before.m_GameTime : {before.GameTime}, next.m_GameTime : {next.GameTime}");
                return;
            }

            MonoEntityBase.Position = Vector3.Lerp(before.position, next.position, t);
            MonoEntityBase.Rotation = Quaternion.Lerp(Quaternion.Euler(before.rotation), Quaternion.Euler(next.rotation), t).eulerAngles;
            MonoEntityBase.Velocity = Vector3.Lerp(before.velocity, next.velocity, t);
            MonoEntityBase.AngularVelocity = Vector3.Lerp(before.angularVelocity, next.angularVelocity, t);
        }
        else if (before != null)
        {
            var angle = Vector3.Angle(MonoEntityBase.Velocity, before.velocity);
            var factor = Mathf.Lerp(1, 0.5f, angle / 180);
            factor = 1;

            float elapsed = syncTime - before.GameTime;

            MonoEntityBase.Rotation = Util.Math.RotateClamp(before.rotation, before.angularVelocity, elapsed, before.destRotation);
            MonoEntityBase.Position = before.position + before.velocity.ToVector3() * elapsed * factor;
            MonoEntityBase.Velocity = before.velocity;
            MonoEntityBase.AngularVelocity = before.angularVelocity;
        }
    }
}
