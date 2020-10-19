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

    private Vector3 lastPosition = default;
    private Vector3 lastRotation = default;
    private Vector3 lastVelocity = default;
    private Vector3 lastAngularVelocity = default;

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
        SyncedMovement();
    }

    private void SyncedMovement()
    {
        EntityTransformSnap before = entityTransformSnaps.FindLast(x => x.GameTime <= Game.Current.GameTime);
        EntityTransformSnap next = entityTransformSnaps.Find(x => x.GameTime >= Game.Current.GameTime);

        if (before == null && next == null)
            return;

        Vector3 currentPosition = default;
        Vector3 currentRotation = default;
        Vector3 currentVelocity = default;
        Vector3 currentAngularVelocity = default;

        if (before != null && next != null)
        {
            float t = (before.GameTime == next.GameTime) ? 0 : (Game.Current.GameTime - before.GameTime) / (next.GameTime - before.GameTime);

            if (float.IsNaN(t))
            {
                Debug.LogWarning(string.Format("localTime : {0}, before.m_GameTime : {1}, next.m_GameTime : {2}", Game.Current.GameTime, before.GameTime, next.GameTime));
            }

            currentPosition = Vector3.Lerp(before.position, next.position, t);
            currentRotation = Quaternion.Lerp(Quaternion.Euler(before.rotation), Quaternion.Euler(next.rotation), t).eulerAngles;
            currentVelocity = Vector3.Lerp(before.velocity, next.velocity, t);
            currentAngularVelocity = Vector3.Lerp(before.angularVelocity, next.angularVelocity, t);
        }
        else if (before != null)
        {
            float elapsed = Game.Current.GameTime - before.GameTime;

            currentPosition = before.position + before.velocity.ToVector3() * elapsed;
            currentRotation = before.rotation + before.angularVelocity.ToVector3() * elapsed;
            currentVelocity = before.velocity;
            currentAngularVelocity = before.angularVelocity;
        }

        //  Position
        currentPosition += FPM_Manager.Instance.PendingPosition * 0.1f;
        float distance = (currentPosition - lastPosition).magnitude;
        if ((distance > ((Entity as Entity.MonoEntityBase).MovementSpeed * Time.deltaTime * 3)) /*순간이동*/ || distance <= ((Entity as Entity.MonoEntityBase).MovementSpeed * Time.deltaTime) /*범위 내*/)
        {
            currentPosition = currentPosition;
        }
        else
        {
            currentPosition = Vector3.LerpUnclamped(lastPosition, currentPosition, 1.1f);    //  보정
        }
        lastPosition = Entity.Position = currentPosition;

        //  Rotation
        currentRotation += FPM_Manager.Instance.PendingPosition * 0.1f;
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
