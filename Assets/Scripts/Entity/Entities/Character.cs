using UnityEngine;

namespace Entity
{
	public class Character : MonoEntityBase
    {
		#region Builder
		private static CharacterBuilder characterBuilder = new CharacterBuilder();
		public static CharacterBuilder Builder()
		{
			characterBuilder.Clear();
			return characterBuilder;
		}
		#endregion

        private CharacterBasicData characterBasicData = null;
		private CharacterStatusData characterStatusData = null;
        private CharacterSkillData characterSkillData = null;

        private BehaviorController behaviorController = null;
        private StateController stateController = null;
        private CharacterStatusController characterStatusController = null;
		//private SkillController m_SkillController = null;

		private MasterData.Character masterData = null;
		public MasterData.Character MasterData
		{
			get
			{
				if (masterData == null)
				{
                    masterData = MasterDataManager.Instance.GetMasterData<MasterData.Character>(characterBasicData.MasterDataID);
				}

				return masterData;
			}
		}

		#region MonoEntityBase
		protected override void InitEntityComponents()
		{
			base.InitEntityComponents();

            characterBasicData = AttachEntityComponent(gameObject.AddComponent<CharacterBasicData>());
            characterStatusData = AttachEntityComponent(gameObject.AddComponent<CharacterStatusData>());
            characterSkillData = AttachEntityComponent(gameObject.AddComponent<CharacterSkillData>());

            entityBasicView = AttachEntityComponent(gameObject.AddComponent<CharacterView>());

            behaviorController = AttachEntityComponent(gameObject.AddComponent<BehaviorController>());
            stateController = AttachEntityComponent(gameObject.AddComponent<StateController>());
            characterStatusController = AttachEntityComponent(gameObject.AddComponent<CharacterStatusController>());
			//m_SkillController = AttachComponent(gameObject.AddComponent<SkillController>());
		}

		public override void Initialize(EntityCreationData entityCreationData)
		{
            base.Initialize(entityCreationData);

            characterBasicData.Initialize(entityCreationData);
            characterStatusData.Initialize(entityCreationData);
            characterSkillData.Initialize(entityCreationData);
        }
		#endregion

		#region Interface For Convenience
		public int Level
		{
			get { return characterBasicData.Level; }
			set { characterBasicData.Level = value; }
		}

		public int CurrentHP 
		{
			get { return characterStatusData.CurrentHP; }
			set { characterStatusData.CurrentHP = value; }
		}

		public int CurrentMP
		{
			get { return characterStatusData.CurrentMP; }
			set { characterStatusData.CurrentHP = value; }
		}

		public int MaximumHP { get { return characterStatusData.MaximumHP; } }

		public int MaximumMP { get { return characterStatusData.MaximumMP; } }

		public bool IsAlive { get { return characterStatusData.CurrentHP > 0; } }

		public bool IsSelectableFirstStatus { get { return characterStatusData.SelectableFirstStatusCount > 0; } }

        public override float MovementSpeed => characterStatusData.MovementSpeed;
        public override float FactoredMovementSpeed => characterStatusData.MovementSpeed * SubGameBase.Current.SubGameEnvironment.MoveSpeedFactor;

        public int BasicAttackSkillID { get { return characterSkillData.BasicAttackSkillID;  } }
		#endregion
	}
}
