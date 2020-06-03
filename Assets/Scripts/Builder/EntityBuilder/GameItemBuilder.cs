using UnityEngine;
using Entity;

public class GameItemBuilder
{
	private int m_nEntityID = -1;
	private int m_nMasterDataID = -1;
	private Vector3 m_vec3Position = Vector3.zero;
	private Vector3 m_vec3Rotation = Vector3.zero;
	private Vector3 m_vec3Velocity = Vector3.zero;
	private Vector3 m_vec3AngularVelocity = Vector3.zero;
	private string m_strModelPath = "";
	private int m_nHP = default;
	private int m_nMaximumHP = default;

	public GameItemBuilder SetEntityID(int nEntityID)
	{
		m_nEntityID = nEntityID;
		return this;
	}

	public GameItemBuilder SetMasterDataID(int nMasterDataID)
	{
		m_nMasterDataID = nMasterDataID;
		return this;
	}

	public GameItemBuilder SetPosition(Vector3 vec3Position)
	{
		m_vec3Position = vec3Position;
		return this;
	}

	public GameItemBuilder SetRotation(Vector3 vec3Rotation)
	{
		m_vec3Rotation = vec3Rotation;
		return this;
	}

	public GameItemBuilder SetVelocity(Vector3 vec3Velocity)
	{
		m_vec3Velocity = vec3Velocity;
		return this;
	}

	public GameItemBuilder SetAngularVelocity(Vector3 vec3AngularVelocity)
	{
		m_vec3AngularVelocity = vec3AngularVelocity;
		return this;
	}

	public GameItemBuilder SetModelPath(string strModelPath)
	{
		m_strModelPath = strModelPath;
		return this;
	}

	public GameItemBuilder SetHP(int nHP)
	{
		m_nHP = nHP;
		return this;
	}

	public GameItemBuilder SetMaximumHP(int nMaximumHP)
	{
		m_nMaximumHP = nMaximumHP;
		return this;
	}

	public GameItem Build()
	{
		GameObject goGameItem = new GameObject(string.Format("Entity_{0}", m_nEntityID));
        GameItem gameItem = goGameItem.AddComponent<GameItem>();

		gameItem.Initialize(m_nEntityID, m_nMasterDataID, m_strModelPath, m_nHP, m_nMaximumHP);
		gameItem.Position = m_vec3Position;
		gameItem.Rotation = m_vec3Rotation;
		gameItem.Velocity = m_vec3Velocity;
		gameItem.AngularVelocity = m_vec3AngularVelocity;

        EntityManager.Instance.RegisterEntity(gameItem);

        return gameItem;
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
		m_nHP = default;
		m_nMaximumHP = default;
	}
}
