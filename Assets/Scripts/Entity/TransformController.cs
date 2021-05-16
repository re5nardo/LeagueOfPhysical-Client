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
    private float offset = 0.05f;

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
        }
        else if (before != null)
        {
            float elapsed = syncTime - before.GameTime;

            MonoEntityBase.Rotation = Util.Math.RotateClamp(before.rotation, before.angularVelocity, elapsed, before.destRotation);
            MonoEntityBase.Position = before.position + before.velocity.ToVector3() * elapsed;
        }
    }

    public void SetRotation(Vector3 rotation)
    {
        StopCoroutine("SetRotationRoutine");
        StartCoroutine("SetRotationRoutine", rotation);
    }

    private IEnumerator SetRotationRoutine(Vector3 rotation)
    {
        while (MonoEntityBase.Rotation != rotation)
        {
            var angle = Vector3.SignedAngle(MonoEntityBase.Rotation.Forward(), rotation.Forward(), Vector3.up);

            var angularVelocity = new Vector3(0, (angle > 0 ? 1 : -1) * 360 * 3, 0);

            MonoEntityBase.Rotation = Util.Math.RotateClamp(MonoEntityBase.Rotation, angularVelocity, Time.deltaTime, rotation);

            yield return null;
        }
    }
}
