using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class WaveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float elapsedTime = (float)Time.ElapsedTime;

        Entities.ForEach((ref Translation trans, ref MoveSpeed moveSpeed, ref WaveData waveData) => 
        {
            float zPosition = waveData.amplitude * math.sin(elapsedTime * moveSpeed.Value + trans.Value.x * waveData.xOffset + trans.Value.y * waveData.yOffset);
            trans.Value = new float3(trans.Value.x, trans.Value.y, zPosition);
        }).Schedule();
    }

}
