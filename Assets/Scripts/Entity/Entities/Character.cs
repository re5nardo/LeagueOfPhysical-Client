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

		//	Model
		private CharacterModel m_CharacterModel = null;
		private CharacterStatusModel m_CharacterStatusModel = null;
        private CharacterSkillModel m_CharacterSkillModel = null;

		//	View
		private CharacterView m_CharacterView = null;

		//	Controller
		private BasicController m_BasicController = null;
		private CharacterStatusController m_CharacterStatusController = null;
		//private SkillController m_SkillController = null;

		private MasterData.Character m_MasterData__ = null;
		public MasterData.Character m_MasterData
		{
			get
			{
				if (m_MasterData__ == null)
				{
					m_MasterData__ = MasterDataManager.Instance.GetMasterData<MasterData.Character>(m_CharacterModel.MasterDataID);
				}

				return m_MasterData__;
			}
		}

		#region MonoEntityBase
		protected override void InitComponents()
		{
			base.InitComponents();

			//	Model
			m_CharacterModel = AttachComponent(gameObject.AddComponent<CharacterModel>());
			m_CharacterStatusModel = AttachComponent(gameObject.AddComponent<CharacterStatusModel>());
            m_CharacterSkillModel = AttachComponent(gameObject.AddComponent<CharacterSkillModel>());

			//	View
			m_CharacterView = AttachComponent(gameObject.AddComponent<CharacterView>());

			//	Controller
			m_BasicController = AttachComponent(gameObject.AddComponent<BasicController>());
			m_CharacterStatusController = AttachComponent(gameObject.AddComponent<CharacterStatusController>());
			//m_SkillController = AttachComponent(gameObject.AddComponent<SkillController>());
		}

		public override void Initialize(params object[] param)
		{
			EntityID = (int)param[0];

			EntityType = EntityType.Character;

			m_CharacterModel.Initialize(param[1], param[2]);
			m_CharacterStatusModel.Initialize(param[3], param[4], param[5]);
            m_CharacterSkillModel.Initialize();
        }
		#endregion

		#region Interface For Convenience
		public int Level
		{
			get { return m_CharacterModel.Level; }
			set { m_CharacterModel.Level = value; }
		}

		public int CurrentHP 
		{
			get { return m_CharacterStatusModel.CurrentHP; }
			set { m_CharacterStatusModel.CurrentHP = value; }
		}

		public int CurrentMP
		{
			get { return m_CharacterStatusModel.CurrentMP; }
			set { m_CharacterStatusModel.CurrentHP = value; }
		}

		public int MaximumHP { get { return m_CharacterStatusModel.MaximumHP; } }

		public int MaximumMP { get { return m_CharacterStatusModel.MaximumMP; } }

		public bool IsAlive { get { return m_CharacterStatusModel.CurrentHP > 0; } }

		public bool IsSelectableFirstStatus { get { return m_CharacterStatusModel.SelectableFirstStatusCount > 0; } }

		public Transform ModelTransform { get { return m_CharacterView.ModelTransform; } }
	
		public override float MovementSpeed { get { return m_CharacterStatusModel.MovementSpeed; } }

        public int BasicAttackSkillID { get { return m_CharacterSkillModel.BasicAttackSkillID;  } }
		#endregion
	}
}
