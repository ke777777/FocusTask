using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ArrowAssigner : MonoBehaviour
{
    public int rows = 9; // グリッドの行数
    public int columns = 9; // グリッドの列数
    public List<GameObject> tiles; // タイルのリスト (タイルプレハブを生成済みのもの)

    private int[,] tileDirections; // 各タイルの矢印方向 (0:なし, 1:上, 2:右, 3:下, 4:左)

    [ContextMenu("Assign Arrows Automatically")]
    public void AssignArrowsAutomatically()
    {
        if (tiles == null || tiles.Count != rows * columns)
        {
            Debug.LogError("タイルリストが無効か、グリッドのサイズが一致しません。");
            return;
        }

        tileDirections = new int[rows, columns];

        // ゴールにたどり着けるルートを自動生成 (例: スタートからゴールまで)
        List<Vector2Int> path = GeneratePath();

        // ルートの矢印を設定
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector2Int current = path[i];
            Vector2Int next = path[i + 1];

            if (next.x > current.x) tileDirections[current.y, current.x] = 2; // 右
            else if (next.x < current.x) tileDirections[current.y, current.x] = 4; // 左
            else if (next.y > current.y) tileDirections[current.y, current.x] = 1; // 上
            else if (next.y < current.y) tileDirections[current.y, current.x] = 3; // 下
        }

        // 他のタイルにランダム矢印を設定
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (tileDirections[row, col] == 0)
                {
                    tileDirections[row, col] = Random.Range(1, 5); // ランダムな矢印
                }

                // 矢印をタイルに設定
                GameObject tile = tiles[row * columns + col];
                TextMeshPro tmp = tile.GetComponentInChildren<TextMeshPro>();
                if (tmp != null)
                {
                    tmp.text = GetArrowSymbol(tileDirections[row, col]);
                }
            }
        }

        Debug.Log("矢印が自動的に割り当てられました。");
    }

    private List<Vector2Int> GeneratePath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = new Vector2Int(0, 0); // スタート地点
        Vector2Int goal = new Vector2Int(columns - 1, rows - 1); // ゴール地点

        path.Add(current);

        while (current != goal)
        {
            // 次のステップを決定 (右か上を優先して進む)
            if (current.x < goal.x && (current.y == goal.y || Random.value > 0.5f))
            {
                current.x++; // 右に進む
            }
            else if (current.y < goal.y)
            {
                current.y++; // 上に進む
            }

            path.Add(current);
        }

        return path;
    }

    private string GetArrowSymbol(int direction)
    {
        switch (direction)
        {
            case 1: return "↑";
            case 2: return "→";
            case 3: return "↓";
            case 4: return "←";
            default: return "";
        }
    }

    private void Start()
    {
        // 自動割り当てをスタート時に実行
        AssignArrowsAutomatically();
    }
}
