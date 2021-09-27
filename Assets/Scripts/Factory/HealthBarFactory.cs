using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class HealthBarFactory : MonoSingleton<HealthBarFactory>
{
    public HealthBarBase CreateHealthBar(HealthBarType type)
    {
        if (type == HealthBarType.Player)
        {
            GameObject goHealthBar = ResourcePool.Instance.GetResource(Define.ResourcePath.HealthBar.PLAYER, LOP.Game.Current.GameUI.HealthBarCanvas.transform);

            return goHealthBar.GetComponent<HealthBarBase>();
        }

        Debug.LogError(string.Format("type is invalid! type : {0}", type.ToString()));
        return null;
    }
}
