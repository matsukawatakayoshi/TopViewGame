using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float deleteTime = 3.0f;  // 削除する時間指定

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deleteTime);  // 削除設定
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);    // 何かに接触したら消す
    }
}
