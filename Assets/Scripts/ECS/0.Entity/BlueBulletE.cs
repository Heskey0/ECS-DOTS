using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class BlueBulletE : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent(entity, typeof(BlueBulletTag));

        MoveSpeed moveSpeed = new MoveSpeed {Value = 12};
        dstManager.AddComponentData(entity, moveSpeed);

        DestroyTag destroyTag = new DestroyTag {Value = false};
        dstManager.AddComponentData(entity, destroyTag);
    }
}