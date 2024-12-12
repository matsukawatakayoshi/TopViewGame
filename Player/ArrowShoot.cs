using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    public float shootSpeed = 12.0f;    // 矢の速度
    public float shootDelay = 0.25f;    // 発射間隔
    public GameObject bowPrefab;        // 弓のプレハブ
    public GameObject arrowPrefab;      // 矢のプレハブ
    bool inAttack = false;              // 攻撃中フラグ
    GameObject bowObj;                  // 弓のゲームオブジェクト

    // 攻撃
    public void Attack()
    {
        // 
        if (ItemKeeper.hasArrows > 0 && inAttack == false)
        {
            ItemKeeper.hasArrows--; // 矢を減らす
            inAttack = true;    // 攻撃フラグを立てる
            // 矢を撃つ
            PlayerController playerCnt = GetComponent<PlayerController>();
            float angleZ = playerCnt.angleZ; // 回転角度
            // 矢のゲームオブジェクトを作る（進行方向に回転）
            Quaternion r = Quaternion.Euler(0, 0, angleZ);
            GameObject arrowObj = Instantiate(arrowPrefab, transform.position, r);
            // 矢を発射するベクトルを作る
            float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
            float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;
            // 矢に力を加える
            Rigidbody2D body = arrowObj.GetComponent<Rigidbody2D>(); body.AddForce(v, ForceMode2D.Impulse);
            // 攻撃フラグを下ろす遅延実行
            Invoke("StopAttack", shootDelay);

            // SE再生(矢を射る）
            SoundManager.soundManager.SEPlay(SEType.Shoot);
        }
    }

    // 攻撃停止
    public void StopAttack()
    {
        inAttack = false;   // 攻撃フラグを下ろす
    }

    // Start is called before the first frame update
    void Start()
    {
        // 弓をプレイヤーキャラクターに配置
        Vector3 pos = transform.position;
        bowObj = Instantiate(bowPrefab, pos, Quaternion.identity);
        bowObj.transform.SetParent(transform); //  弓の親のプレイヤーキャラクターを設定する
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 攻撃キーが押された
            Attack();
        }
        // 弓の回転と優先順位
        float bowZ = -1; // 弓のZ値（キャラクターの前にする）
        PlayerController plmv = GetComponent<PlayerController>();
        if (plmv.angleZ > 30 && plmv.angleZ < 150)
        {
            // 上向き
            bowZ = 1; // 弓のZ値（キャラクとあーの後ろにする）
            
        }
        // 弓の回転
        bowObj.transform.rotation = Quaternion.Euler(0, 9, plmv.angleZ);
        // 弓の優先順位
        bowObj.transform.position = new Vector3(transform.position.x, transform.position.y, bowZ);
    }
}
