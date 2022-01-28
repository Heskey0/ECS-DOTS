using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct RotateDeg : IComponentData
{
    public float Value;
}
