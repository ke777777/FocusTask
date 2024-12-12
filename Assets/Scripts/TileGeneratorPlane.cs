using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TileGeneratorPlane : MonoBehaviour
{
    public GameObject tilePrefab; // タイルのプレハブ
    public int rows = 9; // グリッドの行数
    public int columns = 9; // グリッドの列数
    public float tileSpacing = 1.0f; // タイル間の間隔

    private List<GameObject> tiles = new List<GameObject>(); // タイルのリスト

    [ContextMenu("Generate Tiles")]
    public void GenerateTiles()
    {
        DeleteExistingTiles();
        tiles.Clear();

        Vector3 startPosition = transform.position;

        // グリッドの生成
        for (int col = 0; col < columns; col++)
        {
            // 列ごとに親オブジェクトを作成
            GameObject columnParent = new GameObject($"Column_{col + 1}");
            columnParent.transform.parent = this.transform;

            for (int row = 0; row < rows; row++)
            {
                // タイルの位置を計算
                Vector3 tilePosition = new Vector3(
                    startPosition.x + col * tileSpacing,
                    startPosition.y + row * tileSpacing,
                    startPosition.z // XY平面上なのでZ座標は固定
                );

                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tile.transform.localScale *= 1f; // 必要に応じてサイズ調整

                // タイルに "1" を設定
                TextMeshPro tmp = tile.GetComponentInChildren<TextMeshPro>();
                if (tmp != null)
                {
                    tmp.text = "1"; // デフォルトのタイル番号は "1"
                    tmp.alignment = TextAlignmentOptions.Center;
                    tmp.fontSize = 2;
                    tmp.color = Color.white;
                }

                tile.transform.parent = columnParent.transform; // 列の親オブジェクトに設定
                tiles.Add(tile);
            }
        }

        SetStartAndGoal(); // スタートとゴールをタイルから選択して設定
        Debug.Log("タイルの生成が完了しました。");
    }

    private void SetStartAndGoal()
    {
        if (tiles.Count < 2)
        {
            Debug.LogWarning("タイルが少なすぎます。スタートとゴールを設定できません。");
            return;
        }

        // スタートとゴールのインデックスを設定
        int startIndex = 0; // 最初のタイルをスタートにする
        int goalIndex = tiles.Count - 1; // 最後のタイルをゴールにする

        // スタートタイルの設定
        GameObject startTile = tiles[startIndex];
        TextMeshPro startText = startTile.GetComponentInChildren<TextMeshPro>();
        if (startText != null)
        {
            startText.text = "S"; // スタートタイルに "S" を設定
            startText.color = Color.green;
            startText.alignment = TextAlignmentOptions.Center;
        }

        // ゴールタイルの設定
        GameObject goalTile = tiles[goalIndex];
        TextMeshPro goalText = goalTile.GetComponentInChildren<TextMeshPro>();
        if (goalText != null)
        {
            goalText.text = "G"; // ゴールタイルに "G" を設定
            goalText.color = Color.red;
            goalText.alignment = TextAlignmentOptions.Center;
        }
    }

    private void DeleteExistingTiles()
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            DestroyImmediate(child);
        }
    }
}
