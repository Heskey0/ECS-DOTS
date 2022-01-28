using System.Collections;
using System.Collections.Generic;
using MiscUtil.Threading;
using UnityEngine;

public class SkillCaster
{
    public Role owner;
    public int _equipId = 0;

    private List<SkillBase> _skills = new List<SkillBase>();

    public SkillCaster(Role owner)
    {
        this.owner = owner;
        _skills.Add(new Shoot(this));
    }

    public void runcd(float dt)
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            for (int j = 0; j < _skills[i].Cd.Length; j++)
            {
                if (_skills[i].Cd[j] > 0)
                {
                    _skills[i].Cd[j] -= dt;
                }
            }
        }
    }

    public void Cast(int skillId, int equipId, int param)
    {
        switch (skillId)
        {
            case 0:
                _skills[skillId].Cast(equipId, param);
                break;
        }
    }

    public SkillBase this[int id] => _skills[id];
}