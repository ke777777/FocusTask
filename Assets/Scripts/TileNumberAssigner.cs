using TMPro;
using UnityEngine;

public class TileNumberAssigner : MonoBehaviour
{
    [ContextMenu("Assign Random Numbers to TMP Text")]
    public void AssignRandomNumbers()
    {
        // TextMeshPro コンポーネントをすべて取得 (TextMeshProUGUI または TextMeshPro)
        TextMeshPro[] textComponents = GetComponentsInChildren<TextMeshPro>(true);

        if (textComponents.Length == 0)
        {
            Debug.LogError("Text (TMP) コンポーネントが見つかりません。");
            return;
        }

        // ランダムな番号を各Text (TMP)に割り当てる
        foreach (var textComponent in textComponents)
        {
            int randomValue = Random.Range(1, 10); // 1〜9のランダムな値
            textComponent.text = randomValue.ToString();
        }

        Debug.Log($"全てのText (TMP)にランダムな番号を割り当てました。数: {textComponents.Length}");
    }
}
