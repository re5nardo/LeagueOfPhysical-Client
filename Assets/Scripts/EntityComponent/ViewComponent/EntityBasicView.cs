using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class EntityBasicView : LOPMonoEntityComponentBase
{
	private GameObject modelGameObject;
    private List<Renderer> modelRenderers = new List<Renderer>();

    public Collider ModelCollider { get; private set; }
    public Animator ModelAnimator { get; private set; }

    public override void OnAttached(IEntity entity)
    {
        base.OnAttached(entity);

        Entity.MessageBroker.AddSubscriber<EntityMessage.ModelChanged>(OnModelChanged);
        Entity.MessageBroker.AddSubscriber<EntityMessage.AnimatorSetTrigger>(OnAnimatorSetTrigger);
        Entity.MessageBroker.AddSubscriber<EntityMessage.AnimatorSetFloat>(OnAnimatorSetFloat);
        Entity.MessageBroker.AddSubscriber<EntityMessage.AnimatorSetBool>(OnAnimatorSetBool);
        Entity.MessageBroker.AddSubscriber<EntityMessage.Destroying>(OnDestroying);

        SceneMessageBroker.AddSubscriber<TickMessage.BeforePhysicsSimulation>(OnBeforePhysicsSimulation);
        SceneMessageBroker.AddSubscriber<TickMessage.AfterPhysicsSimulation>(OnAfterPhysicsSimulation);
    }

    public override void OnDetached()
    {
        base.OnDetached();

        Entity.MessageBroker.RemoveSubscriber<EntityMessage.ModelChanged>(OnModelChanged);
        Entity.MessageBroker.RemoveSubscriber<EntityMessage.AnimatorSetTrigger>(OnAnimatorSetTrigger);
        Entity.MessageBroker.RemoveSubscriber<EntityMessage.AnimatorSetFloat>(OnAnimatorSetFloat);
        Entity.MessageBroker.RemoveSubscriber<EntityMessage.AnimatorSetBool>(OnAnimatorSetBool);
        Entity.MessageBroker.RemoveSubscriber<EntityMessage.Destroying>(OnDestroying);

        SceneMessageBroker.RemoveSubscriber<TickMessage.BeforePhysicsSimulation>(OnBeforePhysicsSimulation);
        SceneMessageBroker.RemoveSubscriber<TickMessage.AfterPhysicsSimulation>(OnAfterPhysicsSimulation);
    }

    #region Command Handlers
    private void OnModelChanged(EntityMessage.ModelChanged message)
    {
        ClearModel();
        SetModel(message.name);
    }
    
    private void OnAnimatorSetTrigger(EntityMessage.AnimatorSetTrigger message)
    {
        Animator_SetTrigger(message.name);
    }

    private void OnAnimatorSetFloat(EntityMessage.AnimatorSetFloat message)
    {
        Animator_SetFloat(message.name, message.value);
    }

    private void OnAnimatorSetBool(EntityMessage.AnimatorSetBool message)
    {
        Animator_SetBool(message.name, message.value);
    }

    private void OnDestroying(EntityMessage.Destroying message)
    {
        Clear();
    }
    #endregion

	#region MonoBehaviour
	private void OnDestroy()
	{
        Clear();
    }
    #endregion

    private void Clear()
    {
        ClearModel();
    }

    protected virtual void SetModel(string strModel)
	{
        SetModel(ResourcePool.Instance.GetResource(strModel));
    }

    public virtual void SetModel(GameObject model)
    {
        modelGameObject = model;
        modelGameObject.transform.SetParent(Entity.Transform);
        modelGameObject.transform.localPosition = Vector3.zero;
        modelGameObject.transform.localRotation = Quaternion.identity;
        modelGameObject.transform.localScale = Vector3.one;

        ModelCollider = modelGameObject.GetComponent<Collider>();
        ModelAnimator = modelGameObject.GetComponent<Animator>();

        Entity.CollisionReporter.onCollisionEnter += OnModelCollisionEnterHandler;
        Entity.CollisionReporter.onTriggerEnter += OnModelTriggerEnterHandler;
        Entity.CollisionReporter.onTriggerStay += OnModelTriggerStayHandler;

        modelGameObject.GetComponentsInChildren(true, modelRenderers);
    }

	protected virtual void ClearModel()
	{
		if (modelGameObject != null)
		{
			if (ResourcePool.HasInstance())
			{
				ResourcePool.Instance.ReturnResource(modelGameObject);
			}
		}

        modelGameObject = null;
        ModelCollider = null;
        ModelAnimator = null;

        Entity.CollisionReporter.onCollisionEnter -= OnModelCollisionEnterHandler;
        Entity.CollisionReporter.onTriggerEnter -= OnModelTriggerEnterHandler;
        Entity.CollisionReporter.onTriggerStay -= OnModelTriggerStayHandler;

        modelRenderers.Clear();
	}

	#region Animator
	public void Animator_SetFloat(string name, float value)
	{
        if (ModelAnimator != null)
        {
            ModelAnimator.SetFloat(name, value);
        }
	}

	public void Animator_SetBool(string name, bool value)
	{
        if (ModelAnimator != null)
        {
            ModelAnimator.SetBool(name, value);
        }
	}

	public void Animator_SetTrigger(string name)
	{
        if (ModelAnimator != null)
        {
            ModelAnimator.SetTrigger(name);
        }
	}
    #endregion

    #region Model Collision Handler
    protected virtual void OnModelCollisionEnterHandler(Collider me, Collision collision)
    {
    }

    protected virtual void OnModelTriggerEnterHandler(Collider me, Collider other)
    {
        SceneMessageBroker.Publish(new EntityMessage.ModelTriggerEnter(Entity.EntityID, me, other));
    }

    protected virtual void OnModelTriggerStayHandler(Collider me, Collider other)
    {
    }
    #endregion

    #region Physics Simulation
    protected virtual void OnBeforePhysicsSimulation(TickMessage.BeforePhysicsSimulation message)
    {
    }

    protected virtual void OnAfterPhysicsSimulation(TickMessage.AfterPhysicsSimulation message)
    {
    }
    #endregion
}
