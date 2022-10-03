﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using System;
using GameFramework;
using EntityMessage;
using NetworkModel.Mirror;

public class EntityManager : GameFramework.EntityManager
{
    #region Singlton Pattern
    protected static EntityManager instance = null;
    public static EntityManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject goSingleton = new GameObject("EntityManager");

                instance = goSingleton.AddComponent<EntityManager>();
            }

            return instance;
        }
    }

    public static bool IsInstantiated()
    {
        return instance != null;
    }

    public static void Instantiate()
    {
        if (instance == null)
        {
            GameObject goSingleton = new GameObject("EntityManager");

            instance = goSingleton.AddComponent<EntityManager>();
        }
    }
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        positionGrid = new Grid();
        positionGrid.SetGrid(LOP.Game.GRID_SIZE);

        SceneMessageBroker.AddSubscriber<SC_EntityAppear>(OnSC_EntityAppear);
        SceneMessageBroker.AddSubscriber<SC_EntityDisAppear>(OnSC_EntityDisAppear);
        SceneMessageBroker.AddSubscriber<TickMessage.Tick>(OnTick);
	}

    private void OnDestroy()
    {
        positionGrid.Clear();
        positionGrid = null;

        SceneMessageBroker.RemoveSubscriber<SC_EntityAppear>(OnSC_EntityAppear);
        SceneMessageBroker.RemoveSubscriber<SC_EntityDisAppear>(OnSC_EntityDisAppear);
        SceneMessageBroker.RemoveSubscriber<TickMessage.Tick>(OnTick);
    }
    #endregion

    public override void RegisterEntity(IEntity entity)
    {
        base.RegisterEntity(entity);

        SceneMessageBroker.Publish(new GameMessage.EntityRegister(entity.EntityID));
    }

    public override void UnregisterEntity(int nEntityID)
    {
        base.UnregisterEntity(nEntityID);

        SceneMessageBroker.Publish(new GameMessage.EntityUnregister(nEntityID));
    }

    #region Message Handler
    private void OnSC_EntityAppear(SC_EntityAppear entityAppear)
	{
		foreach (EntitySnap entitySnap in entityAppear.listEntitySnap)
		{
			if(dicEntity.ContainsKey(entitySnap.entityId))
			{
				Debug.LogError("[OnSC_EntityAppear] Entity already exists! EntityID : " + entitySnap.entityId);
                continue;
			}

			var entity = CreateEntity(entitySnap);

            entity.AttachEntityComponent<TransformSyncController>();
            if (entity.ModelAnimator != null)
            {
                entity.AttachEntityComponent<AnimatorSyncController>();
            }
        }
	}

	private void OnSC_EntityDisAppear(SC_EntityDisAppear entityDisAppear)
	{
		foreach (int nEntityID in entityDisAppear.listEntityId)
		{
			if (!dicEntity.ContainsKey(nEntityID))
			{
				Debug.LogError("[OnSC_EntityDisAppear] Entity doesn't exist! EntityID : " + nEntityID);
                continue;
			}

			DestroyEntity(nEntityID);
		}
	}
	#endregion

    public void DestroyEntity(int nEntityID)
    {
        var entity = GetEntity<LOPMonoEntityBase>(nEntityID);

        entity.MessageBroker.Publish(new Destroying());

        UnregisterEntity(nEntityID);

        Destroy(entity.gameObject);
    }

    private LOPMonoEntityBase CreateEntity(EntitySnap entitySnap)
    {
        LOPMonoEntityBase entity = null;

        if (entitySnap.entityType == EntityType.Character)
        {
			CharacterSnap characterSnap = entitySnap as CharacterSnap;

			entity = Character.Builder()
                .SetEntityId(characterSnap.entityId)
                .SetEntityType(characterSnap.entityType)
                .SetEntityRole(characterSnap.entityRole)
                .SetMasterDataId(characterSnap.masterDataId)
                .SetModelId(characterSnap.modelId)
                .SetFirstStatus(characterSnap.firstStatus)
				.SetSecondStatus(characterSnap.secondStatus)
				.SetPosition(characterSnap.position)
                .SetRotation(characterSnap.rotation)
				.SetVelocity(characterSnap.velocity)
				.SetAngularVelocity(characterSnap.angularVelocity)
                .SetOwnerId(characterSnap.ownerId)
				.Build();
        }
        else if (entitySnap.entityType == EntityType.Projectile)
        {
			ProjectileSnap projectileSnap = entitySnap as ProjectileSnap;

			entity = Projectile.Builder()
                .SetEntityId(projectileSnap.entityId)
                .SetEntityType(projectileSnap.entityType)
                .SetEntityRole(projectileSnap.entityRole)
                .SetMasterDataId(projectileSnap.masterDataId)
                .SetModelId(projectileSnap.modelId)
                .SetPosition(projectileSnap.position)
                .SetRotation(projectileSnap.rotation)
				.SetVelocity(projectileSnap.velocity)
				.SetAngularVelocity(projectileSnap.angularVelocity)
				.SetMovementSpeed(projectileSnap.movementSpeed)
                .SetOwnerId(projectileSnap.ownerId)
				.Build();
        }
		else if (entitySnap.entityType == EntityType.GameItem)
		{
			GameItemSnap gameItemSnap = entitySnap as GameItemSnap;

			entity = GameItem.Builder()
				.SetEntityId(gameItemSnap.entityId)
                .SetEntityType(gameItemSnap.entityType)
                .SetEntityRole(gameItemSnap.entityRole)
                .SetMasterDataId(gameItemSnap.masterDataId)
				.SetModelId(gameItemSnap.modelId)
				.SetPosition(gameItemSnap.position)
				.SetRotation(gameItemSnap.rotation)
				.SetVelocity(gameItemSnap.velocity)
				.SetAngularVelocity(gameItemSnap.angularVelocity)
				.SetHP(gameItemSnap.HP)
				.SetMaximumHP(gameItemSnap.maximumHP)
                .SetOwnerId(gameItemSnap.ownerId)
				.Build();
		}
		else
        {
            Debug.LogError("entitySnapInfo.m_EntityType is invalid! entitySnapInfo.m_EntityType : " + entitySnap.entityType.ToString());
            return null;
        }

        return entity;
    }

    private void OnTick(TickMessage.Tick message)
    {
        //  sort
        //  ...

        GetAllEntities<LOPMonoEntityBase>()?.ForEach(entity =>
        {
            if (entity.IsValid)
            {
                entity.OnTick(message.tick);
            }
        });
    }
}
