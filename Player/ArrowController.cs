using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float deleteTime = 2; // 削除時間

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deleteTime); // 一定時間で消す
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ゲームオブジェクトに接触
    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.SetParent(collision.transform);           // 接触したゲームオブジェクトの子にする
        GetComponent<CircleCollider2D>().enabled = false;   // 当たりを無効化する
        GetComponent<Rigidbody2D>().simulated = false;      // 物理シュミレーションを無効化する
    }
}
