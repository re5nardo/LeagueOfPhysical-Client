using EntityMessage;

public class ProjectileBasicData : EntityBasicData
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

    public float MovementSpeed { get; private set; }

    protected override void OnInitialize(EntityCreationData entityCreationData)
	{
		base.OnInitialize(entityCreationData);

        ProjectileCreationData projectileCreationData = entityCreationData as ProjectileCreationData;

        MasterDataId = projectileCreationData.masterDataId;
        ModelId = projectileCreationData.modelId;
        MovementSpeed = projectileCreationData.movementSpeed;
    }
}
