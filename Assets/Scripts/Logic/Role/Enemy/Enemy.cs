using System;
using UniRx;
using UniRx.Triggers;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Enemy : Role
{
    private EntityManager _manager;
    private GameObject _enemyGo;
    private Entity _enemyE;

    private void Start()
    {
        _enemyGo = Resources.Load<GameObject>("Prefabs/EnemyEntity");

        _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        _enemyE = GameObjectConversionUtility.ConvertGameObjectHierarchy(_enemyGo, settings);
        

        _manager.Instantiate(_enemyE);
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                _manager.SetComponentData(_enemyE, new Translation {Value = transform.position});
            });
    }
}