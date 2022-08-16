using UnityEngine;
using System.Collections;
using TMPro;
using GameFramework;
using UnityEngine.UI;
using System;

public class Entrance : MonoSingleton<Entrance>
{
    [Header("[Entrance Components]")]
    [SerializeField] private IntroComponent introComponent;
    [SerializeField] private InitAppComponent initAppComponent;
    [SerializeField] private LoginComponent loginComponent;
    [SerializeField] private CheckUserComponent checkUserComponent;
    [SerializeField] private JoinLobbyComponent joinLobbyComponent;
    [SerializeField] private CheckLocationComponent checkLocationComponent;

    [Header("[UI Components]")]
    public TextMeshProUGUI stateText;
    public Slider loadingBar;
    public TextMeshProUGUI loadingBarText;

    private EntranceComponentBase[] entranceComponents;

    protected override void Awake()
    {
        base.Awake();

        stateText.gameObject.SetActive(true);
        stateText.text = "";
        loadingBar.gameObject.SetActive(false);
        loadingBarText.text = "";

        entranceComponents = new EntranceComponentBase[]
        {
            //introComponent,
            initAppComponent,
            loginComponent,
            checkUserComponent,
            joinLobbyComponent,
            checkLocationComponent,
        };
    }

    private async void Start()
    {
        try
        {
            foreach (var entranceComponent in entranceComponents)
            {
                await entranceComponent.Execute();
            }
        }
        catch (Exception e)
        {
            Entrance.Instance.stateText.text = $"{e.Message}";
            Debug.LogError(e.Message);
        }
    }
}
