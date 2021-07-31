using UnityEngine;
using System.Collections;
using TMPro;
using GameFramework;
using UnityEngine.UI;

public class Entrance : MonoSingleton<Entrance>
{
    [Header("[Entrance Components]")]
    [SerializeField] private IntroComponent introComponent;
    [SerializeField] private InitAppComponent initAppComponent;
    [SerializeField] private LoginComponent loginComponent;
    [SerializeField] private PunConnectComponent punConnectComponent;
    [SerializeField] private CheckLocationComponent checkLocationComponent;

    [Header("[UI Components]")]
    public TextMeshProUGUI stateText;
    public Slider loadingBar;
    public TextMeshProUGUI loadingBarText;

    protected override void Awake()
    {
        base.Awake();

        stateText.gameObject.SetActive(true);
        stateText.text = "";
        loadingBar.gameObject.SetActive(false);
        loadingBarText.text = "";
    }

    #region MonoBehaviour
    private IEnumerator Start()
    {
        //yield return introComponent.Do();

        yield return initAppComponent.Do();

        yield return loginComponent.Do();

        yield return punConnectComponent.Do();

        yield return checkLocationComponent.Do();
    }
    #endregion
}
