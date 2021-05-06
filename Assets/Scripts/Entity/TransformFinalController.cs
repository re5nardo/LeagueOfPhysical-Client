using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using Entity;

public class TransformFinalController : MonoBehaviour
{
    private MonoEntityBase monoEntityBase = null;
    private MonoEntityBase MonoEntityBase => monoEntityBase ?? (monoEntityBase = GetComponent<MonoEntityBase>());
    
    private EntityTransformSynchronization transformSynchronization = null;
    private EntityTransformSynchronization TransformSynchronization => transformSynchronization ?? (transformSynchronization = GetComponent<EntityTransformSynchronization>());

    private void LateUpdate()
    {
        SyncedMovement();
    }

    private void SyncedMovement()
    {
        EntityTransformSnap before = TransformSynchronization.entityTransformSnaps.FindLast(x => x.GameTime <= Game.Current.GameTime);
        EntityTransformSnap next = TransformSynchronization.entityTransformSnaps.Find(x => x.GameTime >= Game.Current.GameTime);

        if (before == null)
        {
            return;
        }
      
        if (before != null && next != null)
        {
            float t = (before.GameTime == next.GameTime) ? 0 : (Game.Current.GameTime - before.GameTime) / (next.GameTime - before.GameTime);
            if (float.IsNaN(t))
            {
                Debug.LogWarning($"GameTime : {Game.Current.GameTime}, before.m_GameTime : {before.GameTime}, next.m_GameTime : {next.GameTime}");
                return;
            }

            MonoEntityBase.Position = Vector3.Lerp(before.position, next.position, t);
            MonoEntityBase.Rotation = Quaternion.Lerp(Quaternion.Euler(before.rotation), Quaternion.Euler(next.rotation), t).eulerAngles;
        }
        else
        {
            float elapsed = Game.Current.GameTime - before.GameTime;

            MonoEntityBase.Rotation = Util.Math.RotateClamp(before.rotation, before.angularVelocity, elapsed, before.destRotation);
            MonoEntityBase.Position = before.position + before.velocity.ToVector3() * elapsed;
        }
    }
}
