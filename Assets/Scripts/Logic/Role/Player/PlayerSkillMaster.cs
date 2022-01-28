using System;
using QFramework;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerSkillMaster : MonoBehaviour
{
    [Header("当前装备")] public Text txtEquip;
    [Header("Shoot的Cd遮罩")] public Image ShootCover;
    private SkillCaster _caster;
    private IntReactiveProperty _curEquip = new IntReactiveProperty(0);

    private void Start()
    {
        _caster = new SkillCaster(GetComponent<Player>());

        if (txtEquip != null)
        {
            _curEquip.Subscribe(value =>
            {
                DOTween.Complete(0);
                txtEquip.transform
                    .DOPunchScale(Vector3.one * 0.5f, 0.2f)
                    .SetId(0);
                txtEquip.text = (value + 1).ToString();

                _caster._equipId = value;
            });
        }


        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                _caster.runcd(Time.deltaTime);
                ChangeEquip();

                if (_curEquip.Value==1)
                {
                    return;
                }

                ShootCover.transform
                    .DOScaleY(_caster[0].GetCd(_curEquip.Value), 0.2f)
                    .SetEase(Ease.OutCirc);
            });

        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButton(0))
            .Subscribe(_ => { _caster.Cast(0, _curEquip.Value, 50); });
    }

    private void Update()
    {

    }

    void ChangeEquip()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _curEquip.Value = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _curEquip.Value = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _curEquip.Value = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _curEquip.Value = 3;
        }
    }
}