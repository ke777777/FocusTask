using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RandomNumberPlot : MonoBehaviour
{
    public int minValue = 1;
    public int maxValue = 9;
    public bool assignOnStart = true; // 実行時に自動割り当てするかどうか

    // 実行時にランダムな数字を割り当てる
    private void Start()
    {
        if (assignOnStart)
        {
            AssignRandomNumbers();
        }
    }

    // ランダムな数字を割り当て、既存の赤い数字から1つを黄色にするメソッド
    public void AssignRandomNumbers()
    {
        // タイルの親オブジェクトを取得（このスクリプトがアタッチされているオブジェクトの子）
        Transform[] tiles = GetComponentsInChildren<Transform>();

        List<TextMeshPro> redNumbers = new List<TextMeshPro>(); // 元々赤い数字を保存するリスト

        foreach (Transform tile in tiles)
        {
            // 自分自身は無視
            if (tile == this.transform)
                continue;

            // タイルの TextMeshPro コンポーネントを取得
            TextMeshPro tmp = tile.GetComponentInChildren<TextMeshPro>();
            if (tmp != null)
            {
                // ランダムな数字を生成
                int randomNumber = Random.Range(minValue, maxValue);

                // テキストに設定
                tmp.text = randomNumber.ToString();

                // 既存の赤い数字をリストに追加
                if (tmp.color == Color.red)
                {
                    redNumbers.Add(tmp);
                }
            }
        }

        // 赤い数字が存在する場合、その中からランダムに1つを黄色に変更
        if (redNumbers.Count > 0)
        {
            TextMeshPro selectedNumber = redNumbers[Random.Range(0, redNumbers.Count)];
            selectedNumber.color = Color.yellow; // 黄色に変更
            Debug.Log($"黄色に設定された数字: {selectedNumber.text}");
        }
        else
        {
            Debug.LogWarning("赤い数字が見つかりませんでした。");
        }

        Debug.Log("数字を割り当て、赤と黄色を設定しました。");
    }
}
