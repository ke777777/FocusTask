using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RandomNumberPlot : MonoBehaviour
{
    public int minValue = 1;
    public int maxValue = 9;
    public bool assignOnStart = true; // ���s���Ɏ������蓖�Ă��邩�ǂ���

    // ���s���Ƀ����_���Ȑ��������蓖�Ă�
    private void Start()
    {
        if (assignOnStart)
        {
            AssignRandomNumbers();
        }
    }

    // �����_���Ȑ��������蓖�āA�����̐Ԃ���������1�����F�ɂ��郁�\�b�h
    public void AssignRandomNumbers()
    {
        // �^�C���̐e�I�u�W�F�N�g���擾�i���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g�̎q�j
        Transform[] tiles = GetComponentsInChildren<Transform>();

        List<TextMeshPro> redNumbers = new List<TextMeshPro>(); // ���X�Ԃ�������ۑ����郊�X�g

        foreach (Transform tile in tiles)
        {
            // �������g�͖���
            if (tile == this.transform)
                continue;

            // �^�C���� TextMeshPro �R���|�[�l���g���擾
            TextMeshPro tmp = tile.GetComponentInChildren<TextMeshPro>();
            if (tmp != null)
            {
                // �����_���Ȑ����𐶐�
                int randomNumber = Random.Range(minValue, maxValue);

                // �e�L�X�g�ɐݒ�
                tmp.text = randomNumber.ToString();

                // �����̐Ԃ����������X�g�ɒǉ�
                if (tmp.color == Color.red)
                {
                    redNumbers.Add(tmp);
                }
            }
        }

        // �Ԃ����������݂���ꍇ�A���̒����烉���_����1�����F�ɕύX
        if (redNumbers.Count > 0)
        {
            TextMeshPro selectedNumber = redNumbers[Random.Range(0, redNumbers.Count)];
            selectedNumber.color = Color.yellow; // ���F�ɕύX
            Debug.Log($"���F�ɐݒ肳�ꂽ����: {selectedNumber.text}");
        }
        else
        {
            Debug.LogWarning("�Ԃ�������������܂���ł����B");
        }

        Debug.Log("���������蓖�āA�ԂƉ��F��ݒ肵�܂����B");
    }
}
