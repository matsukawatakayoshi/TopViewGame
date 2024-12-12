using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int arrangeId = 0; // 配置の識別に使う
    public bool isGoldDoor = false; // 金のドア
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
        if (collision.gameObject.tag == "Player")
        {
            // 鍵を持っている
            if (isGoldDoor)
            {
                //Debug.Log("ふれた");
                if (ItemKeeper.hasGoldKeys > 0)
                {
                    ItemKeeper.hasGoldKeys--; // 金の鍵を一つ減らす
                    Destroy(gameObject);   // ドアを開ける（削除する）
                    // 配置Idの記録
                    SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
                }
            }
            else
            {
                if(ItemKeeper.hasSilverKeys > 0)
                {
                    ItemKeeper.hasSilverKeys--; // 銀の鍵を一つ減らす
                    Destroy(gameObject);   // ドアを開ける（削除する）
                    // 配置Idの記録
                    SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
                }
            }
        }
    }
}
