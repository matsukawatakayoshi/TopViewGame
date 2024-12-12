using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public Sprite openImage;        // 開いた画像
    public GameObject itemPrefab;   // 出てくるアイテムのプレハブ
    public bool isClosed = true;    // ture ＝　閉まっている　false ＝　開いている
    public int arrangeId = 0;       // 配置の識別に使う

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isClosed && collision.gameObject.tag =="Player")
        {
            // 箱が閉まっている状態でプレイヤーに接触
            GetComponent<SpriteRenderer>().sprite = openImage;
            isClosed = false; // 開いてる状態にする
            if(itemPrefab != null)
            {
                // アイテムをプレハブから作る
                Instantiate(itemPrefab, transform.position, Quaternion.identity);
            }
            // 配置Idの記録
            SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
        }
    }
}

