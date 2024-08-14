using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private GameObject currnetBlockObject;

    private int x;
    private int y;
    private bool isVaild = true;

    /// <summary>
    /// Cell �ʱ�ȭ 
    /// </summary>
    /// <param name="x">x ��ǥ</param>
    /// <param name="y">y ��ǥ</param>
    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// �ش� ��ġ ������Ʈ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="obj">�ش� ��ġ�� �ִ� ������Ʈ</param>
    public void SetBlockObject(GameObject obj)
    {
        currnetBlockObject = obj;
    }

    /// <summary>
    /// �������� �ʴ� Cell�� ���� �ϴ� �Լ� 
    /// </summary>
    public void SetUnVaild()
    {
        isVaild = false;
    }

    /// <summary>
    /// �ش� ��ġ�� ��ȿ�� Cell���� Ȯ���ϴ� �Լ� 
    /// </summary>
    /// <returns>�����ϸ� true �ƴϸ� false</returns>
    public bool CheckVaild()
    {
        return isVaild;
    }
}