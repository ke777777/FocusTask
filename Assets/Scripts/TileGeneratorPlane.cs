using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TileGeneratorPlane : MonoBehaviour
{
    public GameObject tilePrefab; // �^�C���̃v���n�u
    public int rows = 9; // �O���b�h�̍s��
    public int columns = 9; // �O���b�h�̗�
    public float tileSpacing = 1.0f; // �^�C���Ԃ̊Ԋu

    private List<GameObject> tiles = new List<GameObject>(); // �^�C���̃��X�g

    [ContextMenu("Generate Tiles")]
    public void GenerateTiles()
    {
        DeleteExistingTiles();
        tiles.Clear();

        Vector3 startPosition = transform.position;

        // �O���b�h�̐���
        for (int col = 0; col < columns; col++)
        {
            // �񂲂Ƃɐe�I�u�W�F�N�g���쐬
            GameObject columnParent = new GameObject($"Column_{col + 1}");
            columnParent.transform.parent = this.transform;

            for (int row = 0; row < rows; row++)
            {
                // �^�C���̈ʒu���v�Z
                Vector3 tilePosition = new Vector3(
                    startPosition.x + col * tileSpacing,
                    startPosition.y + row * tileSpacing,
                    startPosition.z // XY���ʏ�Ȃ̂�Z���W�͌Œ�
                );

                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tile.transform.localScale *= 1f; // �K�v�ɉ����ăT�C�Y����

                // �^�C���� "1" ��ݒ�
                TextMeshPro tmp = tile.GetComponentInChildren<TextMeshPro>();
                if (tmp != null)
                {
                    tmp.text = "1"; // �f�t�H���g�̃^�C���ԍ��� "1"
                    tmp.alignment = TextAlignmentOptions.Center;
                    tmp.fontSize = 2;
                    tmp.color = Color.white;
                }

                tile.transform.parent = columnParent.transform; // ��̐e�I�u�W�F�N�g�ɐݒ�
                tiles.Add(tile);
            }
        }

        SetStartAndGoal(); // �X�^�[�g�ƃS�[�����^�C������I�����Đݒ�
        Debug.Log("�^�C���̐������������܂����B");
    }

    private void SetStartAndGoal()
    {
        if (tiles.Count < 2)
        {
            Debug.LogWarning("�^�C�������Ȃ����܂��B�X�^�[�g�ƃS�[����ݒ�ł��܂���B");
            return;
        }

        // �X�^�[�g�ƃS�[���̃C���f�b�N�X��ݒ�
        int startIndex = 0; // �ŏ��̃^�C�����X�^�[�g�ɂ���
        int goalIndex = tiles.Count - 1; // �Ō�̃^�C�����S�[���ɂ���

        // �X�^�[�g�^�C���̐ݒ�
        GameObject startTile = tiles[startIndex];
        TextMeshPro startText = startTile.GetComponentInChildren<TextMeshPro>();
        if (startText != null)
        {
            startText.text = "S"; // �X�^�[�g�^�C���� "S" ��ݒ�
            startText.color = Color.green;
            startText.alignment = TextAlignmentOptions.Center;
        }

        // �S�[���^�C���̐ݒ�
        GameObject goalTile = tiles[goalIndex];
        TextMeshPro goalText = goalTile.GetComponentInChildren<TextMeshPro>();
        if (goalText != null)
        {
            goalText.text = "G"; // �S�[���^�C���� "G" ��ݒ�
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
