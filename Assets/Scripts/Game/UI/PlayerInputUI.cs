using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using System;
using GameFramework;

public class PlayerInputUI : MonoBehaviour
{
    [SerializeField] private DirectionInputController moveController = null;
    [SerializeField] private DirectionInputController basicAttackController = null;

    [SerializeField] private UITextureButton itemSlot1Btn = null;
	[SerializeField] private UITextureButton itemSlot2Btn = null;

	[SerializeField] private CoolTimeSkillButton activeSkill1Btn = null;
	[SerializeField] private CoolTimeSkillButton activeSkill2Btn = null;
	[SerializeField] private CoolTimeSkillButton ultimateSkillBtn = null;

	private Dictionary<int, Action<IMessage, int>> m_dicMessageHandler = new Dictionary<int, Action<IMessage, int>>();

	private long sequence = 0;
	private PlayerMoveInput playerMoveInput = null;
	private SkillInputData skillInputData = null;
    private Character m_Entity = null;

	private void LateUpdate()
	{
		if (playerMoveInput != null)
		{
			playerMoveInput.m_lSequence = sequence++;
			playerMoveInput.m_nEntityID = EntityManager.Instance.GetMyEntityID();
			playerMoveInput.m_fGameTime = Game.Current.GameTime - Time.deltaTime;

			CS_NotifyMoveInputData notifyMoveInputData = new CS_NotifyMoveInputData();
			notifyMoveInputData.m_PlayerMoveInput = playerMoveInput;

			RoomNetwork.Instance.Send(notifyMoveInputData, PhotonNetwork.masterClient.ID, bInstant: true);

			playerMoveInput = null;
		}

		if (skillInputData != null)
		{
			CS_NotifySkillInputData notifySkillInputData = new CS_NotifySkillInputData();
			notifySkillInputData.m_SkillInputData = skillInputData;

			RoomNetwork.Instance.Send(notifySkillInputData, PhotonNetwork.masterClient.ID, bInstant: true);

			skillInputData = null;
		}
	}

	public void SetCharacterID(int characterID)
	{
        m_Entity = EntityManager.Instance.GetEntity(characterID) as Character;

		foreach(int skillID in m_Entity.MasterData.SkillIDs)
		{
			MasterData.Skill masterSkill = MasterDataManager.Instance.GetMasterData<MasterData.Skill>(skillID);

			switch(masterSkill.SkillType)
			{
				case "ActiveSkill1":
					activeSkill1Btn.textureIcon.texture = Resources.Load<Texture>(masterSkill.IconResID);
					activeSkill1Btn.SetSkillID(skillID);
					activeSkill1Btn.SetCoolTime(masterSkill.CoolTime);
					activeSkill1Btn.SetRemainTime(masterSkill.CoolTime);
					break;

				case "ActiveSkill2":
					activeSkill2Btn.textureIcon.texture = Resources.Load<Texture>(masterSkill.IconResID);
					activeSkill2Btn.SetSkillID(skillID);
					activeSkill2Btn.SetCoolTime(masterSkill.CoolTime);
					activeSkill2Btn.SetRemainTime(masterSkill.CoolTime);
					break;

				case "UltimateSkill":
					ultimateSkillBtn.textureIcon.texture = Resources.Load<Texture>(masterSkill.IconResID);
					ultimateSkillBtn.SetSkillID(skillID);
					ultimateSkillBtn.SetCoolTime(masterSkill.CoolTime);
					ultimateSkillBtn.SetRemainTime(masterSkill.CoolTime);
					break;
			}
		}
	}

	private void Awake()
	{
        moveController.onPress += OnMoveControllerPress;
        moveController.onRelease += OnMoveControllerRelease;
        moveController.onHold += OnMoveControllerHold;

        basicAttackController.onRelease += OnBasicAttackControllerRelease;
        basicAttackController.onHold += OnBasicAttackControllerHold;

		itemSlot1Btn.onClicked += OnItemSlot1BtnClicked;
		itemSlot2Btn.onClicked += OnItemSlot2BtnClicked;

		activeSkill1Btn.onClicked += OnSkillBtnClicked;
		activeSkill2Btn.onClicked += OnSkillBtnClicked;
		ultimateSkillBtn.onClicked += OnSkillBtnClicked;

		m_dicMessageHandler.Add(PhotonEvent.SC_EntitySkillInfo, OnSC_EntitySkillInfo);

		RoomNetwork.Instance.onMessage += OnMessage;
	}

	private void OnDestroy()
	{
        moveController.onPress -= OnMoveControllerPress;
        moveController.onRelease -= OnMoveControllerRelease;
        moveController.onHold -= OnMoveControllerHold;

        basicAttackController.onRelease -= OnBasicAttackControllerRelease;
        basicAttackController.onHold -= OnBasicAttackControllerHold;

		itemSlot1Btn.onClicked -= OnItemSlot1BtnClicked;
		itemSlot2Btn.onClicked -= OnItemSlot2BtnClicked;

		activeSkill1Btn.onClicked -= OnSkillBtnClicked;
		activeSkill2Btn.onClicked -= OnSkillBtnClicked;
		ultimateSkillBtn.onClicked -= OnSkillBtnClicked;

		m_dicMessageHandler.Clear();

		if (RoomNetwork.IsInstantiated())
		{
			RoomNetwork.Instance.onMessage -= OnMessage;
		}
	}

	#region Message Handler
	private void OnMessage(IMessage msg, object[] objects)
	{
		int nEventID = (msg as IPhotonEventMessage).GetEventID();
		int nSenderID = (int)objects[0];

		if (m_dicMessageHandler.ContainsKey(nEventID))
		{
			m_dicMessageHandler[nEventID](msg, nSenderID);
		}
	}

	private void OnSC_EntitySkillInfo(IMessage msg, int nSenderID)
	{
		SC_EntitySkillInfo entitySkillInfo = msg as SC_EntitySkillInfo;

		foreach (KeyValuePair<int, float> kv in entitySkillInfo.m_dicSkillInfo)
		{
			MasterData.Skill masterSkill = MasterDataManager.Instance.GetMasterData<MasterData.Skill>(kv.Key);

			switch (masterSkill.SkillType)
			{
				case "ActiveSkill1":
					activeSkill1Btn.SetRemainTime(kv.Value);
					break;

				case "ActiveSkill2":
					activeSkill2Btn.SetRemainTime(kv.Value);
					break;

				case "UltimateSkill":
					ultimateSkillBtn.SetRemainTime(kv.Value);
					break;
			}
		}
	}
	#endregion

	#region Event Handler
	private void OnMoveControllerPress(Vector2 vec2ScreenPosition)
	{
		if (!EntityManager.Instance.GetMyCharacter().IsAlive)
			return;

		playerMoveInput = new PlayerMoveInput(inputType: PlayerMoveInput.InputType.Press);
	}

	private void OnMoveControllerRelease(Vector2 vec2ScreenPosition)
	{
		playerMoveInput = new PlayerMoveInput(inputType: PlayerMoveInput.InputType.Release);
	}

	private void OnMoveControllerHold(Vector2 vec2ScreenPosition)
	{
		Character character = EntityManager.Instance.GetMyCharacter();
		if (!character.IsAlive)
			return;

		//	임시 코드.. 나중에 클래스 프로퍼티 or State로 빼서 처리해야 함
		if (character.GetComponent<Behavior.BehaviorBase>() != null)
			return;

		Vector2 vec2DirectionKeyForMoveStart = moveController.GetPressedPosition();

		if (vec2ScreenPosition == vec2DirectionKeyForMoveStart)
			return;
		
		Vector2 vec2FinalDir = vec2ScreenPosition - vec2DirectionKeyForMoveStart;

		float y = Util.Math.FindDegree(vec2FinalDir);
		y += LOP.Game.Current.GameUI.CameraController.GetRotation_Y();

		float x = Mathf.Sin(Mathf.Deg2Rad * y);
		float z = Mathf.Cos(Mathf.Deg2Rad * y);

		Vector3 dir = new Vector3(x, 0, z);

		playerMoveInput = new PlayerMoveInput(inputData: dir.normalized * Time.deltaTime, inputType: PlayerMoveInput.InputType.Hold);
	}

	private void OnBasicAttackControllerRelease(Vector2 vec2ScreenPosition)
	{
        Character character = EntityManager.Instance.GetMyCharacter();
        if (!character.IsAlive)
            return;

        //	임시 코드.. 나중에 클래스 프로퍼티 or State로 빼서 처리해야 함
        if (character.GetComponent<Behavior.BehaviorBase>() != null)
            return;

        Vector2 vec2FinalDir = vec2ScreenPosition - basicAttackController.GetPressedPosition();

        if (vec2FinalDir.magnitude < 40)
        {
            skillInputData = new SkillInputData(character.EntityID, m_Entity.BasicAttackSkillID, Vector3.zero, Game.Current.GameTime);
        }
    }

	private void OnBasicAttackControllerHold(Vector2 vec2ScreenPosition)
	{
		Character character = EntityManager.Instance.GetMyCharacter();
		if (!character.IsAlive)
			return;

		//	임시 코드.. 나중에 클래스 프로퍼티 or State로 빼서 처리해야 함
		if (character.GetComponent<Behavior.BehaviorBase>() != null)
			return;

		Vector2 vec2FinalDir = vec2ScreenPosition - basicAttackController.GetPressedPosition();

		float y = Util.Math.FindDegree(vec2FinalDir);
		y += LOP.Game.Current.GameUI.CameraController.GetRotation_Y();

		float x = Mathf.Sin(Mathf.Deg2Rad * y);
		float z = Mathf.Cos(Mathf.Deg2Rad * y);

		Vector3 dir = new Vector3(x, 0, z);

        if (vec2FinalDir.magnitude < 20)
        {
            dir = Vector3.zero;
        }
		
		skillInputData = new SkillInputData(character.EntityID, m_Entity.BasicAttackSkillID, dir.normalized, Game.Current.GameTime);
	}

	private void OnItemSlot1BtnClicked()
	{
		Debug.Log("OnItemSlot1BtnClicked");
	}

	private void OnItemSlot2BtnClicked()
	{
		Debug.Log("OnItemSlot2BtnClicked");
	}

	private void OnSkillBtnClicked(int skillID)
	{
		CS_NotifySkillInputData notifySkillInputData = new CS_NotifySkillInputData();
		notifySkillInputData.m_SkillInputData = new SkillInputData(EntityManager.Instance.GetMyEntityID(), skillID, default, Game.Current.GameTime);

		RoomNetwork.Instance.Send(notifySkillInputData, PhotonNetwork.masterClient.ID, bInstant: true);
	}
	#endregion
}
