using System;
using System.Diagnostics;
using System.Threading;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BlueBulletMoveForward : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dt = Time.DeltaTime;
        var jobHandle =
            Entities.ForEach((Entity entity, ref Translation trans, in Rotation rot, in MoveSpeed speed, in BlueBulletTag tag) =>
            {
                Debug.Log(Thread.CurrentThread.ManagedThreadId);
                //trans.Value += math.forward(rot.Value) * dt;
                trans.Value += math.mul(rot.Value,new float3(1,0,0)) * dt * speed.Value;
            }).WithBurst().Schedule(inputDeps);

        return jobHandle;
    }
}