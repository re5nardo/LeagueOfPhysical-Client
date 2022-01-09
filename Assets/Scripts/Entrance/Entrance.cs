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
    [SerializeField] private CheckLocationComponent checkLocationComponent;

    [Header("[UI Components]")]
    public TextMeshProUGUI stateText;
    public Slider loadingBar;
    public TextMeshProUGUI loadingBarText;

    private MonoEnumerator[] entranceComponents;

    protected override void Awake()
    {
        base.Awake();

        stateText.gameObject.SetActive(true);
        stateText.text = "";
        loadingBar.gameObject.SetActive(false);
        loadingBarText.text = "";

        entranceComponents = new MonoEnumerator[]
        {
            //introComponent,
            initAppComponent,
            //loginComponent,
            checkLocationComponent,
        };
    }

    private IEnumerator Start()
    {
        foreach (var entranceComponent in entranceComponents)
        {
            yield return entranceComponent.Execute();

            if (!entranceComponent.IsSuccess)
            {
                Debug.LogError($"entranceComponent is failure. entranceComponent: {entranceComponent}");
                yield break;
            }
        }
    }
}
