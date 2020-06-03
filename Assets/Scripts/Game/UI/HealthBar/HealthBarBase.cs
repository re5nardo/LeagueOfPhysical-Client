using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthBarBase : MonoBehaviour
{
    protected HealthBarType m_HealthBarType = HealthBarType.None;

    public HealthBarType GetHealthBarType()
    {
        return m_HealthBarType;
    }
}
