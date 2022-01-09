using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroComponent : MonoEnumerator
{
    [SerializeField] private GameObject goCI;
    [SerializeField] private GameObject goIntro;
 
    public override IEnumerator OnExecute()
    {
        goCI.SetActive(true);
        goIntro.SetActive(false);

        yield return new WaitForSeconds(2f);

        goCI.SetActive(false);
        goIntro.SetActive(true);

        yield return new WaitForSeconds(2f);

        goIntro.SetActive(false);

        IsSuccess = true;
    }
}
