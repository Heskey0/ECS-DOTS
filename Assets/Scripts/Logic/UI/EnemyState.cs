using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

public class EnemyState : MonoBehaviour
{
    private EnemyStateModel _model;
    private Transform imgHp;
    private Transform imgDeltaHp;
    private Text txtHp;

    private void Start()
    {
        _model = EnemyStateModel.Instance;
        imgHp = transform.Find("hp");
        imgDeltaHp = transform.Find("hp_delta");
        txtHp = transform.Find("txtHp").GetComponent<Text>();

        _model.hp
            .Subscribe(value =>
            {
                txtHp.text = Mathf.Clamp(value, 0, 100).ToString();
                //文字动画
                DOTween.Complete(1);
                txtHp.transform
                    .DOPunchScale(0.5f * Vector3.one, 0.3f)
                    .SetEase(Ease.InOutElastic)
                    .SetId(1);
                //血条动画
                float delta = Mathf.Clamp(
                    (float) value / (float) _model.FullHp
                    , 0
                    , (float) _model.FullHp);
                imgHp
                    .DOScaleX(delta, 0.8f)
                    .SetEase(Ease.OutCirc);
                //设置deltaHp

                _model.deltaHp.Value = value;
            });

        _model.deltaHp
            .Throttle(TimeSpan.FromMilliseconds(800))
            .Subscribe(value =>
            {
                float delta = Mathf.Clamp(
                    (float) value / (float) _model.FullHp
                    , 0
                    , (float) _model.FullHp);
                imgDeltaHp
                    .DOScaleX(delta, 1f)
                    .SetEase(Ease.OutCirc);
            });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _model.hp.Value -= 5;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            _model.hp.Value = 100;
        }
    }
}