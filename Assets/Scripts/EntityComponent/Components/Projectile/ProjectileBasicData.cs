
public class ProjectileBasicData : EntityBasicData
{
	private int m_nMasterDataID = -1;
	private float m_fMovementSpeed = 0f;

	public override void Initialize(params object[] param)
	{
		base.Initialize(param[1]);

		m_nMasterDataID = (int)param[0];
		m_fMovementSpeed = (float)param[2];
	}

	public int MasterDataID { get { return m_nMasterDataID; } }

	public float MovementSpeed { get { return m_fMovementSpeed; } }
}
