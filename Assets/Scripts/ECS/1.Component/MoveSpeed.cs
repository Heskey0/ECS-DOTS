using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct MoveSpeed : IComponentData
{
    public float Value;
}
