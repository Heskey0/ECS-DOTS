using System;
using Unity.Entities;
using UnityEngine;

public class PinkBullet2E : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent(entity, typeof(PinkBullet2Tag));

        MoveSpeed moveSpeed = new MoveSpeed {Value = 12};
        dstManager.AddComponentData(entity, moveSpeed);

        DestroyTag destroyTag = new DestroyTag {Value = false};
        dstManager.AddComponentData(entity, destroyTag);

    }
}