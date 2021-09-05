﻿
namespace Entity
{
	public class GameItem : MonoEntityBase
    {
		#region Builder
		private static GameItemBuilder gameItemBuilder = new GameItemBuilder();
		public static GameItemBuilder Builder()
		{
			gameItemBuilder.Clear();
			return gameItemBuilder;
		}
		#endregion

        private GameItemBasicData gameItemBasicData = null;

        private BehaviorController behaviorController = null;
        private StateController stateController = null;

        private MasterData.GameItem masterData = null;
		public MasterData.GameItem MasterData
		{
			get
			{
				if (masterData == null)
				{
                    masterData = MasterDataManager.Instance.GetMasterData<MasterData.GameItem>(gameItemBasicData.MasterDataID);
				}

				return masterData;
			}
		}

		#region MonoEntityBase
		protected override void InitComponents()
		{
			base.InitComponents();

            gameItemBasicData = AttachEntityComponent(gameObject.AddComponent<GameItemBasicData>());

            entityBasicView = AttachEntityComponent(gameObject.AddComponent<EntityBasicView>());

            behaviorController = AttachEntityComponent(gameObject.AddComponent<BehaviorController>());
            stateController = AttachEntityComponent(gameObject.AddComponent<StateController>());
        }

		public override void Initialize(params object[] param)
		{
            base.Initialize(param);

            EntityID = (int)param[0];
			EntityType = EntityType.GameItem;
            EntityRole = (EntityRole)param[5];

            gameItemBasicData.Initialize(param[1], param[2], param[3], param[4]);
		}
		#endregion

        #region Interface For Convenience
        public override float MovementSpeed => gameItemBasicData.MovementSpeed;
        public override float FactoredMovementSpeed => gameItemBasicData.MovementSpeed * SubGameBase.Current.SubGameEnvironment.MoveSpeedFactor;

		public int CurrentHP
		{
			get { return gameItemBasicData.CurrentHP; }
			set { gameItemBasicData.CurrentHP = value; }
		}
		#endregion
	}
}
