using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraEffect : MonoBehaviour
{
    public Material effectMaterial; // �쐬�����}�e���A�������蓖�Ă�

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (effectMaterial != null)
        {
            Graphics.Blit(source, destination, effectMaterial); // �}�e���A����K�p
        }
        else
        {
            Graphics.Blit(source, destination); // �f�t�H���g�̕`��
        }
    }
}
