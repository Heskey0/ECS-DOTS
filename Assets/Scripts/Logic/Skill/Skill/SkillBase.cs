using System;
using UnityEngine;

public abstract class SkillBase
{
    abstract public int SkillId { get; }
    abstract public float[] Cd { get; set; }
    public SkillCaster Caster;

    public abstract float GetCd(int equipId);
    public SkillBase(SkillCaster caster)
    {
        this.Caster = caster;
    }
    abstract public void Cast(int _equipId, int param);
}
