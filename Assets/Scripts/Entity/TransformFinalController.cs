using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class TransformFinalController : MonoBehaviour
{
    private EntityBasicView entityBasicView = null;
    private EntityBasicView EntityBasicView => entityBasicView ?? (entityBasicView = GetComponent<EntityBasicView>());
    
    private EntityTransformSynchronization transformSynchronization = null;
    private EntityTransformSynchronization TransformSynchronization => transformSynchronization ?? (transformSynchronization = GetComponent<EntityTransformSynchronization>());
    
    private Vector3 lastPosition;
    private EntityTransformSnap lastSnap = null;

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

            EntityBasicView.Position = Vector3.Lerp(before.position, next.position, t);
            EntityBasicView.Rotation = Quaternion.Lerp(Quaternion.Euler(before.rotation), Quaternion.Euler(next.rotation), t).eulerAngles;
        }
        else
        {
            float elapsed = Game.Current.GameTime - before.GameTime;

            EntityBasicView.Rotation = Util.Math.RotateClamp(before.rotation, before.angularVelocity, elapsed, before.destRotation);

            if (lastSnap == null || lastSnap == before)
            {
                EntityBasicView.Position = before.position + before.velocity.ToVector3() * elapsed;
            }
            else
            {
                //  새로운 패킷이면 smoothing 작업을 해준다. (다음 프레임 예상 값과 현재 값의 중간 값)
                float nextElapsed = Game.Current.GameTime + Time.deltaTime - before.GameTime;

                Vector3 expectedPosition = before.position + before.velocity.ToVector3() * nextElapsed;

                EntityBasicView.Position = Vector3.Lerp(lastPosition, expectedPosition, 0.5f);
            }
        }

        lastPosition = EntityBasicView.Position;
        lastSnap = before;
    }
}

//  MoveClamp 필요..?
