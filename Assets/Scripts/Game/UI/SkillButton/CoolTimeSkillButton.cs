using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CoolTimeSkillButton : SkillButtonBase
{
	[SerializeField] private Image blackCover = null;
	[SerializeField] private Text textTime = null;

	private float coolTime = float.PositiveInfinity;
	private float remainTime = float.PositiveInfinity;

	private void Start()
	{
		Refresh();
	}

	private void Update()
	{
		if (skillID == -1 || float.IsInfinity(coolTime))
			return;

        remainTime -= Time.deltaTime;

		Refresh();
	}

	public void SetCoolTime(float fCoolTime)
	{
        coolTime = fCoolTime;
	}

	public void SetRemainTime(float fRemainTime)
	{
        remainTime = fRemainTime;
	}

	public void Refresh()
	{
		if (skillID == -1 || float.IsInfinity(coolTime))
		{
            blackCover.gameObject.SetActive(true);
            blackCover.fillAmount = 1;

            textTime.text = "∞";
		}
		else
		{
			if (remainTime > 0)
			{
                blackCover.gameObject.SetActive(true);
                blackCover.fillAmount = (remainTime / coolTime);

                textTime.text = string.Format("{0:#0.00}", remainTime);
			}
			else
			{
                blackCover.gameObject.SetActive(false);
                blackCover.fillAmount = 0;

                textTime.text = "";
			}
		}
	}

	#region Event Handler
	public override void OnClicked()
	{
		if (remainTime > 0)
			return;

		onClicked?.Invoke(skillID);
	}
	#endregion
}
