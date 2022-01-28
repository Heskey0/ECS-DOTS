using System;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class DestroySystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity,ref DestroyTag destroyTag) =>
        {
            if (destroyTag.Value)
            {
                PostUpdateCommands.DestroyEntity(entity);
            }
        });
    }
}
