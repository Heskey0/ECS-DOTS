using System;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;


public class PinkBulletMoveForward : JobComponentSystem
{
    static Random r = new Random(1111);

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dt = Time.DeltaTime;
        var jobHandle =
            Entities.ForEach(
                (Entity entity, ref Translation trans, in Rotation rot, in MoveSpeed speed, in PinkBulletTag tag) =>
                {
                    trans.Value += math.mul(rot.Value, new float3(1, 0, 0)) * dt *
                                   r.NextFloat(1, 12);
                }).WithoutBurst().Schedule(inputDeps);

        return jobHandle;
    }
}