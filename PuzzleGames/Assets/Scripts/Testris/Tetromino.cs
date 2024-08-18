using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShapeType
{
    None,
    O,
    I,
    L,
    T,
    S
}

public class Tetromino : MonoBehaviour
{
    public ShapeType type = ShapeType.None;

    private ShapeType Type
    {
        get => type;
        set
        {
            type = value;
            if (type != ShapeType.None)
            {
                MakeShape();
            }
        }
    }
    /// <summary>
    /// ��Ʈ���� ��� �迭
    /// </summary>
    private GameObject[] blocks;

    /// <summary>
    /// ��� ��� ������ �׸��� ��ġ ���� �迭 (4x4)
    /// </summary>
    private Vector2[] gridPositions;

    /// <summary>
    /// �߽� �� ����
    /// </summary>
    private Vector2 centerVector = new Vector2(0.125f, 0.125f);

    /// <summary>
    /// ���� ��ġ ����
    /// </summary>
    public Vector2 prevVector = Vector2.zero;

    /// <summary>
    /// �������� �� ũ�� (0.25 == �� ĭ), ��� ������Ʈ�� ũ��
    /// </summary>
    private const float DropScale = 0.25f;

    /// <summary>
    /// ���� ĭ���� �������µ� �ɸ��� �ð� Ÿ�̸� (�ð��� �Ǹ� �ʱ�ȭ �� ��ġ����)
    /// </summary>
    private float dropTimer = 0f;

    /// <summary>
    /// ����� �������� �� Ȱ��ȭ�Ǵ� Ÿ�̸� (���� �ð��� ������ ��� ����)
    /// </summary>
    private float stopTimer = 1f;

    /// <summary>
    /// ������ �� �ִ��� üũ�ϴ� ���� (������ �� ������ true �ƴϸ� false)
    /// </summary>
    public bool allowMove = false;

    private void Awake()
    {
        blocks = new GameObject[4];
        for(int i = 0; i < blocks.Length; i++)
        {
            Transform child = transform.GetChild(i);
            blocks[i] = child.gameObject;
        }

        gridPositions = new Vector2[16];
        for(int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            { 
                gridPositions[x + 4 * y] = new Vector2(-0.25f + x * 0.25f, -0.25f + y * 0.25f) + centerVector;
            }
        }
    }

    private void FixedUpdate()
    {
        dropTimer += Time.fixedDeltaTime;

        if((Vector3)prevVector != transform.localPosition)
        {
            stopTimer = 1f; // 1�� ���
        }

        if (allowMove && dropTimer > 0.25f)
        {
            dropTimer = 0f;
            prevVector = transform.localPosition;
            transform.Translate(Vector2.down * DropScale);
        }

        CheckIsStop();
    }

    /// <summary>
    /// ����� ������� Ȯ���ϴ� �Լ� (����������Ʈ �Լ�)
    /// </summary>
    private void CheckIsStop()
    {
        stopTimer -= Time.fixedDeltaTime;

        if(stopTimer < 0f)
        {
            allowMove = false;
        }
    }

    /// <summary>
    /// ��Ʈ���� ��� �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="type">��� Ÿ��</param>
    /// <param name="width">���� ����</param>
    /// <param name="height">���� ����</param>
    public void Init(ShapeType type)
    {
        Type = type;
        allowMove = true;
        SetRandomColor();
    }
    
    /// <summary>
    /// ��� ����� �Լ�
    /// </summary>
    private void MakeShape()
    {
        switch (Type) 
        { 
            case ShapeType.O: // 5 6 9 10
                SetBlockPosition(5, 6, 9, 10);
                break;
            case ShapeType.I: // 1 5 9 13
                SetBlockPosition(1, 5, 9, 13);
                break;
            case ShapeType.L: // 5 6 9 13
                SetBlockPosition(5, 6, 9, 13);
                break;
            case ShapeType.T: // 4 5 6 9
                SetBlockPosition(4, 5, 6, 9);
                break;
            case ShapeType.S: // 4 5 9 10
                SetBlockPosition(4, 5, 9, 10);
                break;
        }
    }

    /// <summary>
    /// ��� ��ġ�� �����ϴ� �Լ� (gridPositions �迭 ���)
    /// </summary>
    /// <param name="first">ù��° ��ġ</param>
    /// <param name="second">�ι�° ��ġ</param>
    /// <param name="third">����° ��ġ</param>
    /// <param name="fourth">�׹�° ��ġ</param>
    private void SetBlockPosition(int first, int second, int third, int fourth)
    {
        int[] ints = { first, second, third, fourth };
        int index = 0;

        foreach (var obj in blocks)
        {
            obj.transform.localPosition = gridPositions[ints[index]];
            index++;
        }
    }

    /// <summary>
    /// ������ ���� ���� �Լ� (true : ������ ����, false : �������� ����)
    /// </summary>
    /// <param name="value">true ���� �ο�, false ���� ����</param>
    public void SetMoveAllow(bool value)
    {
        allowMove = value;
    }

    /// <summary>
    /// allowMove ��ȯ �Լ� (������ ���� ��ȯ �Լ�)
    /// </summary>
    public bool checkMoveAllow()
    {
        return allowMove;
    }

    /// <summary>
    /// ��� ���� ���� ���� �Լ�
    /// </summary>
    private void SetRandomColor()
    {
        float rand = Random.value;
        foreach(var obj in blocks)
        {
            Material material = obj.GetComponent<SpriteRenderer>().material;
            material.color = new Color(rand, rand, rand);
        }
    }

    /// <summary>
    /// ������Ʈ �����̴� �Լ�
    /// </summary>
    /// <param name="inputVec">��ǲ ��</param>
    public void MoveObjet(Vector2 inputVec)
    {
        // ����� ���� �ö󰡴� �� ����
        if(inputVec.y > 0)
        {
            inputVec.y = 0;
        }

        // ��� �����̱� (��ĭ)
        prevVector = transform.localPosition;
        transform.Translate(inputVec * 0.25f);
    }

    /// <summary>
    /// ��Ʈ���� ����� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public GameObject[] GetBlocks()
    {
        return blocks;
    }

    /// <summary>
    /// ������Ʈ ȸ�� �Լ�
    /// </summary>
    public void RotateObject()
    {
        transform.Rotate(Vector3.forward * 90f);
    }

    /// <summary>
    /// ������Ʈ ��� �Լ�
    /// </summary>
    /// <param name="dropPosition">������ ��ġ
    /// </param>
    public void DropObject(Vector2 dropPosition)
    {
        transform.localPosition = dropPosition;
    }
}