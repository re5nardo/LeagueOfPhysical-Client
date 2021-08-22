﻿
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
                    masterData = MasterDataManager.Instance.GetMasterData<MasterData.Projectile>(projectileBasicData.MasterDataID);
				}

				return masterData;
			}
		}

		#region MonoEntityBase
		protected override void InitComponents()
		{
			base.InitComponents();

            projectileBasicData = AttachComponent(gameObject.AddComponent<ProjectileBasicData>());

            entityBasicView = AttachComponent(gameObject.AddComponent<EntityBasicView>());

            behaviorController = AttachComponent(gameObject.AddComponent<BehaviorController>());
            stateController = AttachComponent(gameObject.AddComponent<StateController>());
        }

		public override void Initialize(params object[] param)
		{
            base.Initialize(param);

            EntityID = (int)param[0];
			EntityType = EntityType.Projectile;
            EntityRole = (EntityRole)param[4];

            projectileBasicData.Initialize(param[1], param[2], param[3]);
		}
		#endregion

        #region Interface For Convenience
        public override float MovementSpeed => projectileBasicData.MovementSpeed;
        public override float FactoredMovementSpeed => projectileBasicData.MovementSpeed * SubGameBase.Current.SubGameEnvironment.MoveSpeedFactor;
        #endregion
    }
}
