using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class SnipeBullet : MonoBehaviour
{
    public float speed;
    [Range(0, 90)] public float degRange;

    private static Random r = new Random(22222);
    private Transform _target;
    private float _deg;

    private Vector3 _dir;
    private float _angle;

    private void Start()
    {
        _deg = r.NextFloat(-degRange, degRange);
        _target = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    void Update()
    {
        _dir = _target.transform.position - transform.position;
        if (math.length(_dir) <= 0.5f)
        {
            Destroy(gameObject);
        }

        _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg + _deg;

        transform.rotation = Quaternion.Euler(0, 0, _angle);
        transform.position += transform.right * speed * Time.deltaTime;
    }
}