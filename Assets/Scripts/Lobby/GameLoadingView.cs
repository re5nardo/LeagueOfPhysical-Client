using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using TMPro;

public class GameLoadingView : MonoSingleton<GameLoadingView>
{
    [SerializeField] private TextMeshProUGUI tip = null;

    public static void Show()
    {
        Instance.gameObject.SetActive(true);

        Instance.Refresh();
    }

    public static void Hide()
    {
        Instance.gameObject.SetActive(false);
    }

    private void Refresh()
    {
        tip.text = "Tip 영역입니다! 오늘 하루도 좋은 하루 보내세요!";
    }

    public static GameLoadingView CreateInstance()
    {
        return Instantiate(Resources.Load<GameLoadingView>("UI/GameLoadingView"));
    }
}
