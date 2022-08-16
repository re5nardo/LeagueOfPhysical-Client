using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public abstract class EntranceComponentBase : MonoBehaviour
{
    public async Task Execute()
    {
        await OnBeforeExecute();
        await OnExecute();
    }

    public virtual async Task OnBeforeExecute() { }
    public abstract Task OnExecute();
}
