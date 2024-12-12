using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultDisplay : MonoBehaviour
{
    public TextMeshProUGUI resultText; // ���v�l��\������TextMeshProUGUI
    private bool actionTriggered = false; // A�{�^���ł̒ǉ���������x�������s�����悤�ɂ���t���O

    void Start()
    {
        // �ۑ����ꂽ�X�R�A���擾
        int score = PlayerPrefs.GetInt("Score", 0);

        // ���v�l��\��
        if (resultText != null)
        {
            resultText.text = $"���v�l: {score}";
        }
        else
        {
            Debug.LogError("ResultText ���ݒ肳��Ă��܂���I");
        }

        Debug.Log($"���v�l: {score}");
    }

    void Update()
    {
        // A�{�^���������Ď��̑���ɐi��
        if (OVRInput.GetDown(OVRInput.Button.One) && !actionTriggered)
        {
            actionTriggered = true; // ��x�������s
            HandleNextAction();
        }
    }

    private void HandleNextAction()
    {
        Debug.Log("A�{�^����������܂����B�V�[�������Z�b�g���čŏ��ɖ߂�܂��B");

        // �V�[�������Z�b�g���čŏ��ɖ߂�
        SceneManager.LoadScene("MainScene"); // "MainScene" �������V�[�����ɒu������
    }
}
