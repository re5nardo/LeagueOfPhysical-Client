using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Entity;
using NetworkModel.Mirror;
using GameFramework;

public class EntityAnimatorController : MonoBehaviour
{
    private LOPMonoEntityBase entity;
    private RoomProtocolDispatcher roomProtocolDispatcher;
    private EntityAnimatorSnap entityAnimatorSnap = new EntityAnimatorSnap();

    // Note: not an object[] array because otherwise initialization is real annoying
    private int[] lastIntParameters;
    private float[] lastFloatParameters;
    private bool[] lastBoolParameters;
    private AnimatorControllerParameter[] parameters;

    // multiple layers
    private int[] animationHash;
    private int[] transitionHash;
    private float[] layerWeight;

    private void Awake()
    {
        entity = GetComponent<LOPMonoEntityBase>();

        roomProtocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();
        roomProtocolDispatcher[typeof(SC_Synchronization)] = OnSC_Synchronization;

        // store the animator parameters in a variable - the "Animator.parameters" getter allocates
        // a new parameter array every time it is accessed so we should avoid doing it in a loop
        parameters = entity.ModelAnimator.parameters.Where(par => !entity.ModelAnimator.IsParameterControlledByCurve(par.nameHash)).ToArray();
        lastIntParameters = new int[parameters.Length];
        lastFloatParameters = new float[parameters.Length];
        lastBoolParameters = new bool[parameters.Length];

        animationHash = new int[entity.ModelAnimator.layerCount];
        transitionHash = new int[entity.ModelAnimator.layerCount];
        layerWeight = new float[entity.ModelAnimator.layerCount];

        TickPubSubService.AddSubscriber("LateTickEnd", OnLateTickEnd);
    }

    private void OnDestroy()
    {
        TickPubSubService.RemoveSubscriber("LateTickEnd", OnLateTickEnd);
    }

    private void OnSC_Synchronization(IMessage msg)
    {
        if (entity.HasAuthority)
        {
            return;
        }

        SC_Synchronization synchronization = msg as SC_Synchronization;

        synchronization.listSnap?.ForEach(snap =>
        {
            if (snap is EntityAnimatorSnap entityAnimatorSnap && entityAnimatorSnap.entityId == entity.EntityID)
            {
                SyncAnimator(entityAnimatorSnap);
            }
        });
    }

    private void OnLateTickEnd(int tick)
    {
        if (entity.HasAuthority)
        {
            var synchronization = ObjectPool.Instance.GetObject<CS_Synchronization>();
            SetEntityAnimatorSnap(entityAnimatorSnap);
            synchronization.listSnap.Add(entityAnimatorSnap);

            RoomNetwork.Instance.Send(synchronization, 0, instant: true);
        }
    }

    private void SyncAnimator(EntityAnimatorSnap entityAnimatorSnap)
    {
        entity.ModelAnimator.speed = entityAnimatorSnap.animatorSpeed;

        for (int i = 0; i < parameters.Length; i++)
        {
            AnimatorControllerParameter par = parameters[i];
            if (par.type == AnimatorControllerParameterType.Int)
            {
                int newIntValue = (int)entityAnimatorSnap.animationParametersData.values[i];
                entity.ModelAnimator.SetInteger(par.nameHash, newIntValue);
            }
            else if (par.type == AnimatorControllerParameterType.Float)
            {
                float newFloatValue = (float)entityAnimatorSnap.animationParametersData.values[i];
                entity.ModelAnimator.SetFloat(par.nameHash, newFloatValue);
            }
            else if (par.type == AnimatorControllerParameterType.Bool)
            {
                bool newBoolValue = (bool)entityAnimatorSnap.animationParametersData.values[i];
                entity.ModelAnimator.SetBool(par.nameHash, newBoolValue);
            }
        }

        entityAnimatorSnap.animStateDataList?.ForEach(animStateData =>
        {
            if (animStateData.stateHash != 0 && entity.ModelAnimator.enabled)
            {
                entity.ModelAnimator.Play(animStateData.stateHash, animStateData.layerId, animStateData.normalizedTime);
            }

            entity.ModelAnimator.SetLayerWeight(animStateData.layerId, animStateData.weight);
        });
    }

    private bool CheckAnimStateChanged(out int stateHash, out float normalizedTime, int layerId)
    {
        bool change = false;
        stateHash = 0;
        normalizedTime = 0;

        float lw = entity.ModelAnimator.GetLayerWeight(layerId);
        if (Mathf.Abs(lw - layerWeight[layerId]) > 0.001f)
        {
            layerWeight[layerId] = lw;
            change = true;
        }

        if (entity.ModelAnimator.IsInTransition(layerId))
        {
            AnimatorTransitionInfo tt = entity.ModelAnimator.GetAnimatorTransitionInfo(layerId);
            if (tt.fullPathHash != transitionHash[layerId])
            {
                // first time in this transition
                transitionHash[layerId] = tt.fullPathHash;
                animationHash[layerId] = 0;
                return true;
            }
            return change;
        }

        AnimatorStateInfo st = entity.ModelAnimator.GetCurrentAnimatorStateInfo(layerId);
        if (st.fullPathHash != animationHash[layerId])
        {
            // first time in this animation state
            if (animationHash[layerId] != 0)
            {
                // came from another animation directly - from Play()
                stateHash = st.fullPathHash;
                normalizedTime = st.normalizedTime;
            }
            transitionHash[layerId] = 0;
            animationHash[layerId] = st.fullPathHash;
            return true;
        }

        return change;
    }

    private void SetEntityAnimatorSnap(EntityAnimatorSnap entityAnimatorSnap)
    {
        entityAnimatorSnap.Tick = Game.Current.CurrentTick;
        entityAnimatorSnap.entityId = entity.EntityID;
        entityAnimatorSnap.animatorSpeed = entity.ModelAnimator.speed;

        entityAnimatorSnap.animationParametersData.values.Clear();
        for (int i = 0; i < entity.ModelAnimator.parameters.Length; i++)
        {
            AnimatorControllerParameter par = entity.ModelAnimator.parameters[i];
            if (par.type == AnimatorControllerParameterType.Int)
            {
                int value = entity.ModelAnimator.GetInteger(par.nameHash);
                entityAnimatorSnap.animationParametersData.values.Add(value);
            }
            else if (par.type == AnimatorControllerParameterType.Float)
            {
                float value = entity.ModelAnimator.GetFloat(par.nameHash);
                entityAnimatorSnap.animationParametersData.values.Add(value);
            }
            else if (par.type == AnimatorControllerParameterType.Bool)
            {
                bool value = entity.ModelAnimator.GetBool(par.nameHash);
                entityAnimatorSnap.animationParametersData.values.Add(value);
            }
        }

        entityAnimatorSnap.animStateDataList.Clear();
        for (int i = 0; i < entity.ModelAnimator.layerCount; i++)
        {
            if (!CheckAnimStateChanged(out var stateHash, out var normalizedTime, i))
            {
                continue;
            }

            AnimStateData animStateData = new AnimStateData();
            animStateData.stateHash = stateHash;
            animStateData.normalizedTime = normalizedTime;
            animStateData.layerId = i;
            animStateData.weight = layerWeight[i];

            entityAnimatorSnap.animStateDataList.Add(animStateData);
        }
    }
}
