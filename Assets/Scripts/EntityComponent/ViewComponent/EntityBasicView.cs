﻿using System.Collections.Generic;
using UnityEngine;
using EntityCommand;
using GameFramework;
using Entity;

public class EntityBasicView : MonoViewComponentBase
{
	private GameObject m_goModel = null;
    private Collider m_ColliderModel = null;
    private Animator m_AnimatorModel = null;
	private List<Renderer> m_listModelRenderer = new List<Renderer>();

    public Collider ModelCollider => m_ColliderModel;
    public Animator ModelAnimator => m_AnimatorModel;

    protected LOPMonoEntityBase lopEntity;

    public override void OnAttached(IEntity entity)
    {
        base.OnAttached(entity);

        lopEntity = entity as LOPMonoEntityBase;

        AddCommandHandler(typeof(ModelChanged), OnModelChanged);
        AddCommandHandler(typeof(AnimatorSetTrigger), OnAnimatorSetTrigger);
        AddCommandHandler(typeof(AnimatorSetFloat), OnAnimatorSetFloat);
        AddCommandHandler(typeof(AnimatorSetBool), OnAnimatorSetBool);
        AddCommandHandler(typeof(Destroying), OnDestroying);
    }

    public override void OnDetached()
    {
        base.OnDetached();

        lopEntity = null;

        RemoveCommandHandler(typeof(ModelChanged), OnModelChanged);
        RemoveCommandHandler(typeof(AnimatorSetTrigger), OnAnimatorSetTrigger);
        RemoveCommandHandler(typeof(AnimatorSetFloat), OnAnimatorSetFloat);
        RemoveCommandHandler(typeof(AnimatorSetBool), OnAnimatorSetBool);
        RemoveCommandHandler(typeof(Destroying), OnDestroying);
    }

    #region Command Handlers
    private void OnModelChanged(ICommand command)
    {
        ModelChanged cmd = command as ModelChanged;

        ClearModel();
        SetModel(cmd.name);
    }
    
    private void OnAnimatorSetTrigger(ICommand command)
    {
        Animator_SetTrigger((command as AnimatorSetTrigger).name);
    }

    private void OnAnimatorSetFloat(ICommand command)
    {
        AnimatorSetFloat cmd = command as AnimatorSetFloat;

        Animator_SetFloat(cmd.name, cmd.value);
    }

    private void OnAnimatorSetBool(ICommand command)
    {
        AnimatorSetBool cmd = command as AnimatorSetBool;

        Animator_SetBool(cmd.name, cmd.value);
    }

    private void OnDestroying(ICommand command)
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
        m_goModel = model;
        m_goModel.transform.SetParent(lopEntity.Transform);
        m_goModel.transform.localPosition = Vector3.zero;
        m_goModel.transform.localRotation = Quaternion.identity;
        m_goModel.transform.localScale = Vector3.one;

        m_ColliderModel = m_goModel.GetComponent<Collider>();
        m_AnimatorModel = m_goModel.GetComponent<Animator>();

        m_goModel.GetComponentsInChildren(true, m_listModelRenderer);
    }

	protected virtual void ClearModel()
	{
		if (m_goModel != null)
		{
			if (ResourcePool.HasInstance())
			{
				ResourcePool.Instance.ReturnResource(m_goModel);
			}
		}

		m_goModel = null;
        m_ColliderModel = null;
        m_AnimatorModel = null;

		m_listModelRenderer.Clear();
	}

	#region Animator
	public void Animator_SetFloat(string name, float value)
	{
        if (m_AnimatorModel != null)
        {
            m_AnimatorModel.SetFloat(name, value);
        }
	}

	public void Animator_SetBool(string name, bool value)
	{
        if (m_AnimatorModel != null)
        {
            m_AnimatorModel.SetBool(name, value);
        }
	}

	public void Animator_SetTrigger(string name)
	{
        if (m_AnimatorModel != null)
        {
            m_AnimatorModel.SetTrigger(name);
        }
	}
    #endregion
}
