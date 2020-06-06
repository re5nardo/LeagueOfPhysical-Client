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

		private CharacterView characterView = null;

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
		protected override void InitComponents()
		{
			base.InitComponents();

            characterBasicData = AttachComponent(gameObject.AddComponent<CharacterBasicData>());
            characterStatusData = AttachComponent(gameObject.AddComponent<CharacterStatusData>());
            characterSkillData = AttachComponent(gameObject.AddComponent<CharacterSkillData>());

            characterView = AttachComponent(gameObject.AddComponent<CharacterView>());

            behaviorController = AttachComponent(gameObject.AddComponent<BehaviorController>());
            stateController = AttachComponent(gameObject.AddComponent<StateController>());
            characterStatusController = AttachComponent(gameObject.AddComponent<CharacterStatusController>());
			//m_SkillController = AttachComponent(gameObject.AddComponent<SkillController>());
		}

		public override void Initialize(params object[] param)
		{
			EntityID = (int)param[0];

			EntityType = EntityType.Character;

            characterBasicData.Initialize(param[1], param[2]);
            characterStatusData.Initialize(param[3], param[4], param[5]);
            characterSkillData.Initialize();
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

		public Transform ModelTransform { get { return characterView.ModelTransform; } }
	
		public override float MovementSpeed { get { return characterStatusData.MovementSpeed; } }

        public int BasicAttackSkillID { get { return characterSkillData.BasicAttackSkillID;  } }
		#endregion
	}
}
