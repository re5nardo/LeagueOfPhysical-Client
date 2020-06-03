
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

		//	Model
		private GameItemModel m_GameItemModel = null;

		//	View
		private BasicView m_BasicView = null;

		//	Controller
		private BasicController m_BasicController = null;

		private MasterData.GameItem m_MasterData__ = null;
		public MasterData.GameItem m_MasterData
		{
			get
			{
				if (m_MasterData__ == null)
				{
					m_MasterData__ = MasterDataManager.Instance.GetMasterData<MasterData.GameItem>(m_GameItemModel.MasterDataID);
				}

				return m_MasterData__;
			}
		}

		#region MonoEntityBase
		protected override void InitComponents()
		{
			base.InitComponents();

			//	Model
			m_GameItemModel = AttachComponent(gameObject.AddComponent<GameItemModel>());

			//	View
			m_BasicView = AttachComponent(gameObject.AddComponent<BasicView>());

			//	Controller
			m_BasicController = AttachComponent(gameObject.AddComponent<BasicController>());
		}

		public override void Initialize(params object[] param)
		{
			EntityID = (int)param[0];

			EntityType = EntityType.GameItem;

			m_GameItemModel.Initialize(param[1], param[2], param[3], param[4]);
		}
		#endregion

		#region Interface For Convenience
		public override float MovementSpeed { get { return m_GameItemModel.MovementSpeed; } }

		public int CurrentHP
		{
			get { return m_GameItemModel.CurrentHP; }
			set { m_GameItemModel.CurrentHP = value; }
		}
		#endregion
	}
}
