using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;

public class CoolTimeSkillButton : SkillButtonBase
{
	[SerializeField] private Image blackCover = null;
	[SerializeField] private Text textTime = null;

	private float coolTime = float.PositiveInfinity;
	private float remainTime = float.PositiveInfinity;

    private int lastUpdateTick = -1;

	private void Start()
	{
		Refresh();
	}

	private void Update()
	{
		if (skillID == -1 || float.IsInfinity(coolTime))
			return;

        if (Game.Current != null && Game.Current.CurrentTick != lastUpdateTick)
        {
            remainTime -= Game.Current.TickInterval;
            
            Refresh();

            lastUpdateTick = Game.Current.CurrentTick;
        }
	}

	public void SetCoolTime(float coolTime)
	{
        this.coolTime = coolTime;
	}

	public void SetRemainTime(float remainTime)
	{
        this.remainTime = remainTime;
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

        base.OnClicked();
	}
	#endregion
}
