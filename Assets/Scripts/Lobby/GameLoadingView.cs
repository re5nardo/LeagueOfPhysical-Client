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
        tip.text = "Tip �����Դϴ�! ���� �Ϸ絵 ���� �Ϸ� ��������!";
    }

    public static GameLoadingView CreateInstance()
    {
        return Instantiate(Resources.Load<GameLoadingView>("UI/GameLoadingView"));
    }
}
