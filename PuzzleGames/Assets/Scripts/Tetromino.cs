using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    /// <summary>
    /// �������� �� ũ�� (0.25 == �� ĭ)
    /// </summary>
    private const float DropScale = 0.25f;

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
        allowMove = !allowMove;
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
    /// ����� ������ �� �ִ� �������� Ȯ���ϴ� �Լ� (ī�޶� �� = ���簡��, �� �� = false)
    /// </summary>
    /// <returns></returns>
    private bool IsVaildPosition()
    {
        Vector2 currentPos = Camera.main.WorldToScreenPoint(transform.position);

        return (currentPos.x < Screen.width && currentPos.x > 0 && currentPos.y < Screen.height && currentPos.y > 0);
    }

    public void MoveObjet(Vector2 inputVec)
    {
        if (!IsVaildPosition()) return;

        transform.Translate(inputVec * 0.25f);
    }
}
