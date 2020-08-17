using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using Entity;

public class SC_NearEntityTransformInfosHandler
{
    public static void Handle(IPhotonEventMessage msg)
    {
        SC_NearEntityTransformInfos nearEntityTransformInfos = msg as SC_NearEntityTransformInfos;

        foreach (EntityTransformInfo entityTransformInfo in nearEntityTransformInfos.m_listEntityTransformInfo)
        {
            IEntity entity = EntityManager.Instance.GetEntity(entityTransformInfo.m_nEntityID);
            if (entity == null || entity.EntityID == EntityManager.Instance.GetMyEntityID())
            {
                continue;
            }

            TransformInterpolator transformInterpolator = Util.GetOrAddComponent<TransformInterpolator>((entity as MonoEntityBase).gameObject);
            transformInterpolator.SetData(entityTransformInfo);
        }
    }
}
