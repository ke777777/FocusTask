using UnityEngine;
using TMPro;

public class RandomNumberAssigner : MonoBehaviour
{
    public Transform[] tileGroups;

    private Transform[] allTiles;
    public int currentRedIndex = 0; // �ŏ��͖k1�i0�Ԗځj����J�n

    // �Ԃ��^�C�����ύX���ꂽ�Ƃ��̃C�x���g
    public delegate void RedTileChanged(string direction, int tileNumber, int tileValue);
    public event RedTileChanged OnRedTileChanged;

    private string[] directions = { "�k", "��", "��", "��" };
    private int nextRedIndex = -1; // ���̃^�C���C���f�b�N�X

    public RedTileManager redTileManager; // RedTileManager�̎Q��

    void Start()
    {
        allTiles = CollectAllTiles();
        AssignRandomNumbersToTiles();

        // RedTileManager���擾
        redTileManager = FindObjectOfType<RedTileManager>();
        if (redTileManager == null)
        {
            Debug.LogError("RedTileManager ��������܂���I");
        }

        // �ŏ��̐Ԃ��^�C����ݒ�
        currentRedIndex = 0; // �����I�ɏ����ʒu��ݒ�i�k1�j
        HighlightTile(currentRedIndex);
        Debug.Log($"�ŏ��̐Ԃ��^�C����ݒ�: �C���f�b�N�X {currentRedIndex}");
    }

    void Update()
    {
        // Oculus��A�{�^�������o
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("A�{�^����������܂���");
            // �Q�[�����I�����Ă��Ȃ��ꍇ�݈̂ړ�
            if (redTileManager == null || !redTileManager.isGameOver)
            {
                MoveRedHighlight();
            }
            else
            {
                Debug.Log("�Q�[���͏I�����Ă��܂��BA�{�^���̓��͖͂�������܂��B");
            }
        }
    }

    private void MoveRedHighlight()
    {
        // ���݂̐Ԃ��^�C�������̐����ɖ߂�
        if (currentRedIndex >= 0)
        {
            Transform currentTile = allTiles[currentRedIndex];
            TextMeshPro currentTMP = currentTile.GetComponentInChildren<TextMeshPro>();
            if (currentTMP != null)
            {
                currentTMP.color = Color.white;
                currentTMP.text = currentTMP.text.Split('\n')[0]; // ���������ɖ߂�
            }
        }

        // ���̃^�C���ֈړ�
        if (nextRedIndex >= 0 && nextRedIndex < allTiles.Length)
        {
            currentRedIndex = nextRedIndex;
        }
        else
        {
            Debug.LogError("���̐Ԃ��^�C�����ݒ肳��Ă��܂���I");
            return;
        }

        // �V�����Ԃ��^�C����ݒ�
        HighlightTile(currentRedIndex);
    }

    private void HighlightTile(int tileIndex)
    {
        if (tileIndex < 0 || tileIndex >= allTiles.Length)
        {
            Debug.LogError($"�����ȃ^�C���C���f�b�N�X: {tileIndex}");
            return;
        }

        Transform tile = allTiles[tileIndex];
        TextMeshPro tmp = tile.GetComponentInChildren<TextMeshPro>();
        if (tmp != null)
        {
            tmp.color = Color.white;

            // �^�C���̕��p�Ɣԍ����v�Z
            string direction = directions[tileIndex / 25];
            int tileNumber = (tileIndex % 25) + 1;

            // ���̃^�C���̏����v�Z
            nextRedIndex = Random.Range(0, allTiles.Length);
            string nextDirection = directions[nextRedIndex / 25];
            int nextTileNumber = (nextRedIndex % 25) + 1;

            // ���̃^�C������\��
            tmp.text = $"{tmp.text}\n��: {nextDirection}{nextTileNumber}";

            // �������擾���ăC�x���g��ʒm
            if (int.TryParse(tmp.text.Split('\n')[0], out int tileValue))
            {
                OnRedTileChanged?.Invoke(direction, tileNumber, tileValue);
                Debug.Log($"�Ԃ��^�C��: ���p {direction}, �ԍ� {tileNumber}, �l {tileValue}, ��: {nextDirection}{nextTileNumber}");
            }
            else
            {
                Debug.LogError("�Ԃ��^�C���̒l���擾�ł��܂���ł���");
            }
        }
    }

    private void AssignRandomNumbersToTiles()
    {
        foreach (Transform tile in allTiles)
        {
            TextMeshPro tmp = tile.GetComponentInChildren<TextMeshPro>();
            if (tmp != null)
            {
                tmp.text = Random.Range(1, 10).ToString();
                tmp.color = Color.white;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.fontSize = 4;
            }
        }
    }

    private Transform[] CollectAllTiles()
    {
        var tiles = new System.Collections.Generic.List<Transform>();
        foreach (Transform group in tileGroups)
        {
            foreach (Transform tile in group)
            {
                tiles.Add(tile);
            }
        }
        return tiles.ToArray();
    }
}
