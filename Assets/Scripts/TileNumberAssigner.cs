using UnityEngine;
using TMPro; // TextMeshProを使用する場合に必要

public class TileNumberAssigner : MonoBehaviour
{
    [SerializeField] private int minNumber = 1; // ランダム番号の最小値
    [SerializeField] private int maxNumber = 9; // ランダム番号の最大値

    private void OnValidate()
    {
        AssignRandomNumbers(); // Inspector上でスクリプトを編集時にランダム番号を割り当て
    }

    private void AssignRandomNumbers()
    {
        // 自身の子オブジェクトをすべて取得
        foreach (Transform tile in transform)
        {
            // ランダム番号生成
            int randomNumber = Random.Range(minNumber, maxNumber + 1);

            // タイル名に番号を付加
            tile.name = $"Tile_{tile.GetSiblingIndex()}_Number_{randomNumber}";

            // タイルに表示する番号を設定（TextMeshProがある場合）
            TextMeshPro textComponent = tile.GetComponentInChildren<TextMeshPro>();
            if (textComponent != null)
            {
                textComponent.text = randomNumber.ToString();
            }
        }
    }
}
