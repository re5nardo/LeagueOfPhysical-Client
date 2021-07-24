using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using System;
using GameFramework;
using EntityCommand;
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

    private RoomProtocolDispatcher roomProtocolDispatcher = null;

    #region MonoBehaviour
    private void Awake()
    {
        positionGrid = new Grid();
        positionGrid.SetGrid(10);

        roomProtocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();
        roomProtocolDispatcher[typeof(SC_EntityAppear)] = OnSC_EntityAppear;
        roomProtocolDispatcher[typeof(SC_EntityDisAppear)] = OnSC_EntityDisAppear;

        TickPubSubService.AddSubscriber("Tick", OnTick);
	}

    private void OnDestroy()
    {
        TickPubSubService.RemoveSubscriber("Tick", OnTick);
    }
#endregion

    public override void RegisterEntity(IEntity entity)
    {
        base.RegisterEntity(entity);

        GamePubSubService.Publish(GameMessageKey.EntityRegister, new object[] { entity.EntityID });
    }

    public override void UnregisterEntity(int nEntityID)
    {
        base.UnregisterEntity(nEntityID);

        GamePubSubService.Publish(GameMessageKey.EntityUnregister, new object[] { nEntityID });
    }

    #region Message Handler
    private void OnSC_EntityAppear(IMessage msg)
	{
		SC_EntityAppear entityAppear = msg as SC_EntityAppear;

		foreach (EntitySnapInfo entitySnapInfo in entityAppear.listEntitySnapInfo)
		{
			if(dicEntity.ContainsKey(entitySnapInfo.entityId))
			{
				Debug.LogError("[OnSC_EntityAppear] Entity already exists! EntityID : " + entitySnapInfo.entityId);
                continue;
			}

			var entity = CreateEntity(entitySnapInfo);

			if (entity.EntityID == Entities.MyEntityID)
			{
                EntityTransformInfo info = new EntityTransformInfo();
                info.tick = entityAppear.tick;
                info.entityId = entitySnapInfo.entityId;
                info.position = entitySnapInfo.position;
                info.rotation = entitySnapInfo.rotation;
                info.velocity = entitySnapInfo.velocity;

                LOP.Game.Current.OnMyCharacterCreated(entity as Character);
			}
			else
			{
				EntityTransformInfo info = new EntityTransformInfo();
                info.tick = entityAppear.tick;
                info.entityId = entitySnapInfo.entityId;
                info.position = entitySnapInfo.position;
                info.rotation = entitySnapInfo.rotation;
                info.velocity = entitySnapInfo.velocity;
            }

            if (Entities.MyEntityID != entitySnapInfo.entityId)
            {
                (entity as MonoEntityBase).gameObject.AddComponent<TransformController>();
            }
        }
	}

	private void OnSC_EntityDisAppear(IMessage msg)
	{
		SC_EntityDisAppear entityDisAppear = msg as SC_EntityDisAppear;

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
        MonoEntityBase entity = GetEntity(nEntityID) as MonoEntityBase;

        entity.SendCommandToAll(new Destroying());

        UnregisterEntity(nEntityID);

        Destroy(entity.gameObject);
    }

    private IEntity CreateEntity(EntitySnapInfo entitySnapInfo)
    {
		IEntity entity = null;

        if (entitySnapInfo.entityType == EntityType.Character)
        {
			CharacterSnapInfo characterSnapInfo = entitySnapInfo as CharacterSnapInfo;

			entity = Character.Builder()
                .SetEntityID(characterSnapInfo.entityId)
                .SetEntityRole(characterSnapInfo.entityRole)
                .SetMasterDataID(characterSnapInfo.masterDataId)
                .SetModelPath(characterSnapInfo.model)
                .SetFirstStatus(characterSnapInfo.firstStatus)
				.SetSecondStatus(characterSnapInfo.secondStatus)
				.SetPosition(characterSnapInfo.position)
                .SetRotation(characterSnapInfo.rotation)
				.SetVelocity(characterSnapInfo.velocity)
				.SetAngularVelocity(characterSnapInfo.angularVelocity)
				.SetSelectableFirstStatusCount(0)
				.Build();
        }
        else if (entitySnapInfo.entityType == EntityType.Projectile)
        {
			ProjectileSnapInfo projectileSnapInfo = entitySnapInfo as ProjectileSnapInfo;

			entity = Projectile.Builder()
                .SetEntityID(projectileSnapInfo.entityId)
                .SetEntityRole(projectileSnapInfo.entityRole)
                .SetMasterDataID(projectileSnapInfo.masterDataId)
                .SetModelPath(projectileSnapInfo.model)
                .SetPosition(projectileSnapInfo.position)
                .SetRotation(projectileSnapInfo.rotation)
				.SetVelocity(projectileSnapInfo.velocity)
				.SetAngularVelocity(projectileSnapInfo.angularVelocity)
				.SetMovementSpeed(projectileSnapInfo.movementSpeed)
				.Build();
        }
		else if (entitySnapInfo.entityType == EntityType.GameItem)
		{
			GameItemSnapInfo gameItemSnapInfo = entitySnapInfo as GameItemSnapInfo;

			entity = GameItem.Builder()
				.SetEntityID(gameItemSnapInfo.entityId)
                .SetEntityRole(gameItemSnapInfo.entityRole)
                .SetMasterDataID(gameItemSnapInfo.masterDataId)
				.SetModelPath(gameItemSnapInfo.model)
				.SetPosition(gameItemSnapInfo.position)
				.SetRotation(gameItemSnapInfo.rotation)
				.SetVelocity(gameItemSnapInfo.velocity)
				.SetAngularVelocity(gameItemSnapInfo.angularVelocity)
				.SetHP(gameItemSnapInfo.HP)
				.SetMaximumHP(gameItemSnapInfo.maximumHP)
				.Build();
		}
		else
        {
            Debug.LogError("entitySnapInfo.m_EntityType is invalid! entitySnapInfo.m_EntityType : " + entitySnapInfo.entityType.ToString());
            return null;
        }

        return entity;
    }

    private void OnTick(int tick)
    {
        //  sort
        //  ...

        GetAllEntities<MonoEntityBase>()?.ForEach(entity =>
        {
            if (entity.IsValid)
            {
                entity.OnTick(tick);
            }
        });
    }
}
