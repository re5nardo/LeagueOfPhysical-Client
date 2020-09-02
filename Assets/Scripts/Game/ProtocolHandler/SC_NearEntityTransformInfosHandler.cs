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

        foreach (EntityTransformInfo entityTransformInfo in nearEntityTransformInfos.entityTransformInfos)
        {
            IEntity entity = Entities.Get(entityTransformInfo.m_nEntityID);
            if (entity == null)
            {
                continue;
            }

            if (entity.EntityID == Entities.MyEntityID)
            {
                TransformInterpolator_User transformInterpolator_User = Util.GetOrAddComponent<TransformInterpolator_User>((entity as MonoEntityBase).gameObject);
                transformInterpolator_User.SetData(entityTransformInfo);
            }
            else
            {
                TransformInterpolator transformInterpolator = Util.GetOrAddComponent<TransformInterpolator>((entity as MonoEntityBase).gameObject);
                transformInterpolator.SetData(entityTransformInfo);
            }
        }
    }
}
