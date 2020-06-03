using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;

public class TickStatusChecker : MonoBehaviour
{
    [SerializeField] private Text localTick = null;
    [SerializeField] private Text serverTick = null;
    [SerializeField] private Image[] tickGapImage = null;

    private List<int> gaps = new List<int>();

    // Update is called once per frame
    void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (Game.Current == null)
            return;

        localTick.text = Game.Current.CurrentTick.ToString();
        serverTick.text = Game.Current.SyncTick.ToString();

        int gap = Game.Current.SyncTick - Game.Current.CurrentTick;

        gaps.Add(gap);

        if (gaps.Count > tickGapImage.Length)
        {
            gaps.RemoveRange(0, gaps.Count - tickGapImage.Length);
        }
        
        for(int i = 0; i < tickGapImage.Length; ++i)
        {
            if (gaps.Count <= i)
                continue;

            tickGapImage[i].color = gaps[i] > 10 ? Color.gray : gaps[i] > 3 ? Color.green : Color.red;
            tickGapImage[i].rectTransform.sizeDelta = new Vector2(tickGapImage[i].rectTransform.sizeDelta.x, Mathf.Min(160, gaps[i] * 20));
        }
    }
}
