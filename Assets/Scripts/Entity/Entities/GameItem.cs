
namespace Entity
{
	public class GameItem : LOPEntityBase
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
                    masterData = MasterDataManager.Instance.GetMasterData<MasterData.GameItem>(gameItemBasicData.MasterDataId);
				}

				return masterData;
			}
		}

        #region LOPEntityBase
        protected override void InitEntityComponents()
		{
			base.InitEntityComponents();

            gameItemBasicData = AttachEntityComponent(gameObject.AddComponent<GameItemBasicData>());

            entityBasicView = AttachEntityComponent(gameObject.AddComponent<EntityBasicView>());

            behaviorController = AttachEntityComponent(gameObject.AddComponent<BehaviorController>());
            stateController = AttachEntityComponent(gameObject.AddComponent<StateController>());
        }

		public override void Initialize(EntityCreationData entityCreationData)
		{
            base.Initialize(entityCreationData);

            gameItemBasicData.Initialize(entityCreationData);
		}
		#endregion

        #region Interface For Convenience
        public override float MovementSpeed => gameItemBasicData.MovementSpeed;
        public override float FactoredMovementSpeed => MovementSpeed * SubGameBase.Current.SubGameEnvironment.MoveSpeedFactor;

		public int HP
        {
			get => gameItemBasicData.HP;
            set => gameItemBasicData.HP = value;
        }
		#endregion
	}
}
