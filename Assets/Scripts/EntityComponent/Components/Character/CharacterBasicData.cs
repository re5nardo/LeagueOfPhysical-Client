﻿using UnityEngine;
using EntityMessage;

public class CharacterBasicData : EntityBasicData
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

    public int Level { get; set; } = 1;

    protected override void OnInitialize(EntityCreationData entityCreationData)
	{
		base.OnInitialize(entityCreationData);

        CharacterCreationData characterCreationData = entityCreationData as CharacterCreationData;

        MasterDataId = characterCreationData.masterDataId;
        ModelId = characterCreationData.modelId;
    }
}
