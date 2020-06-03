using UnityEngine;

public class GameItemModel : BasicModel
{
	private int m_nMasterDataID = -1;
	private int m_nHP = 0;
	private int m_nMaximumHP = 0;

	public override void Initialize(params object[] param)
	{
		base.Initialize(param[1]);

		m_nMasterDataID = (int)param[0];
		m_nHP = (int)param[2];
		m_nMaximumHP = (int)param[3];
	}

	public int MasterDataID { get { return m_nMasterDataID; } }
	
	public float MovementSpeed { get { return 0; } }
	
	public int CurrentHP
	{
		get { return m_nHP; }
		set { m_nHP = Mathf.Min(value, m_nMaximumHP); }
	}
}
