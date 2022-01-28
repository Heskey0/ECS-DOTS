using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct DestroyTag : IComponentData
{
    public bool Value;
}
