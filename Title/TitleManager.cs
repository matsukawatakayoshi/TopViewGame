using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour
{
    public GameObject startButoon;  // スタートボタン
    public GameObject continueButton; // コンティニューボタン
    public string firstSceneName;       // ゲーム開始シーン名　

    // Start is called before the first frame update
    void Start()
    {
        string sceneName = PlayerPrefs.GetString("LastScne");    // 保存シーン
        if (sceneName == "" ) // LastSceneにデータがない
        {
            continueButton.GetComponent<Button>().interactable = false; // 無効化
        }
        else // LastSceneにデータがある
        {
            continueButton.GetComponent<Button>().interactable = true; // 有効化
        }
        // タイトルBGM再生
        SoundManager.soundManager.PlayBgm(BGMType.Title);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // スタートボタン押し
    public void StartButtonClicked()
    {
        // セーブデータをクリア
        PlayerPrefs.DeleteAll();
        // HPを戻す
        PlayerPrefs.SetInt("PlayerHP", 3);
        // ステージ情報をクリア
        PlayerPrefs.SetString("LastScne", firstSceneName); //シーン名初期化
        RoomManager.doorNumber = 0;

        SceneManager.LoadScene(firstSceneName);
    }

    // 続きからボタン押し
    public void ContinueButtonClicked()
    {
        string sceneName = PlayerPrefs.GetString("LastScene");  // 最後のシーン
        RoomManager.doorNumber = PlayerPrefs.GetInt("LastDoor"); // 最後にくぐったドア番号
        SceneManager.LoadScene(sceneName);
    }
}
