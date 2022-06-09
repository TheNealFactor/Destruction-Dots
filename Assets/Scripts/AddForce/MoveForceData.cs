using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TMG.PhysicsAddForces
{
    [GenerateAuthoringComponent]
    public struct MoveForceData : IComponentData
    {
        public float ForceAmount;
        public KeyCode ForwardInputKey;
        public Vector3 ForwardDirection;
    }
}