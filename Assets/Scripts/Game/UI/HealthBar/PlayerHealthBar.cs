using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using System;
using UnityEngine.UI;

public class PlayerHealthBar : HealthBarBase
{
    [SerializeField] private Text nickname = null;
    [SerializeField] private Text level = null;
    [SerializeField] private Slider sliderHP = null;
    [SerializeField] private Slider sliderMP = null;
    [SerializeField] private Slider sliderExp = null;

    private WeakReference targetEntity = null;
    private Transform trMine = null;
    private RectTransform rtMine = null;
    private RectTransform rtCanvas = null;

    private Vector3 screenOffset = new Vector3(0, Screen.height * 0.08f, 0);

	private void Awake()
    {
        m_HealthBarType = HealthBarType.Player;
        trMine = transform;
        rtMine = GetComponent<RectTransform>();
        rtCanvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (targetEntity != null && targetEntity.IsAlive)
        {
            Character character = targetEntity.Target as Character;

            //  UI
            level.text = character.Level.ToString();
            sliderHP.value = (float)character.CurrentHP / character.MaximumHP;
            sliderMP.value = (float)character.CurrentMP / character.MaximumMP;
//            m_sliderExp.value = character.GetCurrentExpPercent() / 100.0f;

            //  Position
            Vector3 vec3Pos = Camera.main.WorldToScreenPoint(character.Position);
            //vec3Pos.z = 0;

            rtMine.anchoredPosition = Util.UGUI.ConvertWorldToCanvas(vec3Pos + screenOffset, rtCanvas);
        }
    }

	
	public void SetTarget(Character character)
    {
        targetEntity = new WeakReference(character);

        GlobalMonoBehavior.StartCoroutine(WaitForMasterData(character));
      
        level.text = character.Level.ToString();
        sliderHP.value = (float)character.CurrentHP / character.MaximumHP;
        sliderMP.value = (float)character.CurrentMP / character.MaximumMP;
//        m_sliderExp.value = character.GetCurrentExpPercent() / 100.0f;
    }

    private IEnumerator WaitForMasterData(Character character)
    {
        yield return new WaitUntil(() => character.m_MasterData != null);

        nickname.text = string.Format("{0}_{1}", character.m_MasterData.Name, character.EntityID);
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
