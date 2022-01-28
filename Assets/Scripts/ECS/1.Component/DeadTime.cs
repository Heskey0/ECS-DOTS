using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct DeadTime : IComponentData
{
    public float Value;
}
