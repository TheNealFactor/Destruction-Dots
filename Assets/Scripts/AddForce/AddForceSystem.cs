using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;

namespace TMG.PhysicsAddForces
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial class AddForceSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((Entity projectile, ref PhysicsVelocity physicsVelocity, ref PhysicsMass physicsMass, ref Translation translation, ref Rotation rotation,
                in MoveForceData moveForceData) =>
            {

                    var forceVector = (float3)Vector3.forward * moveForceData.ForceAmount * deltaTime;
            physicsVelocity.ApplyLinearImpulse(physicsMass, forceVector);
            //var direction = rotation;

               // physicsVelocity.ApplyImpulse(physicsMass, translation, rotation, forceVector, 0.0f);

            }).Run();
        }
    }
}