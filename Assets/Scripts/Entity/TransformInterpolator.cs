using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using GameFramework;

public class TransformInterpolator : MonoBehaviour
{
    private MonoEntityBase entity = null;
    private MonoEntityBase Entity
    {
        get
        {
            if(entity == null)
            {
                entity = GetComponent<MonoEntityBase>();
            }

            return entity;
        }
    }

	private const int MAX_BUFFER_COUNT = 5;

	private List<EntityTransformInfo> entityTransformInfos = new List<EntityTransformInfo>();

    private Vector3 lastPosition = default;
    private Vector3 lastRotation = default;
    private Vector3 lastVelocity = default;
    private Vector3 lastAngularVelocity = default;

    public void SetData(EntityTransformInfo entityTransformInfo)
    {
        entityTransformInfos.Add(entityTransformInfo);

		if (entityTransformInfos.Count > MAX_BUFFER_COUNT)
		{
            entityTransformInfos.RemoveRange(0, entityTransformInfos.Count - MAX_BUFFER_COUNT);
		}
	}
   
    private void Update()
    {
        SyncedMovement();
    }

    private void SyncedMovement()
	{
        EntityTransformInfo before = entityTransformInfos.FindLast(x => x.m_GameTime <= Game.Current.GameTime);
        EntityTransformInfo next = entityTransformInfos.Find(x => x.m_GameTime >= Game.Current.GameTime);

        if (before == null && next == null)
            return;

        Vector3 currentPosition = default;
        Vector3 currentRotation = default;
        Vector3 currentVelocity = default;
        Vector3 currentAngularVelocity = default;

        if (before != null && next != null)
        {
            float t = (before.m_GameTime == next.m_GameTime) ? 0 : (Game.Current.GameTime - before.m_GameTime) / (next.m_GameTime - before.m_GameTime);

            if (float.IsNaN(t))
            {
                Debug.LogWarning(string.Format("localTime : {0}, before.m_GameTime : {1}, next.m_GameTime : {2}", Game.Current.GameTime, before.m_GameTime, next.m_GameTime));
            }

            currentPosition = Vector3.Lerp(before.m_Position, next.m_Position, t);
            currentRotation = Quaternion.Lerp(Quaternion.Euler(before.m_Rotation), Quaternion.Euler(next.m_Rotation), t).eulerAngles;
            currentVelocity = Vector3.Lerp(before.m_Velocity, next.m_Velocity, t);
            currentAngularVelocity = Vector3.Lerp(before.m_AngularVelocity, next.m_AngularVelocity, t);
        }
        else if (before != null)
        {
            float elapsed = Game.Current.GameTime - before.m_GameTime;

            currentPosition = before.m_Position + before.m_Velocity.ToVector3() * elapsed;
            currentRotation = before.m_Rotation + before.m_AngularVelocity.ToVector3() * elapsed;
            currentVelocity = before.m_Velocity;
            currentAngularVelocity = before.m_AngularVelocity;
        }

        //  Position
        float distance = (currentPosition - lastPosition).magnitude;
        if ((distance > (Entity.MovementSpeed * Time.deltaTime * 3)) /*순간이동*/ || distance <= (Entity.MovementSpeed * Time.deltaTime) /*범위 내*/)   
        {
            currentPosition = currentPosition;
        }
        else
        {
            currentPosition = Vector3.LerpUnclamped(lastPosition, currentPosition, 1.1f);    //  보정
        }
        lastPosition = Entity.Position = currentPosition;

        //  Rotation
        float rotate = Mathf.Abs(currentRotation.y - lastRotation.y);
        if ((rotate > (Behavior.Rotation.ANGULAR_SPEED * Time.deltaTime * 3)) /*순간이동*/ || rotate <= (Behavior.Rotation.ANGULAR_SPEED * Time.deltaTime) /*범위 내*/)
        {
            currentRotation = currentRotation;
        }
        else
        {
            currentRotation = Quaternion.LerpUnclamped(Quaternion.Euler(lastRotation), Quaternion.Euler(currentRotation), 1.1f).eulerAngles;    //  보정
        }
        lastRotation = Entity.Rotation = currentRotation;

        //  Velocity (현재는 보정x)
        lastVelocity = Entity.Velocity = currentVelocity;

        //  AngularVelocity (현재는 보정x)
        lastAngularVelocity = Entity.AngularVelocity = currentAngularVelocity;
    }
}
