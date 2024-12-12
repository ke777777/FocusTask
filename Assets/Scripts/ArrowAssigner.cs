using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ArrowAssigner : MonoBehaviour
{
    public int rows = 9; // �O���b�h�̍s��
    public int columns = 9; // �O���b�h�̗�
    public List<GameObject> tiles; // �^�C���̃��X�g (�^�C���v���n�u�𐶐��ς݂̂���)

    private int[,] tileDirections; // �e�^�C���̖����� (0:�Ȃ�, 1:��, 2:�E, 3:��, 4:��)

    [ContextMenu("Assign Arrows Automatically")]
    public void AssignArrowsAutomatically()
    {
        if (tiles == null || tiles.Count != rows * columns)
        {
            Debug.LogError("�^�C�����X�g���������A�O���b�h�̃T�C�Y����v���܂���B");
            return;
        }

        tileDirections = new int[rows, columns];

        // �S�[���ɂ��ǂ蒅���郋�[�g���������� (��: �X�^�[�g����S�[���܂�)
        List<Vector2Int> path = GeneratePath();

        // ���[�g�̖���ݒ�
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector2Int current = path[i];
            Vector2Int next = path[i + 1];

            if (next.x > current.x) tileDirections[current.y, current.x] = 2; // �E
            else if (next.x < current.x) tileDirections[current.y, current.x] = 4; // ��
            else if (next.y > current.y) tileDirections[current.y, current.x] = 1; // ��
            else if (next.y < current.y) tileDirections[current.y, current.x] = 3; // ��
        }

        // ���̃^�C���Ƀ����_������ݒ�
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (tileDirections[row, col] == 0)
                {
                    tileDirections[row, col] = Random.Range(1, 5); // �����_���Ȗ��
                }

                // �����^�C���ɐݒ�
                GameObject tile = tiles[row * columns + col];
                TextMeshPro tmp = tile.GetComponentInChildren<TextMeshPro>();
                if (tmp != null)
                {
                    tmp.text = GetArrowSymbol(tileDirections[row, col]);
                }
            }
        }

        Debug.Log("��󂪎����I�Ɋ��蓖�Ă��܂����B");
    }

    private List<Vector2Int> GeneratePath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = new Vector2Int(0, 0); // �X�^�[�g�n�_
        Vector2Int goal = new Vector2Int(columns - 1, rows - 1); // �S�[���n�_

        path.Add(current);

        while (current != goal)
        {
            // ���̃X�e�b�v������ (�E�����D�悵�Đi��)
            if (current.x < goal.x && (current.y == goal.y || Random.value > 0.5f))
            {
                current.x++; // �E�ɐi��
            }
            else if (current.y < goal.y)
            {
                current.y++; // ��ɐi��
            }

            path.Add(current);
        }

        return path;
    }

    private string GetArrowSymbol(int direction)
    {
        switch (direction)
        {
            case 1: return "��";
            case 2: return "��";
            case 3: return "��";
            case 4: return "��";
            default: return "";
        }
    }

    private void Start()
    {
        // �������蓖�Ă��X�^�[�g���Ɏ��s
        AssignArrowsAutomatically();
    }
}
