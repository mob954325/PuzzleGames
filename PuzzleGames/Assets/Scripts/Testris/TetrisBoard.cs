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
        if(player.currentTetromino != null)
        {
            if(!player.currentTetromino.checkMoveAllow()) // �ش� �����ġ ���� ����
            {
                // ��� ���� �����
                foreach(var obj in player.currentTetromino.GetBlocks())
                {
                    Vector2Int grid = WorldToGrid(player.currentTetromino.transform.localPosition + obj.transform.localPosition);
                    Debug.Log($"{obj.name} {grid.x} , {grid.y}");
                    cells[grid.y, grid.x].SetBlockObject(obj);
                }

                // �� �� üũ
                CheckHorizontal();
            }
        }

        CheckAllBlockIsVaild();
    }
    
    /// <summary>
    /// �ʱ�ȭ �Լ�
    /// </summary>
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
        tetromino.transform.localPosition = spawnPoint.transform.localPosition; //
        tetromino.gameObject.name = $"Tetromino_{type}";

        tetromino.Init(type);

        player.currentTetromino = tetromino; // �ӽ�
        player.Init();
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

            Debug.Log($"{obj.gameObject.name} : {pos} {grid}");
            // �ش���ġ�� ����� �ִ��� Ȯ��
            if (grid.x >= 0 && grid.y >= 0 && grid.x < count_x && grid.y < count_y + 5f) //
            {
                if (!cells[grid.y, grid.x].CheckVaild())
                {
                    curBlock.transform.localPosition = curBlock.prevVector;
                    curBlock.SetMoveAllow(false);
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

    /// <summary>
    /// ��� ��ġ �����ϴ� �Լ� (CheckAllBlockIsVaild �Լ���)
    /// </summary>
    /// <param name="curTetromino">��Ʈ���� ���</param>
    /// <param name="curBlock">���� üũ�ϰ� �ִ� ��Ʈ���� ����� �ڽ� ���</param>
    /// <param name="condition">���� ��� ����</param>
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
                        if (curTetromino.transform.localPosition.x + curBlock.transform.localPosition.x < 0)
                            curTetromino.transform.localPosition += Vector3.right * 0.25f;
                        break;
                    case 1: // x�� boardWidth ���� ŭ
                        curTetromino.transform.localPosition = new Vector2(boardWidth - 0.25f, curTetromino.transform.localPosition.y);
                        if (curTetromino.transform.localPosition.x + curBlock.transform.localPosition.x > boardWidth - 0.25f)
                            curTetromino.transform.localPosition += Vector3.left * 0.25f;
                            break;
                    case 2: // y�� 0���� ����
                        curTetromino.transform.localPosition = new Vector2(curTetromino.transform.localPosition.x, 0);
                        if (curTetromino.transform.localPosition.y + curBlock.transform.localPosition.y < 0)
                            curTetromino.transform.localPosition += Vector3.up * 0.25f;
                        break;
                }
            }
            mask <<= 1; // ���� �ڸ� ���� �� ����
        }
    }

    /// <summary>
    /// ������ üũ �Լ�
    /// </summary>
    private void CheckHorizontal()
    {
        // ���� üũ
        for(int y = 0; y < count_y; y++)
        {
            int count = 0; // y���� x��� ����
            for(int x = 0; x < count_x; x++)
            {
                if (!cells[y,x].CheckVaild())
                {
                    count++;
                }
            }

            // �ش� �ٿ� ��� ����� �����ϸ� ���� (1�� ����)
            if (count >= count_x)
            {
                for(int i = 0; i < count_x; i++)
                {
                    cells[y, i].RemoveBlockObject();
                    DownOneBlock(i, y);
                }
            }
        }
    }

    /// <summary>
    /// ��� �� ĭ ������
    /// </summary>
    private void DownOneBlock(int gridX, int gridY)
    {
        int checkHeight = gridY + 1;
        while(checkHeight < count_y)
        {
            if (!cells[checkHeight, gridX].CheckVaild()) // �ش� ��ġ�� ����� ������ ����
            {
                GameObject upperObject = cells[checkHeight, gridX].GetBlockObject();
                upperObject.transform.localPosition = new Vector2(upperObject.transform.localPosition.x, upperObject.transform.localPosition.y - 0.25f); // ��ġ ����
                cells[checkHeight - 1, gridX].SetBlockObject(upperObject);  // �� �缳��
                cells[checkHeight, gridX].RemoveBlockObject(true);          // ������ �ִ� �� ���� ����
            }

            checkHeight++;
        }
    }

    // ��ǥ ��ȯ ===================================================================================

    public Vector2Int WorldToGrid(Vector2 world)
    {
        return new Vector2Int(Mathf.CeilToInt(world.x / 0.25f + 0.01f) - 1, Mathf.CeilToInt(world.y / 0.25f + 0.01f) - 1); // 2D ������Ʈ�̿��� ������ x y�� ���
    }

    public Vector2 GridToWorld(Vector2Int grid)
    {
        return new Vector2(grid.x * 0.25f, grid.y * 0.25f);
    }
}