using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int hp = 3;　    // ヒットポイント
    public float speed = 0.5f; // スピード
    public float reactionDistance = 4.0f; //索敵の範囲

    float axisH;                //横軸値
    float axisV;                //縦軸値

    Rigidbody2D rbody;          //Rigidbody 2D

    Animator animator;          //Animator
    bool isActive = false;      //アクティブフラグ

    public int arrangeId = 0;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();    // Rigidbody2Dを参照する
        animator = GetComponent<Animator>();    //Animatorを参照する
    }

    // Update is called once per frame
    void Update()
    {
        //移動値初期化
        axisH = 0;
        axisV = 0;

        // Playerのゲームオブジェクトを得る
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // プレイヤーとの距離チェック
            float dist = Vector2.Distance(transform.position, player.transform.position);

            //プレイヤーとの距離が近ければ
            if (dist < reactionDistance)
            {
                isActive = true;    // アクティブにする（まだアニメは切り替えていない）
            }
            else
            {
                isActive = false;    // 非アクティブにする（まだアニメは切り替えていない）
            }

            // アニメーションを切り替える
            animator.SetBool("IsActive", isActive);

            //もしもアクティブだった場合はプレイヤーを追いかける
            //動作の元となる数字を決める
            if (isActive)
            {
                //テキストでは書いてありますが特に必要ない
                //animator.SetBool("IsActive", isActive);

                // プレイヤーへの角度を求める
                float dx = player.transform.position.x - transform.position.x; float dy = player.transform.position.y - transform.position.y; float rad = Mathf.Atan2(dy, dx);
                //ラジアン（円周率）で結果を取得するのでオイラー角度に変換
                float angle = rad * Mathf.Rad2Deg;

                // 移動角度でEnemyのアニメーションを変更する
                int direction;
                if (angle > -45.0f && angle <= 45.0f)
                {
                    direction = 3;    //右向き（まだアニメは切り替えていない）
                }
                else if (angle > 45.0f && angle <= 135.0f)
                {
                    direction = 2;    //上向き（まだアニメは切り替えていない）
                }
                else if (angle >= -135.0f && angle <= -45.0f)
                {
                    direction = 0;    //下向き（まだアニメは切り替えていない）
                }
                else
                {
                    direction = 1;    //左向き（まだアニメは切り替えていない）
                }

                //アニメの切り替え
                animator.SetInteger("Direction", direction);

                // 移動するベクトルを作る
                axisH = Mathf.Cos(rad) * speed;
                axisV = Mathf.Sin(rad) * speed;
            }
        }
        else //プレイヤーが見当たらなければ 眠っている
        {
            isActive = false;
        }
    }

    //Updateで決まった値をもとに実際の動作
    void FixedUpdate()
    {
        //isActiveがtrueで敵HPが残っている場合
        if (isActive && hp > 0)
        {
            // Updateで決めたaxisHとaxisVの値をもとに移動
            rbody.velocity = new Vector2(axisH, axisV).normalized;
        }
        //isActiveがfalse（プレイヤーが遠い）か敵をやっつけた時(HP=0)
        else
        {
            rbody.velocity = Vector2.zero;
        }
    }

    //何かとぶつかった時
    void OnCollisionEnter2D(Collision2D collision)
    {
        //相手が矢だったらダメージ
        if (collision.gameObject.tag == "Arrow")
        {

            hp--;//ダメージ

            //もし0以下になったら死亡
            if (hp <= 0)
            {
                //死亡演出

                // 当たりを消す
                GetComponent<CircleCollider2D>().enabled = false;
                //移動停止
                rbody.velocity = Vector2.zero;
                //アニメーションを切り替える
                animator.SetBool("IsDead", true);

                //animator.Play("EnemyDead");

                //0.５秒後に消す
                Destroy(gameObject, 0.5f);
                // 配置Idの記録
                SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
            }
        }
    }
}