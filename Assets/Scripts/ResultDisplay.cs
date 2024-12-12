using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultDisplay : MonoBehaviour
{
    public TextMeshProUGUI resultText; // 合計値を表示するTextMeshProUGUI
    private bool actionTriggered = false; // Aボタンでの追加処理が一度だけ実行されるようにするフラグ

    void Start()
    {
        // 保存されたスコアを取得
        int score = PlayerPrefs.GetInt("Score", 0);

        // 合計値を表示
        if (resultText != null)
        {
            resultText.text = $"合計値: {score}";
        }
        else
        {
            Debug.LogError("ResultText が設定されていません！");
        }

        Debug.Log($"合計値: {score}");
    }

    void Update()
    {
        // Aボタンを押して次の操作に進む
        if (OVRInput.GetDown(OVRInput.Button.One) && !actionTriggered)
        {
            actionTriggered = true; // 一度だけ実行
            HandleNextAction();
        }
    }

    private void HandleNextAction()
    {
        Debug.Log("Aボタンが押されました。シーンをリセットして最初に戻ります。");

        // シーンをリセットして最初に戻る
        SceneManager.LoadScene("MainScene"); // "MainScene" を初期シーン名に置き換え
    }
}
