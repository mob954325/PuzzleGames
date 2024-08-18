using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// ���� �÷��̾ �����ϴ� ��Ʈ���� ���
    /// </summary>
    public Tetromino currentTetromino;

    /// <summary>
    /// �����̽��� ������ �� ȣ��Ǵ� ��������Ʈ
    /// </summary>
    public Action OnSpace;

    private void Awake()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    private void DropTetromino()
    {
        // ��� ���
        currentTetromino.DropObject(currentTetromino.transform.localPosition * Vector2.right); // ��ġ �ӽ� ����
    }

    /// <summary>
    /// �÷��̾ ���� �����ϰ� �ִ� ����� ��ȯ�ϴ� �Լ�
    /// </summary>
    public Tetromino GetPlayerTetromino()
    {
        return currentTetromino;
    }
}