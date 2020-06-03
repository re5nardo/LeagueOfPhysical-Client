
namespace Entity
{
	public class Projectile : MonoEntityBase
    {
		#region Builder
		private static ProjectileBuilder projectileBuilder = new ProjectileBuilder();
		public static ProjectileBuilder Builder()
		{
			projectileBuilder.Clear();
			return projectileBuilder;
		}
		#endregion

		//	Model
		private ProjectileModel m_ProjectileModel = null;

		//	View
		private BasicView m_BasicView = null;

		//	Controller
		private BasicController m_BasicController = null;

		private MasterData.Projectile m_MasterData__ = null;
		public MasterData.Projectile m_MasterData
		{
			get
			{
				if (m_MasterData__ == null)
				{
					m_MasterData__ = MasterDataManager.Instance.GetMasterData<MasterData.Projectile>(m_ProjectileModel.MasterDataID);
				}

				return m_MasterData__;
			}
		}

		#region MonoEntityBase
		protected override void InitComponents()
		{
			base.InitComponents();

			//	Model
			m_ProjectileModel = AttachComponent(gameObject.AddComponent<ProjectileModel>());

			//	View
			m_BasicView = AttachComponent(gameObject.AddComponent<BasicView>());

			//	Controller
			m_BasicController = AttachComponent(gameObject.AddComponent<BasicController>());
		}

		public override void Initialize(params object[] param)
		{
			EntityID = (int)param[0];

			EntityType = EntityType.Projectile;

			m_ProjectileModel.Initialize(param[1], param[2], param[3]);
		}
		#endregion

		#region Interface For Convenience
		public override float MovementSpeed { get { return m_ProjectileModel.MovementSpeed; } }
		#endregion
	}
}
