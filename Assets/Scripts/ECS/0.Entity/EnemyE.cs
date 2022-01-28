using System;
using System.Diagnostics;
using Unity.Entities;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EnemyE : MonoBehaviour,IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Debug.Log("convert");
        dstManager.AddComponent(entity, typeof(EnemyTag));
    }
}
