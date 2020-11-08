using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System;

public class EntityTransformSynchronization : MonoComponentBase, ISynchronizable
{
    #region ISynchronizable
    public ISynchronizable Parent { get; set; } = null;
    public bool Enable { get; set; } = true;
    public bool EnableInHierarchy => Parent == null ? Enable : Parent.EnableInHierarchy && Enable;
    public bool HasCoreChange => LastSendSnap == null ? true : !LastSendSnap.EqualsCore(CurrentSnap.Set(this));
    public bool IsDirty => isDirty || LastSendSnap == null ? true : !LastSendSnap.EqualsValue(CurrentSnap.Set(this));
    #endregion

    private bool isDirty = false;
    private EntityTransformSnap LastSendSnap { get; set; } = new EntityTransformSnap();
    private EntityTransformSnap CurrentSnap { get; set; } = new EntityTransformSnap();
    private List<EntityTransformSnap> entityTransformSnaps = new List<EntityTransformSnap>();


    private const int MAX_BUFFER_COUNT = 5;

    private EntityTransformSnap lastSnap = null;

    public override void OnAttached(IEntity entity)
    {
        base.OnAttached(entity);

        MonoEntitySynchronization monoEntitySynchronization = Entity.GetComponent<MonoEntitySynchronization>();
        monoEntitySynchronization?.Add(this);
    }

    public override void OnDetached()
    {
        base.OnDetached();

        MonoEntitySynchronization monoEntitySynchronization = Entity.GetComponent<MonoEntitySynchronization>();
        monoEntitySynchronization?.Remove(this);
    }

    private void Update()
    {
        //SyncedMovement();
    }

    private void SyncedMovement()
    {
        EntityTransformSnap before = entityTransformSnaps.FindLast(x => x.GameTime <= Game.Current.GameTime);
        EntityTransformSnap next = entityTransformSnaps.Find(x => x.GameTime >= Game.Current.GameTime);

        if (before == null)
        {
            return;
        }

        if (before != null && next != null)
        {
            float t = (before.GameTime == next.GameTime) ? 0 : (Game.Current.GameTime - before.GameTime) / (next.GameTime - before.GameTime);

            if (float.IsNaN(t))
            {
                Debug.LogWarning(string.Format("localTime : {0}, before.m_GameTime : {1}, next.m_GameTime : {2}", Game.Current.GameTime, before.GameTime, next.GameTime));
            }

            Entity.Position = Vector3.Lerp(before.position, next.position, t);
            Entity.Rotation = Quaternion.Lerp(Quaternion.Euler(before.rotation), Quaternion.Euler(next.rotation), t).eulerAngles;
            Entity.Velocity = Vector3.Lerp(before.velocity, next.velocity, t);
            Entity.AngularVelocity = Vector3.Lerp(before.angularVelocity, next.angularVelocity, t);
        }
        else
        {
            //  로테이션 예측한 값이 실제 값 보다 크거나 작으면, 실제 값으로 맞춰지는데 그 느낌이 튕기는(유쾌하지 않은) 느낌...

            if (lastSnap == null || lastSnap == before)
            {
                float elapsed = Game.Current.GameTime - before.GameTime;

                Entity.Position = before.position + before.velocity.ToVector3() * elapsed;
                Entity.Rotation = before.rotation + before.angularVelocity.ToVector3() * elapsed * 0.5f/*로테이션 예측 정도를 반으로 줄여서 로테이션이 튕겨보이는 현상 방지*/;
                Entity.Velocity = before.velocity;
                Entity.AngularVelocity = before.angularVelocity;
            }
            else
            {
                //  새로운 패킷이면 smoothing 작업을 해준다. (다음 프레임 예상 값과 현재 값의 중간 값)
                float nextTime = Game.Current.GameTime + 0.033f;
                float elapsed = nextTime - before.GameTime;

                Vector3 expectedPosition = before.position + before.velocity.ToVector3() * elapsed;
                Vector3 expectedRotation = before.rotation + before.angularVelocity.ToVector3() * elapsed * 0.5f/*로테이션 예측 정도를 반으로 줄여서 로테이션이 튕겨보이는 현상 방지*/;

                Entity.Position = Vector3.Lerp(Entity.Position, expectedPosition, 0.5f);
                Entity.Rotation = Quaternion.Lerp(Quaternion.Euler(Entity.Rotation), Quaternion.Euler(expectedRotation), 0.5f).eulerAngles;
                Entity.Velocity = before.velocity;
                Entity.AngularVelocity = before.angularVelocity;
            }
        }
        
        lastSnap = before;
    }

    private void Reconcile(ISnap snap)
    {
    }

    #region ISynchronizable
    public void SetDirty()
    {
        isDirty = true;
    }

    public ISnap GetSnap()
    {
        throw new NotImplementedException();
    }

    public void UpdateSynchronizable()
    {
        throw new NotImplementedException();
    }

    public void SendSynchronization()
    {
        throw new NotImplementedException();
    }

    public void OnReceiveSynchronization(ISnap snap)
    {
        entityTransformSnaps.Add(snap as EntityTransformSnap);

        if (entityTransformSnaps.Count > MAX_BUFFER_COUNT)
        {
            entityTransformSnaps.RemoveRange(0, entityTransformSnaps.Count - MAX_BUFFER_COUNT);
        }

        Reconcile(snap);
    }
    #endregion
}
