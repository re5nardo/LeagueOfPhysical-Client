using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;

public class TickStatusChecker : MonoBehaviour
{
    [SerializeField] private Text localTick = null;
    [SerializeField] private Text syncTick = null;
    [SerializeField] private Image[] tickGapImage = null;

    private List<int> gaps = new List<int>();

    private void Update()
    {
        if (!Game.Current.Initialized)
            return;

        Refresh();
    }

    private void Refresh()
    {
        localTick.text = Game.Current.CurrentTick.ToString();
        syncTick.text = Game.Current.SyncTick.ToString();

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

            Color color = default;
            switch(gaps[i])
            {
                case 0: color = Color.red; break;
                case 1: color = Color.white; break;
                case 2: color = Color.yellow; break;
                case 3: color = Color.green; break;
                case 4: color = Color.blue; break;
                case 5: color = Color.cyan; break;
                case 6: color = Color.magenta; break;
                case 7: color = Color.gray; break;
                default: color = Color.black; break;
            }
            tickGapImage[i].color = color;

            tickGapImage[i].rectTransform.sizeDelta = new Vector2(tickGapImage[i].rectTransform.sizeDelta.x, gaps[i] * 10);
        }
    }
}
