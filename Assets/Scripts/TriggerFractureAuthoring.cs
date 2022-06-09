using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

public struct TriggerFractureFactor : IComponentData
{
    
}

[BurstCompile]

struct TriggerFractureFactorJob : ITriggerEventsJob
{
    [ReadOnly] public ComponentDataFromEntity<TriggerFractureFactor> TriggerFractureGroup;

    public void Execute(TriggerEvent triggerEvent)
    {
        Entity entityA = triggerEvent.EntityA;
        Entity entityB = triggerEvent.EntityB;

        bool isBodyATrigger = TriggerFractureGroup.HasComponent(entityA);
        bool isBodyBTrigger = TriggerFractureGroup.HasComponent(entityB);

        // Ignoring Triggers overlapping other Triggers
        if (isBodyATrigger && isBodyBTrigger)
            return;

        bool isBodyADynamic = TriggerFractureGroup.HasComponent(entityA);
        bool isBodyBDynamic = TriggerFractureGroup.HasComponent(entityB);

        // Ignoring overlapping static bodies
        if ((isBodyATrigger && !isBodyBDynamic) ||
            (isBodyBTrigger && !isBodyADynamic))
            return;

        var triggerEntity = isBodyATrigger ? entityA : entityB;
        var dynamicEntity = isBodyATrigger ? entityB : entityA;

        var triggerFractureComponent = TriggerFractureGroup[triggerEntity];

    }
}


public class TriggerFractureAuthoring : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
