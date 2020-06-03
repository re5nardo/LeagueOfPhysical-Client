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
    private EntityTransformInfo m_lastTransformInfo = null;
    private float localTime = 0;

    public void SetData(EntityTransformInfo entityTransformInfo)
    {
		m_listEntityTransformInfo.Add(entityTransformInfo);

		if (m_listEntityTransformInfo.Count > MAX_BUFFER_COUNT)
		{
			m_listEntityTransformInfo.RemoveRange(0, m_listEntityTransformInfo.Count - MAX_BUFFER_COUNT);
		}

		m_lastTransformInfo = entityTransformInfo;
	}
   
    private void Update()
    {
        if (localTime < Game.Current.GameTime)
        {
            localTime = Game.Current.GameTime;  //  lerping??
        }
        else if (localTime - Game.Current.GameTime < 1)
        {
            localTime += Time.deltaTime;
        }

        SyncedMovement();
    }

	private void SyncedMovement()
	{
        EntityTransformInfo before = m_listEntityTransformInfo.FindLast(x => x.m_GameTime <= localTime);
        EntityTransformInfo next = m_listEntityTransformInfo.Find(x => x.m_GameTime >= localTime);

        if (before == null && next == null)
            return;

        if (before != null && next != null)
        {
            float t = (before.m_GameTime == next.m_GameTime) ? 0 : (localTime - before.m_GameTime) / (next.m_GameTime - before.m_GameTime);

            if (float.IsNaN(t))
            {
                Debug.LogWarning(string.Format("localTime : {0}, before.m_GameTime : {1}, next.m_GameTime : {2}", localTime, before.m_GameTime, next.m_GameTime));
            }

            m_Entity.Position = Vector3.Lerp(before.m_Position, next.m_Position, t);
            m_Entity.Rotation = Quaternion.Lerp(Quaternion.Euler(before.m_Rotation), Quaternion.Euler(next.m_Rotation), t).eulerAngles;
            m_Entity.Velocity = Vector3.Lerp(before.m_Velocity, next.m_Velocity, t);
            m_Entity.AngularVelocity = Vector3.Lerp(before.m_AngularVelocity, next.m_AngularVelocity, t);
        }
        else if (before != null)
        {
            float elapsed = localTime - before.m_GameTime;

            m_Entity.Position = before.m_Position + before.m_Velocity.ToVector3() * elapsed;
            m_Entity.Rotation = before.m_Rotation + before.m_AngularVelocity.ToVector3() * elapsed;
            m_Entity.Velocity = before.m_Velocity;
            m_Entity.AngularVelocity = before.m_AngularVelocity;
        }
    }


    //	https://m.blog.naver.com/PostView.nhn?blogId=linegamedev&logNo=221061964789&targetKeyword=&targetRecommendationCode=1
    //	현재 멈출때 쯤 앞으로 갔다 뒤로 가는 이슈 있음 (결과적으로 애니메이터 연출 이상해짐)
    //	정확하지 않은 velocity값을 받고 있어서 예측이 틀림
    //	서버에서 velocity를 감가속 개념 도입해서 아날로그한 구조로 변경해야 할듯??

    private float m_fLastSmoothingTime = -1f;
    private void DeadReckoning_Smoothing()
	{
		if (m_lastTransformInfo == null)
		{
			return;
		}

        var expected2 = m_lastTransformInfo.m_Position + m_lastTransformInfo.m_Velocity.ToVector3() * Time.deltaTime * 2;

        m_Entity.Position += ((expected2 - m_Entity.Position).normalized * m_lastTransformInfo.m_Velocity.ToVector3().magnitude * Time.deltaTime);

        //  Rotation
        EntityTransformInfo before = m_listEntityTransformInfo.FindLast(x => x.m_GameTime <= Game.Current.GameTime);
        EntityTransformInfo next = m_listEntityTransformInfo.Find(x => x.m_GameTime >= Game.Current.GameTime);

        if (before == null && next == null)
            return;

        if (before != null && next != null)
        {
            float t = (before.m_GameTime == next.m_GameTime) ? 0 : (Game.Current.GameTime - before.m_GameTime) / (next.m_GameTime - before.m_GameTime);

            if (float.IsNaN(t))
            {
                Debug.LogWarning(string.Format("Game.Current.GameTime : {0}, before.m_GameTime : {1}, next.m_GameTime : {2}", Game.Current.GameTime, before.m_GameTime, next.m_GameTime));
            }

            m_Entity.Rotation = Quaternion.Lerp(Quaternion.Euler(before.m_Rotation), Quaternion.Euler(next.m_Rotation), t).eulerAngles;
            m_Entity.Velocity = Vector3.Lerp(before.m_Velocity, next.m_Velocity, t);
            m_Entity.AngularVelocity = Vector3.Lerp(before.m_AngularVelocity, next.m_AngularVelocity, t);
        }
        else if (before != null)
        {
            float elapsed = Game.Current.GameTime - before.m_GameTime;

            m_Entity.Position = before.m_Position + before.m_Velocity.ToVector3() * elapsed;
            m_Entity.Rotation = before.m_Rotation + before.m_AngularVelocity.ToVector3() * elapsed;
            m_Entity.Velocity = before.m_Velocity;
            m_Entity.AngularVelocity = before.m_AngularVelocity;
        }

		m_fLastSmoothingTime = m_lastTransformInfo.m_GameTime;
	}
}
