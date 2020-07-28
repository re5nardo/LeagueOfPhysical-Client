using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using GameFramework;

public class TransformInterpolator : MonoBehaviour
{
    private MonoEntityBase m_Entity__ = null;
    private MonoEntityBase m_Entity
    {
        get
        {
            if(m_Entity__ == null)
            {
                m_Entity__ = GetComponent<MonoEntityBase>();
            }

            return m_Entity__;
        }
    }

	private const int MAX_BUFFER_COUNT = 5;

	private List<EntityTransformInfo> m_listEntityTransformInfo = new List<EntityTransformInfo>();
    private float localTime = 0;

    private Vector3 lastPosition = default;
    private Vector3 lastRotation = default;
    private Vector3 lastVelocity = default;
    private Vector3 lastAngularVelocity = default;

    public void SetData(EntityTransformInfo entityTransformInfo)
    {
		m_listEntityTransformInfo.Add(entityTransformInfo);

		if (m_listEntityTransformInfo.Count > MAX_BUFFER_COUNT)
		{
			m_listEntityTransformInfo.RemoveRange(0, m_listEntityTransformInfo.Count - MAX_BUFFER_COUNT);
		}
	}
   
    private void Update()
    {
        UpdateLocalTime();
    
        SyncedMovement();
    }

    private void UpdateLocalTime()
    {
        float gap = localTime - Game.Current.GameTime;

        if (gap < -0.2)
        {
            localTime = Game.Current.GameTime;
        }
        if (gap < -0.1)
        {
            localTime += (Time.deltaTime * 2);
        }
        else if (gap < 0.05)    //  0.05 기준 (latency 정도로 맞추면 될 듯??)
        {
            localTime += (Time.deltaTime * 1.5f);
        }
        else if (gap < 0.1)
        {
            localTime += Time.deltaTime;
        }
        else if (gap < 0.2)
        {
            localTime += (Time.deltaTime * 0.5f);
        }
        else
        {
            localTime += 0;
        }
    }

	private void SyncedMovement()
	{
        EntityTransformInfo before = m_listEntityTransformInfo.FindLast(x => x.m_GameTime <= localTime);
        EntityTransformInfo next = m_listEntityTransformInfo.Find(x => x.m_GameTime >= localTime);

        if (before == null && next == null)
            return;

        Vector3 currentPosition = default;
        Vector3 currentRotation = default;
        Vector3 currentVelocity = default;
        Vector3 currentAngularVelocity = default;

        if (before != null && next != null)
        {
            float t = (before.m_GameTime == next.m_GameTime) ? 0 : (localTime - before.m_GameTime) / (next.m_GameTime - before.m_GameTime);

            if (float.IsNaN(t))
            {
                Debug.LogWarning(string.Format("localTime : {0}, before.m_GameTime : {1}, next.m_GameTime : {2}", localTime, before.m_GameTime, next.m_GameTime));
            }

            currentPosition = Vector3.Lerp(before.m_Position, next.m_Position, t);
            currentRotation = Quaternion.Lerp(Quaternion.Euler(before.m_Rotation), Quaternion.Euler(next.m_Rotation), t).eulerAngles;
            currentVelocity = Vector3.Lerp(before.m_Velocity, next.m_Velocity, t);
            currentAngularVelocity = Vector3.Lerp(before.m_AngularVelocity, next.m_AngularVelocity, t);
        }
        else if (before != null)
        {
            float elapsed = localTime - before.m_GameTime;

            currentPosition = before.m_Position + before.m_Velocity.ToVector3() * elapsed;
            currentRotation = before.m_Rotation + before.m_AngularVelocity.ToVector3() * elapsed;
            currentVelocity = before.m_Velocity;
            currentAngularVelocity = before.m_AngularVelocity;
        }

        //  Position
        float distance = (currentPosition - lastPosition).magnitude;
        if ((distance > (m_Entity.MovementSpeed * Time.deltaTime * 3)) /*순간이동*/ || distance <= (m_Entity.MovementSpeed * Time.deltaTime) /*이동 범위 내*/)   
        {
            currentPosition = currentPosition;
        }
        else
        {
            currentPosition = Vector3.LerpUnclamped(lastPosition, currentPosition, 1.1f);    //  보정
        }
        lastPosition = m_Entity.Position = currentPosition;

        //  Rotation (현재는 보정x)
        lastRotation = m_Entity.Rotation = currentRotation;

        //  Velocity (현재는 보정x)
        lastVelocity = m_Entity.Velocity = currentVelocity;

        //  AngularVelocity (현재는 보정x)
        lastAngularVelocity = m_Entity.AngularVelocity = currentAngularVelocity;
    }
}
