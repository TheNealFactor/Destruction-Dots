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


            var allTranslations = GetComponentDataFromEntity<Translation>(true);

            var deltaTime = Time.DeltaTime;
            Entities.ForEach((Entity projectile, ref PhysicsVelocity physicsVelocity, ref PhysicsMass physicsMass, ref Translation translation, ref Rotation rotation, in LocalToWorld localToWorld,
                in MoveForceData moveForceData) =>
            {

                //if (Input.GetKey(moveForceData.ForwardInputKey))
                //{

                  var direction = math.forward(rotation.Value);
                    //quaternion
                    var forceVector = direction * moveForceData.ForceAmount * deltaTime;
                    // rotation.Value = quaternion.LookRotation(translation.Value - tar);
                    // translation.Value = 
                    physicsVelocity.ApplyLinearImpulse(physicsMass, forceVector);

                   
             
               // }

            }).Run();
        }
            }
        }
    
