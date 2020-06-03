﻿using System.Collections.Generic;
using UnityEngine;
using EntityCommand;
using GameFramework;

public class BasicView : MonoViewComponentBase
{
	private GameObject m_goModel = null;
	private Transform m_trModel = null;
	private Animator m_AnimatorModel = null;
	private List<Renderer> m_listModelRenderer = new List<Renderer>();
	
	public override void OnCommand(ICommand command)
	{
		base.OnCommand(command);

		if (command is ModelChanged)
		{
			ModelChanged cmd = command as ModelChanged;

			ClearModel();
			SetModel(cmd.name);
		}
		else if(command is PositionChanged)
		{
			Position = Entity.Position;
		}
		else if (command is RotationChanged)
		{
			Rotation = Entity.Rotation;
		}
		else if (command is AnimatorSetTrigger)
		{
			Animator_SetTrigger((command as AnimatorSetTrigger).name);
		}
		else if (command is AnimatorSetFloat)
		{
			AnimatorSetFloat cmd = command as AnimatorSetFloat;

			Animator_SetFloat(cmd.name, cmd.value);
		}
		else if (command is AnimatorSetBool)
		{
			AnimatorSetBool cmd = command as AnimatorSetBool;

			Animator_SetBool(cmd.name, cmd.value);
		}
        else if (command is Destroying)
        {
            Clear();
        }
    }

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
		m_goModel = ResourcePool.Instance.GetResource(strModel);
		m_goModel.transform.parent = null;
		m_trModel = m_goModel.transform;

		m_AnimatorModel = m_goModel.GetComponent<Animator>();

		m_goModel.GetComponentsInChildren(true, m_listModelRenderer);
	}

	protected virtual void ClearModel()
	{
		if (m_goModel != null)
		{
			if (ResourcePool.IsInstantiated())
			{
				ResourcePool.Instance.ReturnResource(m_goModel);
			}
		}

		m_goModel = null;
		m_trModel = null;
		m_AnimatorModel = null;

		m_listModelRenderer.Clear();
	}

	public Transform ModelTransform { get { return m_trModel; } }
	
	public Vector3 Position
	{
		get { return m_trModel.position; }
		set { m_trModel.position = value; }
	}

	public Vector3 Rotation
	{
		get { return m_trModel.rotation.eulerAngles; }
		set { m_trModel.rotation = Quaternion.Euler(value); }
	}
	
	#region Animator
	public void Animator_SetFloat(string name, float value)
	{
		m_AnimatorModel?.SetFloat(name, value);
	}

	public void Animator_SetBool(string name, bool value)
	{
		m_AnimatorModel?.SetBool(name, value);
	}

	public void Animator_SetTrigger(string name)
	{
		m_AnimatorModel?.SetTrigger(name);
	}
	#endregion
}
