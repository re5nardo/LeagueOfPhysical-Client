
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

        private ProjectileBasicData projectileBasicData = null;

        private BehaviorController behaviorController = null;
        private StateController stateController = null;

        private MasterData.Projectile masterData = null;
		public MasterData.Projectile MasterData
		{
			get
			{
				if (masterData == null)
				{
                    masterData = MasterDataManager.Instance.GetMasterData<MasterData.Projectile>(projectileBasicData.MasterDataId);
				}

				return masterData;
			}
		}

		#region MonoEntityBase
		protected override void InitEntityComponents()
		{
			base.InitEntityComponents();

            projectileBasicData = AttachEntityComponent(gameObject.AddComponent<ProjectileBasicData>());

            entityBasicView = AttachEntityComponent(gameObject.AddComponent<EntityBasicView>());

            behaviorController = AttachEntityComponent(gameObject.AddComponent<BehaviorController>());
            stateController = AttachEntityComponent(gameObject.AddComponent<StateController>());
        }

		public override void Initialize(EntityCreationData entityCreationData)
		{
            base.Initialize(entityCreationData);

            projectileBasicData.Initialize(entityCreationData);
		}
		#endregion

        #region Interface For Convenience
        public override float MovementSpeed => projectileBasicData.MovementSpeed;
        public override float FactoredMovementSpeed => MovementSpeed * SubGameBase.Current.SubGameEnvironment.MoveSpeedFactor;
        #endregion
    }
}
