using UnityEngine;
using EntityCommand;

public class GameItemBasicData : EntityBasicData
{
    public string ModelId { get; private set; }
    private int m_nMasterDataID = -1;
	private int m_nHP = 0;
	private int m_nMaximumHP = 0;

	public override void Initialize(EntityCreationData entityCreationData)
	{
		base.Initialize(entityCreationData);

        GameItemCreationData gameItemCreationData = entityCreationData as GameItemCreationData;

        m_nMasterDataID = gameItemCreationData.masterDataId;
        m_nHP = gameItemCreationData.HP;
        m_nMaximumHP = gameItemCreationData.maximumHP;

        ModelId = gameItemCreationData.modelId;

        Entity.SendCommandToViews(new ModelChanged(ModelId));
    }

	public int MasterDataID { get { return m_nMasterDataID; } }
	
	public float MovementSpeed { get { return 0; } }
	
	public int CurrentHP
	{
		get { return m_nHP; }
		set { m_nHP = Mathf.Min(value, m_nMaximumHP); }
	}
}
