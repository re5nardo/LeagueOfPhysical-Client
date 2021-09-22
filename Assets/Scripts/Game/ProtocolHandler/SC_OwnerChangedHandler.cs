using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using NetworkModel.Mirror;
using Entity;

public class SC_OwnerChangedHandler
{
    public static void Handle(SC_OwnerChanged ownerChanged)
    {
        var entity = Entities.Get<LOPMonoEntityBase>(ownerChanged.entityId);
        if (entity == null)
        {
            return;
        }

        entity.OwnerId = ownerChanged.ownerId;
    }
}
