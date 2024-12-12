using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // ヒットポイント
    public int hp = 10;
    // 反応距離
    public float reactionDistance = 7.0f;

    public GameObject bulletPrefab;     //弾
    public float shootSpeed = 5.0f;     //弾の速度

    //攻撃中フラグ
    bool inArrack = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > 0)
        {
            //Playerのゲームオブジェクトを得る
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                //プレイヤーとの距離チェック
                Vector3 plpos = player.transform.position;
                float dist = Vector2.Distance(transform.position, plpos);
                if (dist <= reactionDistance && inArrack == false)
                {
                    //範囲内＆攻撃中ではない＆HP攻撃
                    inArrack = true;
                    // アニメを切り換える
                    GetComponent<Animator>().Play("BossAttack");
                }
                else if (dist > reactionDistance && inArrack)
                {
                    inArrack = false;
                    // アニメを切り換える
                    GetComponent<Animator>().Play("BossIdle");
                }
            }
            else
            {
                inArrack = false;
                // アニメを切り換える
                GetComponent<Animator>().Play("BossIdle");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            //ダメージ
            hp--;
            if (hp <= 0)
            {
                //死亡！
                //当たりを消す
                GetComponent<CircleCollider2D>().enabled = false;
                // アニメを切り換える
                GetComponent<Animator>().Play("BossDead");
                //1秒後に消す
                Destroy(gameObject, 1);
            }
        }
    }
    //攻撃
    void Attack()
    {
        //発射口オブジェクトを取得
        Transform tr = transform.Find("gate");
        GameObject gate = tr.gameObject;
        //弾を発射するベクトルを作る
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y - gate.transform.position.y;
            //アークタンジェント２関数で角度（ラジアン）を求める
            float rad = Mathf.Atan2(dy, dx);
            //ラジアンを度に変換して返す
            float angle = rad * Mathf.Rad2Deg;
            //Prefabから弾のゲームオブジェクトを作る（進行方向に回転）
            Quaternion r = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, gate.transform.position, r);
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;
            //発射
            Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
    }
}
