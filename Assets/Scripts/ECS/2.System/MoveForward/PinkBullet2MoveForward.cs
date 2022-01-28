using System;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class PinkBullet2MoveForward : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dt = Time.DeltaTime;
        var jobHandle =
            Entities.ForEach((Entity entity, ref Translation trans, in Rotation rot, in MoveSpeed speed, in PinkBullet2Tag tag) =>
            {
                trans.Value += math.mul(rot.Value,new float3(1,0,0)) * dt * speed.Value;
            }).WithBurst().Schedule(inputDeps);

        return jobHandle;
    }
}
