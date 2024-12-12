using UnityEngine;
using TMPro;

public class RandomNumberAssigner : MonoBehaviour
{
    public Transform[] tileGroups;

    private Transform[] allTiles;
    public int currentRedIndex = 0; // 最初は北1（0番目）から開始

    // 赤いタイルが変更されたときのイベント
    public delegate void RedTileChanged(string direction, int tileNumber, int tileValue);
    public event RedTileChanged OnRedTileChanged;

    private string[] directions = { "北", "東", "南", "西" };
    private int nextRedIndex = -1; // 次のタイルインデックス

    public RedTileManager redTileManager; // RedTileManagerの参照

    void Start()
    {
        allTiles = CollectAllTiles();
        AssignRandomNumbersToTiles();

        // RedTileManagerを取得
        redTileManager = FindObjectOfType<RedTileManager>();
        if (redTileManager == null)
        {
            Debug.LogError("RedTileManager が見つかりません！");
        }

        // 最初の赤いタイルを設定
        currentRedIndex = 0; // 明示的に初期位置を設定（北1）
        HighlightTile(currentRedIndex);
        Debug.Log($"最初の赤いタイルを設定: インデックス {currentRedIndex}");
    }

    void Update()
    {
        // OculusのAボタンを検出
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("Aボタンが押されました");
            // ゲームが終了していない場合のみ移動
            if (redTileManager == null || !redTileManager.isGameOver)
            {
                MoveRedHighlight();
            }
            else
            {
                Debug.Log("ゲームは終了しています。Aボタンの入力は無視されます。");
            }
        }
    }

    private void MoveRedHighlight()
    {
        // 現在の赤いタイルを元の数字に戻す
        if (currentRedIndex >= 0)
        {
            Transform currentTile = allTiles[currentRedIndex];
            TextMeshPro currentTMP = currentTile.GetComponentInChildren<TextMeshPro>();
            if (currentTMP != null)
            {
                currentTMP.color = Color.white;
                currentTMP.text = currentTMP.text.Split('\n')[0]; // 数字だけに戻す
            }
        }

        // 次のタイルへ移動
        if (nextRedIndex >= 0 && nextRedIndex < allTiles.Length)
        {
            currentRedIndex = nextRedIndex;
        }
        else
        {
            Debug.LogError("次の赤いタイルが設定されていません！");
            return;
        }

        // 新しい赤いタイルを設定
        HighlightTile(currentRedIndex);
    }

    private void HighlightTile(int tileIndex)
    {
        if (tileIndex < 0 || tileIndex >= allTiles.Length)
        {
            Debug.LogError($"無効なタイルインデックス: {tileIndex}");
            return;
        }

        Transform tile = allTiles[tileIndex];
        TextMeshPro tmp = tile.GetComponentInChildren<TextMeshPro>();
        if (tmp != null)
        {
            tmp.color = Color.white;

            // タイルの方角と番号を計算
            string direction = directions[tileIndex / 25];
            int tileNumber = (tileIndex % 25) + 1;

            // 次のタイルの情報を計算
            nextRedIndex = Random.Range(0, allTiles.Length);
            string nextDirection = directions[nextRedIndex / 25];
            int nextTileNumber = (nextRedIndex % 25) + 1;

            // 次のタイル情報を表示
            tmp.text = $"{tmp.text}\n次: {nextDirection}{nextTileNumber}";

            // 数字を取得してイベントを通知
            if (int.TryParse(tmp.text.Split('\n')[0], out int tileValue))
            {
                OnRedTileChanged?.Invoke(direction, tileNumber, tileValue);
                Debug.Log($"赤いタイル: 方角 {direction}, 番号 {tileNumber}, 値 {tileValue}, 次: {nextDirection}{nextTileNumber}");
            }
            else
            {
                Debug.LogError("赤いタイルの値を取得できませんでした");
            }
        }
    }

    private void AssignRandomNumbersToTiles()
    {
        foreach (Transform tile in allTiles)
        {
            TextMeshPro tmp = tile.GetComponentInChildren<TextMeshPro>();
            if (tmp != null)
            {
                tmp.text = Random.Range(1, 10).ToString();
                tmp.color = Color.white;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.fontSize = 4;
            }
        }
    }

    private Transform[] CollectAllTiles()
    {
        var tiles = new System.Collections.Generic.List<Transform>();
        foreach (Transform group in tileGroups)
        {
            foreach (Transform tile in group)
            {
                tiles.Add(tile);
            }
        }
        return tiles.ToArray();
    }
}
