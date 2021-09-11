using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class CharacterStatusData : MonoEntityComponentBase
{
	private FirstStatus m_FirstStatus;
	private SecondStatus m_SecondStatus;

	private int m_nSelectableFirstStatusCount;

	public override void Initialize(EntityCreationData entityCreationData)
	{
		base.Initialize(entityCreationData);

        CharacterCreationData characterCreationData = entityCreationData as CharacterCreationData;

        m_FirstStatus = characterCreationData.firstStatus;
        m_SecondStatus = characterCreationData.secondStatus;
	}

	public int CurrentHP
	{
		get { return m_SecondStatus.HP; }
		set { m_SecondStatus.HP = Mathf.Min(value, m_SecondStatus.MaximumHP); }
	}

	public int CurrentMP
	{
		get { return m_SecondStatus.MP; }
		set { m_SecondStatus.MP = Mathf.Min(value, m_SecondStatus.MaximumMP); }
	}

	public int MaximumHP { get { return m_SecondStatus.MaximumHP; } }

	public int MaximumMP { get { return m_SecondStatus.MaximumMP; } }

	public float MovementSpeed { get { return m_SecondStatus.MovementSpeed; } }

	public int STR
	{
		get { return m_FirstStatus.STR; }
		set { m_FirstStatus.STR = value; }
	}

	public int DEX
	{
		get { return m_FirstStatus.DEX; }
		set { m_FirstStatus.DEX = value; }
	}

	public int CON
	{
		get { return m_FirstStatus.CON; }
		set { m_FirstStatus.CON = value; }
	}

	public int INT
	{
		get { return m_FirstStatus.INT; }
		set { m_FirstStatus.INT = value; }
	}

	public int WIS
	{
		get { return m_FirstStatus.WIS; }
		set { m_FirstStatus.WIS = value; }
	}

	public int CHA
	{
		get { return m_FirstStatus.CHA; }
		set { m_FirstStatus.CHA = value; }
	}

	public int SelectableFirstStatusCount
	{
		get { return m_nSelectableFirstStatusCount; }
		set { m_nSelectableFirstStatusCount = value; }
	}

	public void SetFirstStatus(FirstStatus firstStatus)
	{
		m_FirstStatus = firstStatus;
	}

	public void SetSecondStatus(SecondStatus secondStatus)
	{
		m_SecondStatus = secondStatus;
	}
}
