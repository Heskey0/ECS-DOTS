using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PinkBullet2System : JobComponentSystem
{
    private EntityQuery PinkBullet2Group;
    private EntityQuery EnemyGroup;

    protected override void OnCreate()
    {
        base.OnCreate();
        var pinkBullet2Query = new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                ComponentType.ReadOnly<PinkBullet2Tag>(), typeof(Translation), typeof(Rotation), typeof(RotateDeg),
                typeof(DestroyTag), typeof(DeadTime)
            }
        };
        var enemyQuery = new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                ComponentType.ReadOnly<EnemyTag>(), typeof(Translation), typeof(Prefab)
            }
        };

        PinkBullet2Group = GetEntityQuery(pinkBullet2Query);
        EnemyGroup = GetEntityQuery(enemyQuery);
    }

    struct PinkBullet2Job : IJobChunk
    {
        public float dt;

        public ComponentTypeHandle<Translation> bulletTransType;
        public ComponentTypeHandle<Rotation> bulletRotType;
        public ComponentTypeHandle<RotateDeg> rotateDegType;
        public ComponentTypeHandle<DeadTime> deadTimeType;

        public ComponentTypeHandle<DestroyTag> destroyType;
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<Translation> enemyArr;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var bulletTransArr = chunk.GetNativeArray(bulletTransType);
            var bulletRotArr = chunk.GetNativeArray(bulletRotType);
            var rotateDegArr = chunk.GetNativeArray(rotateDegType);
            var deadTimeArr = chunk.GetNativeArray(deadTimeType);

            var destroyArr = chunk.GetNativeArray(destroyType);
            for (int i = 0; i < chunk.Count; i++)
            {
                if (deadTimeArr[i].Value.FloatEqual(0.2f))
                {
                    var v0 = bulletTransArr[i].Value;
                    var r0 = rotateDegArr[i].Value;
                    var dir0 = enemyArr[0].Value - v0;
                    float angle0;
                    if (r0>0)
                    {
                        angle0 = math.degrees(math.atan2(dir0.y, dir0.x)) + r0 * 3 + 90;
                    }
                    else
                    {
                        angle0 = math.degrees(math.atan2(dir0.y, dir0.x)) + r0 * 3 - 90;
                    }

                    var tmpRot0 = bulletRotArr[i];
                    tmpRot0.Value = Quaternion.Euler(new float3(0, 0, angle0));
                    bulletRotArr[i] = tmpRot0;
                }

                deadTimeArr[i] = new DeadTime {Value = deadTimeArr[i].Value - dt};
                if (deadTimeArr[i].Value > 0)
                {
                    //deadTimeArr[i] = new DeadTime {Value = _deadTime};
                    continue;
                }

                //追踪
                var dir = enemyArr[0].Value - bulletTransArr[i].Value;
                float angle = math.degrees(math.atan2(dir.y, dir.x)) + rotateDegArr[i].Value;

                var tmpRot = bulletRotArr[i];
                tmpRot.Value = Quaternion.Euler(new float3(0, 0, angle));
                bulletRotArr[i] = tmpRot;

                //销毁
                var dis = math.length(dir);
                if (dis < 0.3f)
                {
                    var tmpDestroy = destroyArr[i];
                    tmpDestroy.Value = true;
                    destroyArr[i] = tmpDestroy;
                }
            }
        }
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = new PinkBullet2Job
        {
            dt = Time.DeltaTime,

            bulletTransType = GetComponentTypeHandle<Translation>(),
            bulletRotType = GetComponentTypeHandle<Rotation>(),
            rotateDegType = GetComponentTypeHandle<RotateDeg>(),
            deadTimeType = GetComponentTypeHandle<DeadTime>(),

            destroyType = GetComponentTypeHandle<DestroyTag>(),
            enemyArr = EnemyGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
        };
        return jobHandle.Schedule(PinkBullet2Group, inputDeps);
    }
}