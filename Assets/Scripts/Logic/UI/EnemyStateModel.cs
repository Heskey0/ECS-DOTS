using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnemyStateModel : Singleton<EnemyStateModel>
{
    public readonly int FullHp = 100;
    public IntReactiveProperty hp = new IntReactiveProperty(100);
    public IntReactiveProperty deltaHp = new IntReactiveProperty(100);

    public EnemyStateModel()
    {
        hp.Value = FullHp;
        deltaHp.Value = FullHp;
    }
}