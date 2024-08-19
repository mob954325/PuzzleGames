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
        isVaild = false;
    }

    /// <summary>
    /// �ش� ���� ������ �����ϴ� �Լ� (��� ����)
    /// </summary>
    /// <param name="activeSelf">��� ������Ʈ Ȱ��ȭ ���� (default : false)</param>
    public void RemoveBlockObject(bool activeSelf = false)
    {
        currnetBlockObject.SetActive(activeSelf);
        currnetBlockObject = null;
        isVaild = true;
    }

    public GameObject GetBlockObject()
    {
        return currnetBlockObject;
    }

    /// <summary>
    /// SetVaild ���� �Լ�
    /// </summary>
    public void SetVaild(bool value)
    {
        isVaild = value;
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