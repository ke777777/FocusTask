using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TileGenerator : MonoBehaviour
{
    public GameObject tilePrefab; // �^�C���̃v���n�u
    public int numberOfGroups = 12; // �~����̃O���[�v��
    public int tilesPerGroup = 6; // �e�O���[�v�̏c�̃^�C����
    public float radius = 5f; // �~�̔��a
    public float verticalSpacing = 1.5f; // �^�C���Ԃ̐����Ԋu

    private List<GameObject> tiles = new List<GameObject>(); // �^�C���̃��X�g

    [ContextMenu("Generate Tiles")]
    public void GenerateTiles()
    {
        DeleteExistingTiles();
        tiles.Clear();

        Vector3 userPosition = transform.position;

        float angleStep = 360f / numberOfGroups;

        // �ʏ�^�C���̐���
        for (int i = 0; i < numberOfGroups; i++)
        {
            // �񂲂Ƃɂ܂Ƃ߂邽�߂̐e�I�u�W�F�N�g���쐬
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

                // �^�C���� "1" ��ݒ�
                TextMeshPro tmp = tile.GetComponentInChildren<TextMeshPro>();
                if (tmp != null)
                {
                    tmp.text = "1"; // �f�t�H���g�̃^�C���ԍ��� "1"
                    tmp.alignment = TextAlignmentOptions.Center;
                    tmp.fontSize = 2;
                    tmp.color = Color.white;
                }

                tile.transform.parent = columnParent.transform; // �񂲂Ƃɂ܂Ƃ߂�
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

        // �����_���ɃX�^�[�g�ƃS�[���̃C���f�b�N�X��I��
        int startIndex = 0; // �ŏ��̃^�C�����X�^�[�g�ɂ���ꍇ
        int goalIndex = tiles.Count - 1; // �Ō�̃^�C�����S�[���ɂ���ꍇ

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
