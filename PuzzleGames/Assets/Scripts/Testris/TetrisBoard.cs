using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBoard : MonoBehaviour
{
    // ��� ����
    // ��� ���� üũ
    // �� �� üũ

    /// <summary>
    /// �÷��̾� ��ǲ (�÷��̾�)
    /// </summary>
    Player player;

    /// <summary>
    /// ���� ��ġ Ʈ������
    /// </summary>
    private Transform spawnPoint;

    /// <summary>
    /// ��Ʈ���� ��ϵ��� �θ� Ʈ������
    /// </summary>
    private Transform tetrominoContainer;

    /// <summary>
    /// ��Ʈ���� ��� ������
    /// </summary>
    public GameObject tetrominoPrefab;

    /// <summary>
    /// ���� �� �迭 (��Ʈ���� ��� ������Ʈ Ȯ�ο�)
    /// </summary>
    private Cell[,] cells;

    /// <summary>
    /// ���� ����
    /// </summary>
    public float boardWidth;

    /// <summary>
    /// ���� ũ��
    /// </summary>
    public float boardHeight;

    /// <summary>
    /// x ����
    /// </summary>
    private int count_x;

    /// <summary>
    /// y ����
    /// </summary>
    private int count_y;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();

        Transform child = transform.GetChild(0);

        boardWidth = child.GetChild(0).transform.localScale.x;
        boardHeight = child.GetChild(0).transform.localScale.y;

        child = transform.GetChild(1);
        tetrominoContainer = child.gameObject.transform;

        child = transform.GetChild(2);
        spawnPoint = child.gameObject.transform;

        Init();
    }

    private void Update()
    {
        CheckAllBlockIsVaild();

        if(player.currentTetromino != null)
        {
            if(!player.currentTetromino.checkMoveAllow())
            {
                // ��� ���� �����
                // �ش� �����ġ ���� ����
                int index = 0;
                foreach(var obj in player.currentTetromino.GetBlocks())
                {
                    Vector2Int grid = WorldToGrid(player.currentTetromino.transform.localPosition + obj.transform.localPosition);
                    Debug.Log(grid);
                    cells[grid.y, grid.x].SetBlockObject(obj);
                }
            }
        }
    }

    public void Init()
    {
        count_x = (int)(boardWidth / 0.25f);
        count_y = (int)(boardHeight / 0.25f);

        CreateCells();
    }

    /// <summary>
    /// �� ���� �Լ�
    /// </summary>
    private void CreateCells()
    {
        cells = new Cell[count_y, count_x];

        for(int y = 0; y < count_y; y++)
        {
            for(int x = 0; x < count_x; x++)
            {
                cells[y, x] = new Cell(x, y);
            }
        }
    }

    /// <summary>
    /// ��Ʈ���� ��� ���� �Լ�
    /// </summary>
    public void CreateTetromino(ShapeType type)
    {
        Tetromino tetromino = Instantiate(tetrominoPrefab).GetComponent<Tetromino>();
        tetromino.transform.parent = tetrominoContainer;
        tetromino.transform.localPosition = spawnPoint.transform.localPosition;
        tetromino.gameObject.name = $"Tetromino_{type}";

        tetromino.Init(type);

        player.currentTetromino = tetromino; // �ӽ�
    }

    /// <summary>
    /// ����� �����ϴ� ��ġ�� �ִ��� Ȯ���ϴ� �Լ� (������Ʈ �Լ�)
    /// </summary>
    private void CheckVaildPosition()
    {
        if (player.currentTetromino == null)
            return;

        Tetromino curBlock = player.currentTetromino;
        // �����̳� ������Ʈ�� (0,0) �̹Ƿ� ��� ���ϰ� �ۼ�
        if (curBlock.transform.localPosition.x < 0) // x�� 0���� ������
        {
            curBlock.transform.localPosition = new Vector2(0, curBlock.transform.localPosition.y);
        }

        if(curBlock.transform.localPosition.x > boardWidth) // x�� ���� �ִ밪 ���� ������
        {
            curBlock.transform.localPosition = new Vector2(boardWidth - 0.25f, curBlock.transform.localPosition.y);
        }

        if(curBlock.transform.localPosition.y < 0) // y�� 0���� ������
        {
            curBlock.transform.localPosition = new Vector2(curBlock.transform.localPosition.x, 0);
        }
    }

    /// <summary>
    /// ��� ����� ���� ���� ������ �� �ִ��� Ȯ���ϴ� �Լ� (����� ��ġ�� �ʾҴ��� ���� ���� �ִ��� Ȯ��)
    /// </summary>
    /// <returns>�����ϸ� true �ƴϸ� false</returns>
    private void CheckAllBlockIsVaild()
    {
        if (player.currentTetromino == null)
             return;

        Tetromino curBlock = player.currentTetromino;
        int condition = 0; // �ش� ����� � �������� üũ�ϴ� ����

        foreach(var obj in curBlock.GetBlocks())
        {
            Vector2 pos = obj.transform.localPosition + curBlock.transform.localPosition; // �� ����� ����� ��ġ
            Vector2Int grid = WorldToGrid(pos);
            //Debug.Log($"{obj.name} : {pos}");
            // �ش���ġ�� ����� �ִ��� Ȯ��
            if (grid.x > 0 && grid.y > 0 && grid.x < count_x && grid.y < count_y)
            {
                if (!cells[grid.y, grid.x].CheckVaild())
                {
                    Debug.Log(curBlock.prevVector);
                    curBlock.transform.localPosition = curBlock.prevVector;
                }
            }

            // ������� Ȯ��
            condition = 0; // �ʱ�ȭ

            if (pos.x < 0) // 001
            {
                condition += 1;
            }

            if(pos.x > boardWidth) // 010
            {
                condition += 2;
            }

            if(pos.y < 0) // 100
            {
                condition += 4;
            }

            if(condition != 0) // ����� Ư�� ��Ȳ�� ����
            {
                SetBlockByCondition(curBlock, obj, condition); // ��� ��ġ ���
            }
        }
    }

    private void SetBlockByCondition(Tetromino curTetromino, GameObject curBlock, int condition)
    {
        int mask = 1;
        for (int i = 0; i < 3; i++) // 3���� ���� Ȯ��
        {
            if ((condition & mask) == Mathf.Pow(2, i))
            {
                switch (i)
                {
                    case 0: // x�� 0���� ����
                        curTetromino.transform.localPosition = new Vector2(0, curTetromino.transform.localPosition.y);
                        break;
                    case 1: // x�� 0���� ŭ
                        curTetromino.transform.localPosition = new Vector2(boardWidth - 0.25f, curTetromino.transform.localPosition.y);
                        break;
                    case 2: // y�� 0���� ����
                        curTetromino.transform.localPosition = new Vector2(curTetromino.transform.localPosition.x, 0);
                        if (curBlock.transform.localPosition.y < 0)
                            curTetromino.transform.localPosition += Vector3.up * 0.25f;
                        break;
                }
            }
            mask <<= 1; // ���� �ڸ� ���� �� ����
        }
    }

    // ��ǥ ��ȯ ===================================================================================

    public Vector2Int WorldToGrid(Vector2 world)
    {
        return new Vector2Int(Mathf.CeilToInt(world.x / 0.25f + 0.01f), Mathf.CeilToInt(world.y / 0.25f + 0.01f)); // 2D ������Ʈ�̿��� ������ x y�� ���
    }

    public Vector2 GridToWorld(Vector2Int grid)
    {
        return new Vector2(grid.x * 0.25f, grid.y * 0.25f);
    }
}