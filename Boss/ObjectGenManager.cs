using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenManager : MonoBehaviour
{
    ObjectGenPoint[] objGens;   //シーンに配置されているObjectGenPointの配列

    // Start is called before the first frame update
    void Start()
    {
        objGens = GameObject.FindObjectsOfType<ObjectGenPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        //ItemDataを探す
        ItemData[] items = GameObject.FindObjectsOfType<ItemData>();
        //ループを回して矢を探す
        for (int i = 0; i < items.Length; i++)
        {
            ItemData item = items[i];
            if (item.type == ItemType.Arrow)
            {
                return; //矢があれば何もせずにメソッドを抜ける
            }
        }
        //プレイヤーの存在と矢の数をチェックする
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (ItemKeeper.hasArrows == 0 && player != null)
        {
            //矢の数が０でプレイヤーがいる
            //配列の範囲で乱数を作る
            int index = Random.Range(0, objGens.Length);
            ObjectGenPoint objgen = objGens[index];
            objgen.ObjectCreate();   //アイテム配置
        }
    }
}