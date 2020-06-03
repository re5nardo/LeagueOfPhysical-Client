using UnityEngine;
using Entity;

public class ProjectileBuilder
{
    private int m_nEntityID = -1;
    private int m_nMasterDataID = -1;
    private Vector3 m_vec3Position = Vector3.zero;
    private Vector3 m_vec3Rotation = Vector3.zero;
	private Vector3 m_vec3Velocity = Vector3.zero;
	private Vector3 m_vec3AngularVelocity = Vector3.zero;
	private string m_strModelPath = "";
	private float m_fMovementSpeed = default;

    public ProjectileBuilder SetEntityID(int nEntityID)
    {
        m_nEntityID = nEntityID;
        return this;
    }

    public ProjectileBuilder SetMasterDataID(int nMasterDataID)
    {
        m_nMasterDataID = nMasterDataID;
        return this;
    }

    public ProjectileBuilder SetPosition(Vector3 vec3Position)
    {
        m_vec3Position = vec3Position;
        return this;
    }

    public ProjectileBuilder SetRotation(Vector3 vec3Rotation)
    {
        m_vec3Rotation = vec3Rotation;
        return this;
    }

	public ProjectileBuilder SetVelocity(Vector3 vec3Velocity)
	{
		m_vec3Velocity = vec3Velocity;
		return this;
	}

	public ProjectileBuilder SetAngularVelocity(Vector3 vec3AngularVelocity)
	{
		m_vec3AngularVelocity = vec3AngularVelocity;
		return this;
	}

	public ProjectileBuilder SetModelPath(string strModelPath)
    {
        m_strModelPath = strModelPath;
        return this;
    }

	public ProjectileBuilder SetMovementSpeed(float fMovementSpeed)
	{
		m_fMovementSpeed = fMovementSpeed;
		return this;
	}

    public Projectile Build()
    {
        GameObject goProjectile = new GameObject(string.Format("Entity_{0}", m_nEntityID));
        Projectile projectile = goProjectile.AddComponent<Projectile>();

        projectile.Initialize(m_nEntityID, m_nMasterDataID, m_strModelPath, m_fMovementSpeed);
        projectile.Position = m_vec3Position;
        projectile.Rotation = m_vec3Rotation;
		projectile.Velocity = m_vec3Velocity;
		projectile.AngularVelocity = m_vec3AngularVelocity;

        EntityManager.Instance.RegisterEntity(projectile);

        return projectile;
    }

    public void Clear()
    {
        m_nEntityID = -1;
		m_nMasterDataID = -1;
		m_vec3Position = Vector3.zero;
        m_vec3Rotation = Vector3.zero;
		m_vec3Velocity = Vector3.zero;
		m_vec3AngularVelocity = Vector3.zero;
		m_strModelPath = "";
		m_fMovementSpeed = default;
	}
}