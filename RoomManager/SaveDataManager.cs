using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{

    public static SaveDataList arrangeDataList; //別途定義してあるSaveDataListクラスの仕組みを活用して記録する


    // Start is called before the first frame update
    void Start()
    {
        //①最初にSaveDataListクラスを実体化
        arrangeDataList = new SaveDataList();
        //②saveDataListクラスが使えるsaveDatas配列（SaveData達を格納できる）を用意しておく
        arrangeDataList.saveDatas = new SaveData[] { };

        //現在どのシーンかを取得
        string stageName = PlayerPrefs.GetString("LastScene");

        //シーン名をキーにして過去の状態（JSON化として保存されているデータ）を取得
        string data = PlayerPrefs.GetString(stageName);

        //もし保存されている情報が空じゃなければ
        if (data != "")
        {
            //取得したデータ(JSON化されていたデータ)をSaveDataListが扱える形に変換する
            arrangeDataList = JsonUtility.FromJson<SaveDataList>(data);

            //この時点でarrageDataList(SaveDataListクラス)が扱っているsaveDatas配列に情報が入ったはず。
            //配列の中の情報をひとつずつ順番に読み取っていく。
            //情報と一致するオブジェクトは抹消する

            for (int i = 0; i < arrangeDataList.saveDatas.Length; i++)
            {
                //まずはSaveData型の情報をひとつずつ取得
                SaveData savedata = arrangeDataList.saveDatas[i];

                //取得したSaveData型が扱っているタグ情報(objTag)と同じオブジェクトをシーン中から全部集める(GameObject配列objectsに格納していく)
                string objTag = savedata.objTag;
                GameObject[] objects = GameObject.FindGameObjectsWithTag(objTag);

                //配列objectsに集めたシーン上のオブジェクト達をひとつずつチェック、arrangeIdが一致している物がないかを調べる
                for (int ii = 0; ii < objects.Length; ii++)
                {
                    GameObject obj = objects[ii];//チェック対象の配列のGameObjectからその時のチェック対象をobjに。

                    //調査対象のタグが「Door」だった場合
                    if (objTag == "Door")
                    {
                        //シーン上のDoorオブジェクトのスクリプトを取得
                        Door door = obj.GetComponent<Door>();
                        //シーン上のDoorオブジェクトが持っているarrangeId(door.arrangeId)とJSON化して記録されていたarrangeIdが一致していたら
                        if (door.arrangeId == savedata.arrangeId)
                        {
                            //該当するシーン上のDoorオブジェクトは過去に開けられた記録があるという事なので抹消
                            Destroy(obj);
                        }
                    }

                    //調査対象のタグが「ItemBox」だった場合
                    else if (objTag == "ItemBox")
                    {
                        //シーン上のItemBoxオブジェクトのスクリプトを取得
                        ItemBox box = obj.GetComponent<ItemBox>();
                        //シーン上のItemBoxオブジェクトが持っているarrangeId(box.arrangeId)とJSON化して記録されていたarrangeIdが一致していたら
                        if (box.arrangeId == savedata.arrangeId)
                        {
                            //該当するシーン上のItemBoxオブジェクトは過去に開けられた記録があるという事なので「isClosed」フラグをfalseにして、見た目をかえておく
                            box.isClosed = false;
                            box.GetComponent<SpriteRenderer>().sprite = box.openImage;
                        }
                    }

                    //調査対象のタグが「Item」だった場合
                    else if (objTag == "Item")
                    {
                        //シーン上のItemオブジェクトのスクリプトを取得
                        ItemData item = obj.GetComponent<ItemData>();
                        //シーン上のItemオブジェクトが持っているarrangeId(item.arrangeId)とJSON化して記録されていたarrangeIdが一致していたら
                        if (item.arrangeId == savedata.arrangeId)
                        {
                            //該当するシーン上のItemオブジェクトは過去に取得済みという記録があるという事なので抹消
                            Destroy(obj);
                        }
                    }

                    //調査対象のタグが「Enemy」だった場合
                    else if (objTag == "Enemy")
                    {
                        //シーン上のEnemyオブジェクトのスクリプトを取得
                        EnemyController enemy = obj.GetComponent<EnemyController>();
                        //シーン上のEnemyオブジェクトが持っているarrangeId(enemy.arrangeId)とJSON化して記録されていたarrangeIdが一致していたら
                        if (enemy.arrangeId == savedata.arrangeId)
                        {
                            //該当するシーン上のEnemyオブジェクトは過去に撃退済みという記録があるという事なので抹消
                            Destroy(obj);
                        }
                    }


                }
            }

        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    //消化したオブジェクトの記録（都度更新 ※確定ではない）
    //※他のスクリプトから使われるメソッド
    public static void SetArrangeId(int arrangeId, string objTag)
    {
        //対象のarrangeIdが0、またはタグ設定がなければ記録しない
        if (arrangeId == 0 || objTag == "")
        {
            //記録しない
            return;
        }

        //追加するために１つ多いSaveData配列を作る
        Debug.Log(arrangeDataList.saveDatas.Length);
        SaveData[] newSavedatas = new SaveData[arrangeDataList.saveDatas.Length + 1];
        //データをコピーする
        for (int i = 0; i < arrangeDataList.saveDatas.Length; i++)
        {
            newSavedatas[i] = arrangeDataList.saveDatas[i];
        }
        //SaveData作成
        SaveData savedata = new SaveData();
        savedata.arrangeId = arrangeId; //Idを記録
        savedata.objTag = objTag;       //タグを記録
        //SaveData追加
        newSavedatas[arrangeDataList.saveDatas.Length] = savedata;
        arrangeDataList.saveDatas = newSavedatas;
    }

    //PlayerPrefsで保存※シーン切り替えの時に他のスクリプトから呼ばれるメソッド
    public static void SaveArrangeData(string stageName)
    {
        //その時点までに蓄積されている配列がある　かつ シーン名がちゃんと取得できているなら
        if (arrangeDataList.saveDatas != null && stageName != "")
        {
            //そこまで随時まとめてきたSaveDataListをJSONデータに変換
            string saveJson = JsonUtility.ToJson(arrangeDataList);
            //シーン名をキーにしてPlayerPrefsで保存
            PlayerPrefs.SetString(stageName, saveJson);
        }
    }

}