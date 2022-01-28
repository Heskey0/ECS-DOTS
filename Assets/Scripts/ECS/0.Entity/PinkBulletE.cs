using System;
using Unity.Entities;
using UnityEngine;

public class PinkBulletE : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent(entity, typeof(PinkBulletTag));

        MoveSpeed moveSpeed = new MoveSpeed {Value = 12};
        dstManager.AddComponentData(entity, moveSpeed);
        
        DestroyTag destroyTag = new DestroyTag {Value = false};
        dstManager.AddComponentData(entity, destroyTag);
    }
}