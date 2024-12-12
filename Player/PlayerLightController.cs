using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;      //Light 2Dを扱う名前空間

public class PlayerLightController : MonoBehaviour
{
    Light2D light2d;                        //Light 2D
    PlayerController playerCnt;             //PlayerControllerスクリプト
    float lightTimer = 0.0f;                //ライトの消費タイマー

    // Start is called before the first frame update
    void Start()
    {
        light2d = GetComponent<Light2D>();      //Light 2Dコンポーネントを取得

        //アイテムの数とライト距離を連動　複数個持っていると光が遠くまで届く
        light2d.pointLightOuterRadius = (float)ItemKeeper.hasLights;
        playerCnt = GameObject.FindObjectOfType<PlayerController>();    //PlayerControllerを取得しておく
    }
    // Update is called once per frame
    void Update()
    {
        //ライトをプレイヤーに合わせて回転させる
        //（例）プレイヤーが右向きの時 0度
        //→　ライトは基本上向き(90度の方向）なので、右向きの時は-90度で揃える
        transform.localEulerAngles = new Vector3(0, 0, playerCnt.angleZ - 90);

        //ライトを持っている
        if (ItemKeeper.hasLights > 0)
        {
            lightTimer += Time.deltaTime;                                //経過時間

            //10秒経過したら
            if (lightTimer > 10.0f)
            {
                lightTimer = 0.0f;                                       //経過時間リセット
                ItemKeeper.hasLights--;                                  //ライトアイテムを減らす
                light2d.pointLightOuterRadius = ItemKeeper.hasLights;    //アイテムの所持数でライト距離を変更
            }
        }
    }

    //ItemDataスクリプト側でLightを拾い次第実行される
    public void LightUpdate()
    {
        light2d.pointLightOuterRadius = ItemKeeper.hasLights;   //アイテムの所持数でライト距離を変更
    }
}