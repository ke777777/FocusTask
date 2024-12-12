using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraEffect : MonoBehaviour
{
    public Material effectMaterial; // 作成したマテリアルを割り当てる

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (effectMaterial != null)
        {
            Graphics.Blit(source, destination, effectMaterial); // マテリアルを適用
        }
        else
        {
            Graphics.Blit(source, destination); // デフォルトの描画
        }
    }
}
