using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBoard : MonoBehaviour
{
    // ��� ����
    // ��� ���� üũ
    // �� �� üũ

    /// <summary>
    /// ���� ��ġ Ʈ������
    /// </summary>
    private Transform spawnPoint;

    private Transform tetrominoContainer;

    public GameObject tetrominoObjects;

    /// <summary>
    /// ���� ����
    /// </summary>
    public float boardWidth;

    /// <summary>
    /// ���� ũ��
    /// </summary>
    public float boardHeight;

    /// <summary>
    /// ���� �� �迭 (��Ʈ���� ��� ������Ʈ Ȯ�ο�)
    /// </summary>
    private Cell[] cells;

    private void Awake()
    {
        Transform child = transform.GetChild(0);

        boardWidth = child.GetChild(0).transform.localScale.x;
        boardHeight = child.GetChild(0).transform.localScale.y;

        child = transform.GetChild(1);
        tetrominoContainer = child.gameObject.transform;

        child = transform.GetChild(2);
        spawnPoint = child.gameObject.transform;
    }

    public void Init()
    {
        CreateCells();
    }

    /// <summary>
    /// �� ���� �Լ�
    /// </summary>
    private void CreateCells()
    {
        int count_x = (int)(boardWidth * 0.25f);
        int count_y = (int)(boardHeight * 0.25f);

        cells = new Cell[count_x * count_y];

        for(int y = 0; y < count_y; y++)
        {
            for(int x = 0; x < count_x; x++)
            {
                cells[y * count_x + x] = new Cell(x, y);
            }
        }
    }

    /// <summary>
    /// ��Ʈ���� ��� ���� �Լ�
    /// </summary>
    public void CreateTetromino(ShapeType type)
    {
        Tetromino tetromino = Instantiate(tetrominoObjects).GetComponent<Tetromino>();
        tetromino.transform.parent = tetrominoContainer;
        tetromino.transform.localPosition = spawnPoint.transform.localPosition;

        tetromino.Init(type, boardWidth, boardHeight);
    }
}