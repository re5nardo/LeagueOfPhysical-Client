using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using System;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : HealthBarBase
{
    [SerializeField] private TextMeshProUGUI nickname = null;
    [SerializeField] private TextMeshProUGUI level = null;
    [SerializeField] private Slider sliderHP = null;
    [SerializeField] private Slider sliderMP = null;
    [SerializeField] private Slider sliderExp = null;

    private WeakReference targetEntity = null;
    private Transform tfMine = null;

    private RectTransform rectTransformParent = null;
    private RectTransform RectTransformParent { get { return rectTransformParent ?? (rectTransformParent = transform.parent?.GetComponent<RectTransform>()); } }

	private void Awake()
    {
        m_HealthBarType = HealthBarType.Player;
        tfMine = transform;
    }

    private void LateUpdate()
    {
        if (targetEntity != null && targetEntity.IsAlive && RectTransformParent != null)
        {
            Character character = targetEntity.Target as Character;

            //  UI
            level.text = character.Level.ToString();
            sliderHP.value = character.MaximumHP == 0 ? 0 : (float)character.CurrentHP / character.MaximumHP;
            sliderMP.value = character.MaximumMP == 0 ? 0 : (float)character.CurrentMP / character.MaximumMP;
            //            m_sliderExp.value = character.GetCurrentExpPercent() / 100.0f;

            //  Position
            var center = Util.UGUI.ConvertScreenToLocalPoint(RectTransformParent, Camera.main.WorldToScreenPoint(character.Position));
            tfMine.localPosition = new Vector3(center.x, center.y + 50, center.z);
        }
    }
	
	public void SetTarget(Character character)
    {
        targetEntity = new WeakReference(character);

        GlobalMonoBehavior.StartCoroutine(WaitForMasterData(character));
      
        level.text = character.Level.ToString();
        sliderHP.value = character.MaximumHP == 0 ? 0 : (float)character.CurrentHP / character.MaximumHP;
        sliderMP.value = character.MaximumMP == 0 ? 0 : (float)character.CurrentMP / character.MaximumMP;
        //        m_sliderExp.value = character.GetCurrentExpPercent() / 100.0f;
    }

    private IEnumerator WaitForMasterData(Character character)
    {
        yield return new WaitUntil(() => character.MasterData != null);

        nickname.text = string.Format("{0}_{1}", character.MasterData.Name, character.EntityID);
    }

	public void Clear()
    {
        targetEntity = null;
        nickname.text = "";
        level.text = "";
        sliderHP.value = 1.0f;
        sliderMP.value = 1.0f;
        sliderExp.value = 1.0f;
    }
}
