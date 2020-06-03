using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceReturnAgent : MonoBehaviour
{
    public float m_fDelayTime = 0f;

    private float m_fElapsedTime = 0f;

    // Update is called once per frame
    void Update ()
    {
        if (m_fElapsedTime >= m_fDelayTime)
        {
            ResourcePool.Instance.ReturnResource(gameObject);

            Destroy(this);
        }

        m_fElapsedTime += Time.deltaTime;
    }
}
