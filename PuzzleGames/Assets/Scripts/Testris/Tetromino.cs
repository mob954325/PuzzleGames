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
            if (type != value)
            {
                type = value;
                MakeShape();
            }
        }
    }

    /// <summary>
    /// ��Ʈ���� ��� �迭
    /// </summary>
    private GameObject[] blocks;

    /// <summary>
    /// ��輱 üũ�ϴ� ���� ������Ʈ
    /// </summary>
    public GameObject checkObject;

    /// <summary>
    /// �߽� �� ����
    /// </summary>
    private Vector2 centerVector = new Vector2(0.125f, 0.125f);

    /// <summary>
    /// �������� �� ũ�� (0.25 == �� ĭ)
    /// </summary>
    private const float DropScale = 0.25f;

    /// <summary>
    /// ���� ����
    /// </summary>
    private float boardWidth;

    /// <summary>
    /// ���� ����
    /// </summary>
    private float boardHeight;

    /// <summary>
    /// ���� ĭ���� �������µ� �ɸ��� �ð� Ÿ�̸� (�ð��� �Ǹ� �ʱ�ȭ �� ��ġ����)
    /// </summary>
    private float dropTimer = 0f;

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

        checkObject = blocks[0]; // ���� ������Ʈ ���� ����
    }

    private void FixedUpdate()
    {
        dropTimer += Time.fixedDeltaTime;

        if (allowMove && IsVaildPosition() && dropTimer > 1f)
        {
            dropTimer = 0f;
            transform.Translate(Vector2.down * DropScale);
        }
    }

    /// <summary>
    /// ��Ʈ���� ��� �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="type">��� Ÿ��</param>
    /// <param name="width">���� ����</param>
    /// <param name="height">���� ����</param>
    public void Init(ShapeType type, float width, float height)
    {
        Type = type;
        boardHeight = height;
        boardWidth = width;

        allowMove = true;
    }
    
    /// <summary>
    /// ��� ����� �Լ�
    /// </summary>
    private void MakeShape()
    {
        switch(Type) 
        { 
            case ShapeType.O:
                break;
            case ShapeType.I:
                for(int i = 0; i < blocks.Length; i++)
                {
                    blocks[i].transform.localPosition = centerVector - (Vector2.up * 0.25f) + (Vector2.up * 0.25f) * i;

                    if(blocks[i].transform.localPosition.y < checkObject.transform.localPosition.y)
                    {
                        checkObject = blocks[i];
                    }
                }
                break;
            case ShapeType.L: 
                break;
            case ShapeType.T:
                break;
            case ShapeType.S:
                break;
        }
    }

    /// <summary>
    /// ����� ������ �� �ִ� �������� Ȯ���ϴ� �Լ� (ī�޶� �� = ���簡��, �� �� = false)
    /// </summary>
    /// <returns></returns>
    private bool IsVaildPosition()
    {
        Vector2 currnetPosition = transform.localPosition + (checkObject.transform.localPosition * 2);
        return (currnetPosition.x < boardWidth
                && currnetPosition.x > 0 
                && currnetPosition.y < boardHeight + 2f
                && currnetPosition.y > 0);
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
    /// ������Ʈ �����̴� �Լ�
    /// </summary>
    /// <param name="inputVec">��ǲ ��</param>
    public void MoveObjet(Vector2 inputVec)
    {
        //if (!IsVaildPosition()) return;

        transform.Translate(inputVec * 0.25f);
    }

    /// <summary>
    /// ������Ʈ ȸ�� �Լ�
    /// </summary>
    public void RotateObject()
    {

    }
}
