using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using GameFramework;
using NetworkModel.Mirror;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private DirectionInputController moveController = null;
    [SerializeField] private DirectionInputController basicAttackController = null;

    [SerializeField] private TextureButton itemSlot1Btn = null;
	[SerializeField] private TextureButton itemSlot2Btn = null;

	[SerializeField] private CoolTimeSkillButton activeSkill1Btn = null;
	[SerializeField] private CoolTimeSkillButton activeSkill2Btn = null;
	[SerializeField] private CoolTimeSkillButton ultimateSkillBtn = null;

    [SerializeField] private TextureButton jumpBtn = null;

    private Character Entity = null;
    private SkillInputData skillInputData = null;

    private void Update()
    {
        //  Skill
        if (skillInputData != null)
        {
			using var disposer = PoolObjectDisposer<CS_NotifySkillInputData>.Get();
			var notifySkillInputData = disposer.PoolObject;
            notifySkillInputData.skillInputData = skillInputData;

            RoomNetwork.Instance.Send(notifySkillInputData, 0/*의미x*/, instant: true);

            skillInputData = null;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpBtnClicked();
        }
#endif
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

        jumpBtn.button.onClick.AddListener(OnJumpBtnClicked);

        SceneMessageBroker.AddSubscriber<SC_EntitySkillInfo>(OnSC_EntitySkillInfo);
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

        jumpBtn.button.onClick.RemoveListener(OnJumpBtnClicked);

        SceneMessageBroker.RemoveSubscriber<SC_EntitySkillInfo>(OnSC_EntitySkillInfo);
    }

	#region Message Handler
	private void OnSC_EntitySkillInfo(SC_EntitySkillInfo entitySkillInfo)
	{
		foreach (KeyValuePair<int, float> kv in entitySkillInfo.dicSkillInfo)
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

        SceneMessageBroker.Publish(playerMoveInput);
    }

	private void OnMoveControllerRelease(Vector2 screenPosition)
	{
        if (!Entities.MyCharacter.IsAlive)
            return;

        var playerMoveInput = new PlayerMoveInput(inputType: PlayerMoveInput.InputType.Release);

        SceneMessageBroker.Publish(playerMoveInput);
    }

	private void OnMoveControllerHold(Vector2 holdPosition)
	{
		if (!Entities.MyCharacter.IsAlive)
            return;

		if (holdPosition == moveController.PressedPosition)
            return;

        float y = Util.Math.FindDegree(holdPosition - moveController.PressedPosition) + LOP.Game.Current.GameUI.CameraController.RotationY;

		float x = Mathf.Sin(Mathf.Deg2Rad * y);
		float z = Mathf.Cos(Mathf.Deg2Rad * y);

        var playerMoveInput = new PlayerMoveInput(inputData: new Vector3(x, 0, z).normalized, inputType: PlayerMoveInput.InputType.Hold);

        SceneMessageBroker.Publish(playerMoveInput);

    }

	private void OnBasicAttackControllerRelease(Vector2 vec2ScreenPosition)
	{
        if (!Entities.MyCharacter.IsAlive)
            return;

        Vector2 vec2FinalDir = vec2ScreenPosition - basicAttackController.PressedPosition;

        if (vec2FinalDir.magnitude < 40)
        {
            skillInputData = new SkillInputData(Game.Current.CurrentTick, Entities.MyEntityID, Entity.BasicAttackSkillID, Vector3.zero);
        }
    }

	private void OnBasicAttackControllerHold(Vector2 vec2ScreenPosition)
	{
		if (!Entities.MyCharacter.IsAlive)
			return;

		Vector2 vec2FinalDir = vec2ScreenPosition - basicAttackController.PressedPosition;

		float y = Util.Math.FindDegree(vec2FinalDir) + LOP.Game.Current.GameUI.CameraController.RotationY;
		float x = Mathf.Sin(Mathf.Deg2Rad * y);
		float z = Mathf.Cos(Mathf.Deg2Rad * y);

		Vector3 dir = new Vector3(x, 0, z);

        if (vec2FinalDir.magnitude < 20)
        {
            dir = Vector3.zero;
        }
		
		skillInputData = new SkillInputData(Game.Current.CurrentTick, Entities.MyEntityID, Entity.BasicAttackSkillID, dir.normalized);
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
		using var disposer = PoolObjectDisposer<CS_NotifySkillInputData>.Get();
		var notifySkillInputData = disposer.PoolObject;
		notifySkillInputData.skillInputData = new SkillInputData(Game.Current.CurrentTick, Entities.MyEntityID, skillID, default);

		RoomNetwork.Instance.Send(notifySkillInputData, 0, instant: true);
	}

    private void OnJumpBtnClicked()
    {
        var jumpInputData = new JumpInputData();

        SceneMessageBroker.Publish(jumpInputData);
    }
    #endregion
}
