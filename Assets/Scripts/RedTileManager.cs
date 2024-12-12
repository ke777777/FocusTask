using UnityEngine;
using UnityEngine.SceneManagement;

public class RedTileManager : MonoBehaviour
{
    public RandomNumberAssigner numberAssigner; // RandomNumberAssignerの参照
    public int maxHighlights = 4; // 赤いタイルを選ぶ回数の上限

    private int score = 0; // 合計値を記録
    private int highlightCount = 0; // 現在までに赤いタイルを選んだ回数
    public bool isGameOver = false; // ゲーム終了フラグ
    private bool buttonReleasedAfterGameOver = false; // Aボタンがゲーム終了後にリリースされたか

    void Start()
    {
        if (numberAssigner == null)
        {
            Debug.LogError("RandomNumberAssigner が設定されていません！");
            return;
        }

        // RandomNumberAssignerの赤いタイル変更イベントに登録
        numberAssigner.OnRedTileChanged += HandleRedTileChanged;

        // 初期タイルのスコア加算は削除
        // AddInitialTileToScore();
    }

    private void HandleRedTileChanged(string direction, int tileNumber, int tileValue)
    {
        // スコアにタイルの値を加算
        score += tileValue;
        highlightCount++;

        Debug.Log($"現在のスコア: {score}, 選択回数: {highlightCount}, 方角: {direction}, 番号: {tileNumber}");

        // 最大回数に達したらゲーム終了フラグを設定
        if (highlightCount >= maxHighlights)
        {
            isGameOver = true;
            buttonReleasedAfterGameOver = false; // ボタンがゲーム終了後にリリースされるのを待つ
            Debug.Log("ゲーム終了。Aボタンを押して結果画面に移動してください。");
        }
    }

    void Update()
    {
        if (isGameOver)
        {
            if (!buttonReleasedAfterGameOver)
            {
                // Aボタンがリリースされるのを待つ
                if (OVRInput.GetUp(OVRInput.Button.One))
                {
                    buttonReleasedAfterGameOver = true;
                }
            }
            else
            {
                // Aボタンが押されたらゲーム終了
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    EndGame();
                }
            }
        }
    }

    private void EndGame()
    {
        PlayerPrefs.SetInt("Score", score); // スコアを保存
        Debug.Log($"スコア {score} を保存し、ResultScene に移動します。");

        // 結果画面に移動
        if (Application.CanStreamedLevelBeLoaded("ResultScene"))
        {
            SceneManager.LoadScene("ResultScene");
        }
        else
        {
            Debug.LogError("ResultScene が存在しません。シーン名を確認してください。");
        }
    }

    private void OnDestroy()
    {
        if (numberAssigner != null)
        {
            // イベントから登録解除
            numberAssigner.OnRedTileChanged -= HandleRedTileChanged;
        }
    }
}
