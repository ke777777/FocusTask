using UnityEngine;
using UnityEngine.SceneManagement;

public class RedTileManager : MonoBehaviour
{
    public RandomNumberAssigner numberAssigner; // RandomNumberAssigner�̎Q��
    public int maxHighlights = 4; // �Ԃ��^�C����I�ԉ񐔂̏��

    private int score = 0; // ���v�l���L�^
    private int highlightCount = 0; // ���݂܂łɐԂ��^�C����I�񂾉�
    public bool isGameOver = false; // �Q�[���I���t���O
    private bool buttonReleasedAfterGameOver = false; // A�{�^�����Q�[���I����Ƀ����[�X���ꂽ��

    void Start()
    {
        if (numberAssigner == null)
        {
            Debug.LogError("RandomNumberAssigner ���ݒ肳��Ă��܂���I");
            return;
        }

        // RandomNumberAssigner�̐Ԃ��^�C���ύX�C�x���g�ɓo�^
        numberAssigner.OnRedTileChanged += HandleRedTileChanged;

        // �����^�C���̃X�R�A���Z�͍폜
        // AddInitialTileToScore();
    }

    private void HandleRedTileChanged(string direction, int tileNumber, int tileValue)
    {
        // �X�R�A�Ƀ^�C���̒l�����Z
        score += tileValue;
        highlightCount++;

        Debug.Log($"���݂̃X�R�A: {score}, �I����: {highlightCount}, ���p: {direction}, �ԍ�: {tileNumber}");

        // �ő�񐔂ɒB������Q�[���I���t���O��ݒ�
        if (highlightCount >= maxHighlights)
        {
            isGameOver = true;
            buttonReleasedAfterGameOver = false; // �{�^�����Q�[���I����Ƀ����[�X�����̂�҂�
            Debug.Log("�Q�[���I���BA�{�^���������Č��ʉ�ʂɈړ����Ă��������B");
        }
    }

    void Update()
    {
        if (isGameOver)
        {
            if (!buttonReleasedAfterGameOver)
            {
                // A�{�^���������[�X�����̂�҂�
                if (OVRInput.GetUp(OVRInput.Button.One))
                {
                    buttonReleasedAfterGameOver = true;
                }
            }
            else
            {
                // A�{�^���������ꂽ��Q�[���I��
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    EndGame();
                }
            }
        }
    }

    private void EndGame()
    {
        PlayerPrefs.SetInt("Score", score); // �X�R�A��ۑ�
        Debug.Log($"�X�R�A {score} ��ۑ����AResultScene �Ɉړ����܂��B");

        // ���ʉ�ʂɈړ�
        if (Application.CanStreamedLevelBeLoaded("ResultScene"))
        {
            SceneManager.LoadScene("ResultScene");
        }
        else
        {
            Debug.LogError("ResultScene �����݂��܂���B�V�[�������m�F���Ă��������B");
        }
    }

    private void OnDestroy()
    {
        if (numberAssigner != null)
        {
            // �C�x���g����o�^����
            numberAssigner.OnRedTileChanged -= HandleRedTileChanged;
        }
    }
}
