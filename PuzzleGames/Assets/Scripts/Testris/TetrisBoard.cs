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
    PlayerInput player;

    /// <summary>
    /// ���� ��ġ Ʈ������
    /// </summary>
    private Transform spawnPoint;

    private Transform tetrominoContainer;

    /// <summary>
    /// ��Ʈ���� ��� ������
    /// </summary>
    public GameObject tetrominoPrefab;

    /// <summary>
    /// ���� �ֱٿ� ������ ��Ʈ���� ���
    /// </summary>
    public Tetromino currentTetromino;

    /// <summary>
    /// ���� �� �迭 (��Ʈ���� ��� ������Ʈ Ȯ�ο�)
    /// </summary>
    private Cell[] cells;

    /// <summary>
    /// ���� ����
    /// </summary>
    public float boardWidth;

    /// <summary>
    /// ���� ũ��
    /// </summary>
    public float boardHeight;

    private float inputTimer;

    private const float InputDelay = 0.1f;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerInput>();

        Transform child = transform.GetChild(0);

        boardWidth = child.GetChild(0).transform.localScale.x;
        boardHeight = child.GetChild(0).transform.localScale.y;

        child = transform.GetChild(1);
        tetrominoContainer = child.gameObject.transform;

        child = transform.GetChild(2);
        spawnPoint = child.gameObject.transform;
    }

    private void FixedUpdate()
    {
        inputTimer += Time.fixedDeltaTime;

        if (currentTetromino != null)
        {
            if(IsVaildPosition())
            {
                if(inputTimer > InputDelay)
                {
                    inputTimer = 0f;
                    currentTetromino.MoveObjet(player.GetInputVec());
                }
            }
            else
            {
                currentTetromino.SetMoveAllow(false);
            }
        }
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
        int count_x = (int)(boardWidth / 0.25f);
        int count_y = (int)(boardHeight / 0.25f);

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
        Tetromino tetromino = Instantiate(tetrominoPrefab).GetComponent<Tetromino>();
        tetromino.transform.parent = tetrominoContainer;
        tetromino.transform.localPosition = spawnPoint.transform.localPosition;

        tetromino.Init(type, boardWidth, boardHeight);

        currentTetromino = tetromino;
    }

    /// <summary>
    /// ���� �������� ��ϵ��� ���� ���� �ȿ� �ִ��� üũ�ϴ� �Լ�
    /// </summary>
    /// <returns>�ȿ� ������ true �ƴϸ� false</returns>
    private bool IsVaildPosition()
    {
        bool result = true;
        foreach(var obj in currentTetromino.GetBlocks())
        {
            Vector2 world = obj.transform.parent.localPosition + obj.transform.localPosition;
            Vector2Int grid = WorldToGrid(world);

            Debug.Log(grid.x);

            // ������ ���� �� ��
            if (grid.x < 1 || grid.x > boardWidth / 0.25f) // ���� �ϴ� (1,1) ����
            {
                result = false;
            }
            if(grid.y < 2 || grid.y > boardHeight / 0.25f + 5)
            {
                result = false;
            }
        }

        Debug.Log(result);
        return result;
    }

    // ��ǥ ��ȯ ===================================================================================
    public Vector2Int WorldToGrid(Vector2 world)
    {
        return new Vector2Int(Mathf.CeilToInt(world.x / 0.25f + 0.01f), Mathf.CeilToInt(world.y / 0.25f + 0.01f)); // 2D ������Ʈ�̿��� ������ x y�� ���
    }
}