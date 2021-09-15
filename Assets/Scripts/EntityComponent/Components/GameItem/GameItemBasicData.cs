using UnityEngine;
using EntityMessage;

public class GameItemBasicData : EntityBasicData
{
    public int MasterDataId { get; private set; } = -1;

    private string modelId;
    public string ModelId
    {
        get => modelId;
        private set
        {
            modelId = value;
            Entity.MessageBroker.Publish(new ModelChanged(value));
        }
    }

    private int hp;
    public int HP
    {
        get => hp;
        set => hp = Mathf.Min(value, MaximumHP);
    }

    public int MaximumHP { get; private set; }

    public float MovementSpeed => 0;

    public override void Initialize(EntityCreationData entityCreationData)
	{
		base.Initialize(entityCreationData);

        GameItemCreationData gameItemCreationData = entityCreationData as GameItemCreationData;

        MasterDataId = gameItemCreationData.masterDataId;
        ModelId = gameItemCreationData.modelId;
        HP = gameItemCreationData.HP;
        MaximumHP = gameItemCreationData.maximumHP;
    }
}
