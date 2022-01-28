using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public class PinkBulletSystem : JobComponentSystem
{
    private EntityQuery PinkBulletGroup;
    private EntityQuery EnemyGroup;

    protected override void OnCreate()
    {
        base.OnCreate();
        var pinkBulletQuery = new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                ComponentType.ReadOnly<PinkBulletTag>(), typeof(Translation), typeof(Rotation), typeof(RotateDeg),
                typeof(DestroyTag)
            }
        };
        var enemyQuery = new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                ComponentType.ReadOnly<EnemyTag>(), typeof(Translation), typeof(Prefab)
            }
        };

        PinkBulletGroup = GetEntityQuery(pinkBulletQuery);
        EnemyGroup = GetEntityQuery(enemyQuery);
    }

    struct PinkBulletJob : IJobChunk
    {
        public ComponentTypeHandle<Translation> bulletTransType;
        public ComponentTypeHandle<Rotation> bulletRotType;
        public ComponentTypeHandle<RotateDeg> rotateDegType;

        public ComponentTypeHandle<DestroyTag> destroyType;
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<Translation> enemyArr;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var bulletTransArr = chunk.GetNativeArray(bulletTransType);
            var bulletRotArr = chunk.GetNativeArray(bulletRotType);
            var rotateDegArr = chunk.GetNativeArray(rotateDegType);

            var destroyArr = chunk.GetNativeArray(destroyType);
            for (int i = 0; i < chunk.Count; i++)
            {
                //追踪
                var dir = enemyArr[0].Value - bulletTransArr[i].Value;
                float angle = math.degrees(math.atan2(dir.y, dir.x)) + rotateDegArr[i].Value;
                
                var tmpRot = bulletRotArr[i];
                tmpRot.Value = Quaternion.Euler(new float3(0, 0, angle));
                //Debug.Log($"euler:(0,0,{angle})");
                //Debug.Log($"quaternion:{tmpRot.Value}");
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
        var jobHandle = new PinkBulletJob
        {
            bulletTransType = GetComponentTypeHandle<Translation>(),
            bulletRotType = GetComponentTypeHandle<Rotation>(),
            rotateDegType = GetComponentTypeHandle<RotateDeg>(),

            destroyType = GetComponentTypeHandle<DestroyTag>(),
            enemyArr = EnemyGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
        };
        return jobHandle.Schedule(PinkBulletGroup, inputDeps);
    }
}
