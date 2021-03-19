using UnityEngine;
using Photon;
using System.Collections;
using UnityEngine.UI;

public class Entrance : PunBehaviour
{
    [SerializeField] private IntroComponent introComponent;
    [SerializeField] private InitAppComponent initAppComponent;
    [SerializeField] private LoginComponent loginComponent;
    [SerializeField] private PunConnectComponent punConnectComponent;
    [SerializeField] private CheckLocationComponent checkLocationComponent;
    [SerializeField] private Text textState;

    private void Awake()
    {
        textState.gameObject.SetActive(true);
        textState.text = "";
    }

    #region MonoBehaviour
    private IEnumerator Start()
    {
        //yield return introComponent.Do();

        yield return initAppComponent.Do(OnText);

        yield return loginComponent.Do(OnText);

        yield return punConnectComponent.Do(OnText);

        yield return checkLocationComponent.Do(OnText);
    }
    #endregion

    private void OnText(string text)
    {
        textState.text = text;
    }
}
