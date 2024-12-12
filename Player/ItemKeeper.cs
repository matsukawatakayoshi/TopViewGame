using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKeeper : MonoBehaviour
{
    public static int hasGoldKeys = 0;     // 金鍵の数
    public static int hasSilverKeys = 0;   // 銀鍵の数
    public static int hasArrows = 0;       // 矢の数
    public static int hasLights = 0;        // ライトの数

    // Start is called before the first frame update
    void Start()
    {
        // アイテムを読み込む
        hasGoldKeys = PlayerPrefs.GetInt("GoldKeys");
        hasSilverKeys = PlayerPrefs.GetInt("SilverKeys");
        hasArrows = PlayerPrefs.GetInt("Arrows");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // アイテムを保存する
    public static void SaveItem()
    {
        PlayerPrefs.SetInt("GoldKeys", hasGoldKeys);
        PlayerPrefs.SetInt("SilverKeys", hasSilverKeys);
        PlayerPrefs.SetInt("Arrows", hasArrows);
    }
}
