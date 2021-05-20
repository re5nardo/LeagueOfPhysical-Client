﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using GameFramework;
using UniRx;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private DirectionInputController moveController = null;
    [SerializeField] private DirectionInputController basicAttackController = null;

    [SerializeField] private TextureButton itemSlot1Btn = null;
	[SerializeField] private TextureButton itemSlot2Btn = null;

	[SerializeField] private CoolTimeSkillButton activeSkill1Btn = null;
	[SerializeField] private CoolTimeSkillButton activeSkill2Btn = null;
	[SerializeField] private CoolTimeSkillButton ultimateSkillBtn = null;

    private RoomProtocolDispatcher roomProtocolDispatcher = null;
    private Character Entity = null;
    private SkillInputData skillInputData = null;

    private void Update()
    {
        //  Skill
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
        Entity = Entities.Get<Character>(characterID);

		foreach(int skillID in Entity.MasterData.SkillIDs)
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

		itemSlot1Btn.button.onClick.AddListener(OnItemSlot1BtnClicked);
		itemSlot2Btn.button.onClick.AddListener(OnItemSlot2BtnClicked);

        activeSkill1Btn.onClicked += OnSkillBtnClicked;
		activeSkill2Btn.onClicked += OnSkillBtnClicked;
		ultimateSkillBtn.onClicked += OnSkillBtnClicked;

        roomProtocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();
        roomProtocolDispatcher[typeof(SC_EntitySkillInfo)] = OnSC_EntitySkillInfo;
	}

	private void OnDestroy()
	{
        moveController.onPress -= OnMoveControllerPress;
        moveController.onRelease -= OnMoveControllerRelease;
        moveController.onHold -= OnMoveControllerHold;

        basicAttackController.onRelease -= OnBasicAttackControllerRelease;
        basicAttackController.onHold -= OnBasicAttackControllerHold;

		itemSlot1Btn.button.onClick.RemoveListener(OnItemSlot1BtnClicked);
		itemSlot2Btn.button.onClick.RemoveListener(OnItemSlot2BtnClicked);

        activeSkill1Btn.onClicked -= OnSkillBtnClicked;
		activeSkill2Btn.onClicked -= OnSkillBtnClicked;
		ultimateSkillBtn.onClicked -= OnSkillBtnClicked;
	}

	#region Message Handler
	private void OnSC_EntitySkillInfo(IMessage msg)
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
	private void OnMoveControllerPress(Vector2 screenPosition)
	{
		if (!Entities.MyCharacter.IsAlive)
			return;

        var playerMoveInput = new PlayerMoveInput(inputType: PlayerMoveInput.InputType.Press);

        MessageBroker.Default.Publish(playerMoveInput);
    }

	private void OnMoveControllerRelease(Vector2 screenPosition)
	{
        if (!Entities.MyCharacter.IsAlive)
            return;

        var playerMoveInput = new PlayerMoveInput(inputType: PlayerMoveInput.InputType.Release);

        MessageBroker.Default.Publish(playerMoveInput);
    }

	private void OnMoveControllerHold(Vector2 holdPosition)
	{
		if (!Entities.MyCharacter.IsAlive)
            return;

        Vector2 pressedPosition = moveController.GetPressedPosition();

		if (holdPosition == pressedPosition)
            return;

        float y = Util.Math.FindDegree(holdPosition - pressedPosition) + LOP.Game.Current.GameUI.CameraController.GetRotation_Y();

		float x = Mathf.Sin(Mathf.Deg2Rad * y);
		float z = Mathf.Cos(Mathf.Deg2Rad * y);

        var playerMoveInput = new PlayerMoveInput(inputData: new Vector3(x, 0, z).normalized, inputType: PlayerMoveInput.InputType.Hold);

        MessageBroker.Default.Publish(playerMoveInput);

    }

	private void OnBasicAttackControllerRelease(Vector2 vec2ScreenPosition)
	{
        if (!Entities.MyCharacter.IsAlive)
            return;

        Vector2 vec2FinalDir = vec2ScreenPosition - basicAttackController.GetPressedPosition();

        if (vec2FinalDir.magnitude < 40)
        {
            skillInputData = new SkillInputData(Entities.MyEntityID, Entity.BasicAttackSkillID, Vector3.zero, Game.Current.GameTime);
        }
    }

	private void OnBasicAttackControllerHold(Vector2 vec2ScreenPosition)
	{
		if (!Entities.MyCharacter.IsAlive)
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
		
		skillInputData = new SkillInputData(Entities.MyEntityID, Entity.BasicAttackSkillID, dir.normalized, Game.Current.GameTime);
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
		notifySkillInputData.m_SkillInputData = new SkillInputData(Entities.MyEntityID, skillID, default, Game.Current.GameTime);

		RoomNetwork.Instance.Send(notifySkillInputData, PhotonNetwork.masterClient.ID, bInstant: true);
	}
	#endregion
}
