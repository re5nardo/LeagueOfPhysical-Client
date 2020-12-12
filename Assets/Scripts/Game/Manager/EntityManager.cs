using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using System;
using GameFramework;
using EntityCommand;

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

    private Dictionary<int, Action<IMessage, int>> m_dicMessageHandler = new Dictionary<int, Action<IMessage, int>>();

    #region MonoBehaviour
    private void Awake()
    {
        m_PositionGrid = new Grid();
        m_PositionGrid.SetGrid(10);

        m_dicMessageHandler.Add(PhotonEvent.SC_EntityAppear, OnSC_EntityAppear);
		m_dicMessageHandler.Add(PhotonEvent.SC_EntityDisAppear, OnSC_EntityDisAppear);

		RoomNetwork.Instance.onMessage += OnMessage;

        TickPubSubService.AddSubscriber("Tick", OnTick);
	}

    private void OnDestroy()
    {
		m_dicMessageHandler.Clear();

		if (RoomNetwork.HasInstance())
		{
			RoomNetwork.Instance.onMessage -= OnMessage;
		}

        TickPubSubService.RemoveSubscriber("Tick", OnTick);
    }
#endregion

	#region Message Handler
	private void OnMessage(IMessage msg, object[] objects)
	{
		int nEventID = (msg as IPhotonEventMessage).GetEventID();
		int nSenderID = (int)objects[0];

		if (m_dicMessageHandler.ContainsKey(nEventID))
		{
			m_dicMessageHandler[nEventID](msg, nSenderID);
		}
	}

	private void OnSC_EntityAppear(IMessage msg, int nSenderID)
	{
		SC_EntityAppear entityAppear = msg as SC_EntityAppear;

		foreach (EntitySnapInfo entitySnapInfo in entityAppear.m_listEntitySnapInfo)
		{
			if(m_dicEntity.ContainsKey(entitySnapInfo.m_nEntityID))
			{
				Debug.LogError("[OnSC_EntityAppear] Entity already exists! EntityID : " + entitySnapInfo.m_nEntityID);
                continue;
			}

			var entity = CreateEntity(entitySnapInfo);

			if (entity.EntityID == Entities.MyEntityID)
			{
                EntityTransformInfo info = new EntityTransformInfo();
                info.m_nEntityID = entitySnapInfo.m_nEntityID;
                info.m_Position = entitySnapInfo.m_Position;
                info.m_Rotation = entitySnapInfo.m_Rotation;
                info.m_Velocity = entitySnapInfo.m_Velocity;
                info.m_GameTime = entityAppear.m_fGameTime;

                LOP.Game.Current.OnMyCharacterCreated(entity as Character);
			}
			else
			{
				EntityTransformInfo info = new EntityTransformInfo();
				info.m_nEntityID = entitySnapInfo.m_nEntityID;
				info.m_Position = entitySnapInfo.m_Position;
				info.m_Rotation = entitySnapInfo.m_Rotation;
				info.m_Velocity = entitySnapInfo.m_Velocity;
				info.m_GameTime = entityAppear.m_fGameTime;
			}

            (entity as MonoEntityBase).gameObject.AddComponent<TransformFinalController>();
		}
	}

	private void OnSC_EntityDisAppear(IMessage msg, int nSenderID)
	{
		SC_EntityDisAppear entityDisAppear = msg as SC_EntityDisAppear;

		foreach (int nEntityID in entityDisAppear.m_listEntityID)
		{
			if (!m_dicEntity.ContainsKey(nEntityID))
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

        if (entitySnapInfo.m_EntityType == EntityType.Character)
        {
			CharacterSnapInfo characterSnapInfo = entitySnapInfo as CharacterSnapInfo;

			entity = Character.Builder()
                .SetEntityID(characterSnapInfo.m_nEntityID)
                .SetEntityRole(characterSnapInfo.m_EntityRole)
                .SetMasterDataID(characterSnapInfo.m_nMasterDataID)
                .SetModelPath(characterSnapInfo.m_strModel)
                .SetFirstStatus(characterSnapInfo.m_FirstStatus)
				.SetSecondStatus(characterSnapInfo.m_SecondStatus)
				.SetPosition(characterSnapInfo.m_Position)
                .SetRotation(characterSnapInfo.m_Rotation)
				.SetVelocity(characterSnapInfo.m_Velocity)
				.SetAngularVelocity(characterSnapInfo.m_AngularVelocity)
				.SetSelectableFirstStatusCount(characterSnapInfo.m_nSelectableFirstStatusCount)
				.Build();
        }
        else if (entitySnapInfo.m_EntityType == EntityType.Projectile)
        {
			ProjectileSnapInfo projectileSnapInfo = entitySnapInfo as ProjectileSnapInfo;

			entity = Projectile.Builder()
                .SetEntityID(projectileSnapInfo.m_nEntityID)
                .SetEntityRole(projectileSnapInfo.m_EntityRole)
                .SetMasterDataID(projectileSnapInfo.m_nMasterDataID)
                .SetModelPath(projectileSnapInfo.m_strModel)
                .SetPosition(projectileSnapInfo.m_Position)
                .SetRotation(projectileSnapInfo.m_Rotation)
				.SetVelocity(projectileSnapInfo.m_Velocity)
				.SetAngularVelocity(projectileSnapInfo.m_AngularVelocity)
				.SetMovementSpeed(projectileSnapInfo.m_fMovementSpeed)
				.Build();
        }
		else if (entitySnapInfo.m_EntityType == EntityType.GameItem)
		{
			GameItemSnapInfo gameItemSnapInfo = entitySnapInfo as GameItemSnapInfo;

			entity = GameItem.Builder()
				.SetEntityID(gameItemSnapInfo.m_nEntityID)
                .SetEntityRole(gameItemSnapInfo.m_EntityRole)
                .SetMasterDataID(gameItemSnapInfo.m_nMasterDataID)
				.SetModelPath(gameItemSnapInfo.m_strModel)
				.SetPosition(gameItemSnapInfo.m_Position)
				.SetRotation(gameItemSnapInfo.m_Rotation)
				.SetVelocity(gameItemSnapInfo.m_Velocity)
				.SetAngularVelocity(gameItemSnapInfo.m_AngularVelocity)
				.SetHP(gameItemSnapInfo.m_nHP)
				.SetMaximumHP(gameItemSnapInfo.m_nMaximumHP)
				.Build();
		}
		else
        {
            Debug.LogError("entitySnapInfo.m_EntityType is invalid! entitySnapInfo.m_EntityType : " + entitySnapInfo.m_EntityType.ToString());
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
