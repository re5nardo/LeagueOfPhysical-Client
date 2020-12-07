using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using Entity;

public class TransformFinalController : MonoBehaviour
{
    private MonoEntityBase entity = null;
    private MonoEntityBase Entity => entity ?? (entity = GetComponent<MonoEntityBase>());

    private EntityTransformSynchronization transformSynchronization = null;
    private EntityTransformSynchronization TransformSynchronization => transformSynchronization ?? (transformSynchronization = GetComponent<EntityTransformSynchronization>());
    
    private Vector3 lastPosition;
    private Vector3 lastRotation;
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

            Entity.GetComponent<EntityBasicView>().Position = Vector3.Lerp(before.position, next.position, t);
            Entity.GetComponent<EntityBasicView>().Rotation = Quaternion.Lerp(Quaternion.Euler(before.rotation), Quaternion.Euler(next.rotation), t).eulerAngles;
        }
        else
        {
            if (lastSnap == null || lastSnap == before)
            {
                float elapsed = Game.Current.GameTime - before.GameTime;

                Entity.GetComponent<EntityBasicView>().Position = before.position + before.velocity.ToVector3() * elapsed;
                Entity.GetComponent<EntityBasicView>().Rotation = before.rotation + before.angularVelocity.ToVector3() * elapsed;
            }
            else
            {
                //  새로운 패킷이면 smoothing 작업을 해준다. (다음 프레임 예상 값과 현재 값의 중간 값)
                float nextTime = Game.Current.GameTime + Time.deltaTime;
                float elapsed = nextTime - before.GameTime;

                Vector3 expectedPosition = before.position + before.velocity.ToVector3() * elapsed;
                Vector3 expectedRotation = before.rotation + before.angularVelocity.ToVector3() * elapsed;

                Entity.GetComponent<EntityBasicView>().Position = Vector3.Lerp(lastPosition, expectedPosition, 0.5f);
                Entity.GetComponent<EntityBasicView>().Rotation = Quaternion.Lerp(Quaternion.Euler(lastRotation), Quaternion.Euler(expectedRotation), 0.5f).eulerAngles;
            }
        }

        lastPosition = Entity.GetComponent<EntityBasicView>().Position;
        lastRotation = Entity.GetComponent<EntityBasicView>().Rotation;
        lastSnap = before;
    }
}
