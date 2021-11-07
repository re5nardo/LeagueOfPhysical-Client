
namespace Entity
{
	public class Projectile : LOPMonoEntityBase
    {
		#region Builder
		private static ProjectileBuilder projectileBuilder = new ProjectileBuilder();
		public static ProjectileBuilder Builder()
		{
			projectileBuilder.Clear();
			return projectileBuilder;
		}
		#endregion

        public ProjectileBasicData ProjectileBasicData { get; private set; }

        private MasterData.Projectile masterData = null;
        public MasterData.Projectile MasterData => masterData ?? (masterData = MasterDataManager.Instance.GetMasterData<MasterData.Projectile>(ProjectileBasicData.MasterDataId));

        #region LOPEntityBase
        protected override void InitEntityComponents()
		{
			base.InitEntityComponents();

            ProjectileBasicData = AttachEntityComponent(gameObject.AddComponent<ProjectileBasicData>());

            EntityBasicView = AttachEntityComponent(gameObject.AddComponent<EntityBasicView>());
        }

        protected override void OnInitialize(EntityCreationData entityCreationData)
		{
            base.OnInitialize(entityCreationData);

            ProjectileBasicData.Initialize(entityCreationData);
		}
		#endregion

        #region Interface For Convenience
        public override float MovementSpeed => ProjectileBasicData.MovementSpeed;
        public override float FactoredMovementSpeed => MovementSpeed * LOP.Game.Current.GameManager.MapData.mapEnvironment.MoveSpeedFactor;
        #endregion
    }
}
