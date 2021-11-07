using UnityEngine;

namespace Entity
{
	public class Character : LOPMonoEntityBase
    {
		#region Builder
		private static CharacterBuilder characterBuilder = new CharacterBuilder();
		public static CharacterBuilder Builder()
		{
			characterBuilder.Clear();
			return characterBuilder;
		}
		#endregion

        public CharacterBasicData CharacterBasicData { get; private set; }
        public CharacterStatusData CharacterStatusData { get; private set; }
        public CharacterSkillData CharacterSkillData { get; private set; }

        public CharacterStatusController CharacterStatusController { get; private set; }

        private MasterData.Character masterData = null;
        public MasterData.Character MasterData => masterData ?? (masterData = MasterDataManager.Instance.GetMasterData<MasterData.Character>(CharacterBasicData.MasterDataId));

        #region LOPEntityBase
        protected override void InitEntity()
        {
            base.InitEntity();

            Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        protected override void InitEntityComponents()
		{
			base.InitEntityComponents();

            CharacterBasicData = AttachEntityComponent(gameObject.AddComponent<CharacterBasicData>());
            CharacterStatusData = AttachEntityComponent(gameObject.AddComponent<CharacterStatusData>());
            CharacterSkillData = AttachEntityComponent(gameObject.AddComponent<CharacterSkillData>());

            EntityBasicView = AttachEntityComponent(gameObject.AddComponent<CharacterView>());

            CharacterStatusController = AttachEntityComponent(gameObject.AddComponent<CharacterStatusController>());
		}

        protected override void OnInitialize(EntityCreationData entityCreationData)
		{
            base.OnInitialize(entityCreationData);

            CharacterBasicData.Initialize(entityCreationData);
            CharacterStatusData.Initialize(entityCreationData);
            CharacterSkillData.Initialize(entityCreationData);
        }
		#endregion

		#region Interface For Convenience
		public int Level
		{
			get => CharacterBasicData.Level;
            set => CharacterBasicData.Level = value;
        }

		public int HP
        {
			get => CharacterStatusData.HP;
            set => CharacterStatusData.HP = value;
        }

		public int MP
		{
			get => CharacterStatusData.MP;
            set => CharacterStatusData.MP = value;
        }

		public int MaximumHP => CharacterStatusData.MaximumHP;

        public int MaximumMP => CharacterStatusData.MaximumMP;

        public bool IsAlive => CharacterStatusData.HP > 0;

        public override float MovementSpeed => CharacterStatusData.MovementSpeed;
        public override float FactoredMovementSpeed => MovementSpeed * LOP.Game.Current.GameManager.MapData.mapEnvironment.MoveSpeedFactor;

        public int BasicAttackSkillID => CharacterSkillData.BasicAttackSkillID;
		#endregion
	}
}
