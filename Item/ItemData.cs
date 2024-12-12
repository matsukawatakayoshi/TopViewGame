using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイテムの種類
public enum ItemType
{
    Arrow,      // 矢
    GoldKey,    // 金のカギ
    SilverKey,  // 銀の鍵
    Life,       // ライフ
    Light,      // ライト
}
public class ItemData : MonoBehaviour
{
    public ItemType type;
    public int count = 1;
    public int arrangeId = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 接触
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (type == ItemType.GoldKey)
            {
                // 金のカギ
                ItemKeeper.hasGoldKeys += count;
            }
            else if (type == ItemType.SilverKey)
            {
                // 銀のカギ
                ItemKeeper.hasSilverKeys += count;
            }
            else if (type == ItemType.Arrow)
            {
                // 矢
                ArrowShoot shoot = collision.gameObject.GetComponent<ArrowShoot>();
                ItemKeeper.hasArrows += count;
            }
            else if (type == ItemType.Life)
            {
                // ライフ
                if (PlayerController.hp < 3)
                {
                    // HPが３以下の場合加算する
                    PlayerController.hp++;
                    // HPの更新
                    PlayerPrefs.SetInt("PlayerHP", PlayerController.hp);
                }
            }
            else if (type == ItemType.Light)
            {
                // ライト
                ItemKeeper.hasLights += count;
                GameObject.FindObjectOfType<PlayerLightController>().LightUpdate();
            }
            // アイテム取得演出
            // 当たりを消す
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            // アイテムのRigidbody2dを取ってくる
            Rigidbody2D itemBody = GetComponent<Rigidbody2D>();
            // 重力を戻す
            itemBody.gravityScale = 2.5f;
            // 上に少し跳ね上げる演出
            itemBody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
            // 0.5秒後に削除
            Destroy(gameObject, 0.5f);
            // 配置Idの記録
            SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
        }
    }
}
