using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool isInAir = false;

    public float speed;
    public float jumpForce;
    [Header("跳跃次数")] public int jump = 2;
    [HideInInspector] public int _jump = 2; //剩下的跳跃次数
    private Rigidbody2D _rb;

    
    /*************************************************************************/
    /******************************  Start  ******************************/
    /*************************************************************************/
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _jump = jump;

        transform.Find("foot")//.GetComponent<BoxCollider2D>()
            .OnTriggerEnter2DAsObservable()
            .Where(c => c.gameObject.CompareTag("Floor"))
            .Subscribe(c =>
            {
                isInAir = false;
                _jump = jump;
            });
        transform.Find("foot")//.GetComponent<BoxCollider2D>()
            .OnTriggerExit2DAsObservable()
            .Where(c => c.gameObject.CompareTag("Floor"))
            .Subscribe(c =>
            {
                isInAir = true;
                _jump = jump - 1;
                StartCoroutine(jumpEnumerator());
            });
        

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Where(_ => !isInAir)
            .Subscribe(_ => { Jump(); });

        this.FixedUpdateAsObservable()
            .Subscribe(_ => { Movement(); });
    }

    /*************************************************************************/
    /******************************  Function  ******************************/
    /*************************************************************************/

    void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(h * speed, _rb.velocity.y);
    }

    void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(new Vector2(0, jumpForce));
    }


    /*************************************************************************/
    /******************************  协程  ******************************/
    /*************************************************************************/
    IEnumerator jumpEnumerator()
    {
        yield return null;
        while (!Input.GetKeyDown(KeyCode.Space)) //无输入
        {
            if (!isInAir) //落到地面，则退出协程
            {
                Debug.Log("落地");
                _jump = 0;
                break;
            }

            yield return null;
        }

        //在空中按下空格
        _jump--;

        if (_jump >= 0)
        {
            Jump();
            StartCoroutine(jumpEnumerator());
        }
    }
}