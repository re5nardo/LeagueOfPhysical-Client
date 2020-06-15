
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

		private EntityBasicView entityBasicView = null;

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

            gameItemBasicData = AttachComponent(gameObject.AddComponent<GameItemBasicData>());

            entityBasicView = AttachComponent(gameObject.AddComponent<EntityBasicView>());

            behaviorController = AttachComponent(gameObject.AddComponent<BehaviorController>());
            stateController = AttachComponent(gameObject.AddComponent<StateController>());
        }

		public override void Initialize(params object[] param)
		{
			EntityID = (int)param[0];
			EntityType = EntityType.GameItem;
            EntityRole = (EntityRole)param[5];

            gameItemBasicData.Initialize(param[1], param[2], param[3], param[4]);
		}
		#endregion

		#region Interface For Convenience
		public override float MovementSpeed { get { return gameItemBasicData.MovementSpeed; } }

		public int CurrentHP
		{
			get { return gameItemBasicData.CurrentHP; }
			set { gameItemBasicData.CurrentHP = value; }
		}
		#endregion
	}
}
