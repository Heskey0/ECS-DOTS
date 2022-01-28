using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerStateModel : Singleton<PlayerStateModel>
{
    public readonly int FullHp = 100;
    public IntReactiveProperty hp=new IntReactiveProperty(100);
    public IntReactiveProperty deltaHp=new IntReactiveProperty(100);

    public PlayerStateModel()
    {
        hp.Value = FullHp;
        deltaHp.Value = FullHp;
    }
}
