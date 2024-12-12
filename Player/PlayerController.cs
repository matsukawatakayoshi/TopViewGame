//ギット版
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f; //移動スピード
    int direction = 0; //移動方向のアニメ番号
    float axisH; //横軸の値
    float axisV; //縦軸の値
    public float angleZ = -90.0f; //回転角度

    Rigidbody2D rbody; //Rigidbody2Dを参照予定
    Animator animator; //Animatorを参照予定

    //アレンジ
    SpriteRenderer sprite; //SpriteRendererを参照予定


    bool isMoving = false; //移動中か判断するフラグ

    public static int hp = 3; //プレイヤーのHP
    public static string gameState; //ゲームのステータス管理
    bool inDamage = false; //ダメージ中かどうかのフラグ

    //p1からp2の角度を返すメソッド
    float GetAngle(Vector2 p1, Vector2 p2)
    {
        float angle;
        if (axisH != 0 || axisV != 0)
        {
            //移動中であれば角度を更新する
            //p1からp2への差分（原点を0にする)
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            //アークタンジェント2メソッドの能力で角度（ラジアン値：円周率）を求める
            float rad = Mathf.Atan2(dy, dx);
            //ラジアン値をオイラー値（〇度）という表現に変換する
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            //停止中であれば以前の角度を維持していく
            angle = angleZ;
        }
        return angle;
    }

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); //PlayerのRigidbody2Dを参照
        animator = GetComponent<Animator>(); //PlayerのAnimatorを参照

        //アレンジ
        sprite = GetComponent<SpriteRenderer>(); //PlayerのSpriteRendererを参照

        //ゲームの状態をまずは"playing"にする
        gameState = "playing";
        // HPの更新
        hp = PlayerPrefs.GetInt("PlayerHP");
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームステータスが"playing"以外である
        //もしくはダメージを受けている最中
        //の場合はUpdateは何もしない
        if (gameState != "playing" || inDamage)
        {
            return;
        }


        if (isMoving == false)
        {
            axisH = Input.GetAxisRaw("Horizontal"); //左右のキーを検知
            axisV = Input.GetAxisRaw("Vertical"); //上下のキーを検知
        }

        //キー値から移動角度を求める
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt);

        //移動角度から向いている方向を決めてアニメーションを更新

        int dir;
        if (angleZ >= -45 && angleZ < 45)
        {
            //向くべき方向は右向き
            dir = 3;
        }
        else if (angleZ >= 45 && angleZ <= 135)
        {
            //向くべき方向は上向き
            dir = 2;
        }
        else if (angleZ >= -135 && angleZ <= -45)
        {
            //向くべき方向は下向き
            dir = 0;
        }
        else
        {
            //それ以外は消去法で左向き
            dir = 1;
        }
        //向くべき方向が前と違っていたらアニメ更新
        if (dir != direction)
        {
            direction = dir; //向くべきアニメ方向の番号を更新
            animator.SetInteger("Direction", direction);
        }
    }

    void FixedUpdate()
    {
        //ゲームステータスが"playing"以外である
        //の場合はFixedUpdateは何もしない
        if (gameState != "playing")
        {
            return;
        }

        if (inDamage)
        {
            //ダメージを受けている最中なら
            //点滅させる（表示・非表示の連続）

            //変数valに連続的な値を授ける
            float val = Mathf.Sin(Time.time * 50);
            //もしその時の変数valがプラスなら
            if (val > 0)
            {
                //プレイヤーは表示されている
                //gameObject.GetComponent<SpriteRenderer>().enabled = true;
                //アレンジ
                sprite.enabled = true;
            }
            else
            {
                //プレイヤーは表示されない
                //gameObject.GetComponent<SpriteRenderer>().enabled = false;
                //アレンジ
                sprite.enabled = false;
            }

            //そのフレームにおける点滅のための表示・非表示が決まったら、以降の処理は何もさせず、再度FixedUpdateの先頭に戻る
            return;
        }

        //inDamageがfalseの時のみ
        //移動速度の更新
        rbody.velocity = new Vector2(axisH, axisV).normalized * speed;
    }

    //バーチャルパッド対策
    public void SetAxis(float h, float v)
    {
        axisH = h;
        axisV = v;
        if (axisH == 0 && axisV == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    //コライダー同士が接触したら発動
    void OnCollisionEnter2D(Collision2D collision)
    {
        //ぶつかった相手がEnemyだったら
        if (collision.gameObject.tag == "Enemy")
        {
            //すでにダメージ中であれば発動しない※無敵時間
            if (!inDamage)
            {
                //ダメージをうける自作メソッドの発動
                GetDamage(collision.gameObject);
            }
        }
    }

    //ダメージをうけるメソッド
    void GetDamage(GameObject enemy)
    {
        //もしも"playing"中なら
        if (gameState == "playing")
        {
            hp--; //HPを減らす
            // HPの更新
            PlayerPrefs.SetInt("PlayerHP", hp);

            //HPが残っていれば
            if (hp > 0)
            {
                //移動を一旦停止
                rbody.velocity = new Vector2(0, 0);
                //ぶつかった相手のいる方向と反対方向を割り出す
                Vector3 v = (transform.position - enemy.transform.position).normalized;
                //割り出した反対方向にヒットバックさせる
                rbody.AddForce(new Vector2(v.x * 4, v.y * 4), ForceMode2D.Impulse);

                //ダメージ受け中のフラグをON
                inDamage = true;

                //時間差でダメージ受け中のフラグを解除
                Invoke("DamageEnd", 0.25f);
            }
            else //HPが残っていなかったら
            {
                //ゲームオーバーメソッドの発動
                GameOver();
            }
        }
    }

    //ダメージ受け中フラグを解除する処理
    void DamageEnd()
    {
        inDamage = false; //ダメージ受け中フラグをOFF
        //場合によっては点滅の処理の途中（非表示）のタイミングでダメージ処理が終わってしまうかもしれないので、保険として明確にプレイヤーを表示状態にしておく
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    //ゲームオーバーメソッド
    void GameOver()
    {
        gameState = "gameover";

        //ゲームオーバー演出
        GetComponent<CircleCollider2D>().enabled = false; //当たり判定を消す
        rbody.velocity = new Vector2(0, 0); //動きを止める
        rbody.gravityScale = 1; //重力復活
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); //プレイヤーを上に跳ね上げる

        animator.SetBool("isDead", true); //PlayerDeadアニメの発動
        Destroy(gameObject, 1.0f); //演出をしたら1秒後に自分を抹消

        // BGM停止
        SoundManager.soundManager.StopBgm();
        // SE再生（ゲームオーバー）
        SoundManager.soundManager.SEPlay(SEType.GameOver);
    }
}