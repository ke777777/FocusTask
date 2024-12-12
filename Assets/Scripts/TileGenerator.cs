using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TileGenerator : MonoBehaviour
{
    public GameObject tilePrefab; // タイルのプレハブ
    public int numberOfGroups = 12; // 円周上のグループ数
    public int tilesPerGroup = 6; // 各グループの縦のタイル数
    public float radius = 5f; // 円の半径
    public float verticalSpacing = 1.5f; // タイル間の垂直間隔

    private List<GameObject> tiles = new List<GameObject>(); // タイルのリスト

    [ContextMenu("Generate Tiles")]
    public void GenerateTiles()
    {
        DeleteExistingTiles();
        tiles.Clear();

        Vector3 userPosition = transform.position;

        float angleStep = 360f / numberOfGroups;

        // 通常タイルの生成
        for (int i = 0; i < numberOfGroups; i++)
        {
            // 列ごとにまとめるための親オブジェクトを作成
            GameObject columnParent = new GameObject($"Column_{i + 1}");
            columnParent.transform.parent = this.transform;

            float angle = i * angleStep;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 groupPosition = new Vector3(
                Mathf.Sin(rad) * radius,
                userPosition.y,
                Mathf.Cos(rad) * radius
            ) + userPosition;

            for (int j = 0; j < tilesPerGroup; j++)
            {
                Vector3 tilePosition = groupPosition + new Vector3(0, j * verticalSpacing, 0);

                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tile.transform.localScale *= 3f;
                tile.transform.rotation = Quaternion.LookRotation(
                    tilePosition - new Vector3(userPosition.x, tilePosition.y, userPosition.z)
                );

                // タイルに "1" を設定
                TextMeshPro tmp = tile.GetComponentInChildren<TextMeshPro>();
                if (tmp != null)
                {
                    tmp.text = "1"; // デフォルトのタイル番号は "1"
                    tmp.alignment = TextAlignmentOptions.Center;
                    tmp.fontSize = 2;
                    tmp.color = Color.white;
                }

                tile.transform.parent = columnParent.transform; // 列ごとにまとめる
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

        // ランダムにスタートとゴールのインデックスを選択
        int startIndex = 0; // 最初のタイルをスタートにする場合
        int goalIndex = tiles.Count - 1; // 最後のタイルをゴールにする場合

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
