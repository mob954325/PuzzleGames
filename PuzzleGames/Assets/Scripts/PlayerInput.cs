using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInput : MonoBehaviour
{
    // ��Ʈ���� ����
    // asd = ��� ���� (�� ������ ��� �̵�)
    // space(doubleTap) = ��� ���

    PlayerInputAction playerInputAction;

    /// <summary>
    /// ��ǲ ����
    /// </summary>
    private Vector2 inputVec = Vector2.zero;

    public bool allowInput = false;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        playerInputAction.Enable();

        playerInputAction.Player.Move.started += OnBlockMoveStart;
        playerInputAction.Player.Move.performed += OnBlockMove;
        playerInputAction.Player.Move.canceled += OnBlockMove;

        playerInputAction.Player.Drop.performed += OnDrop;
        playerInputAction.Player.Drop.canceled += OnDrop;
    }


    private void OnDisable()
    {
        playerInputAction.Player.Move.started -= OnBlockMoveStart;
        playerInputAction.Player.Move.performed -= OnBlockMove;
        playerInputAction.Player.Move.canceled -= OnBlockMove;

        playerInputAction.Player.Drop.performed -= OnDrop;
        playerInputAction.Player.Drop.canceled -= OnDrop;

        playerInputAction.Disable();
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
    }

    private void OnBlockMoveStart(InputAction.CallbackContext context)
    {

    }

    private void OnBlockMove(InputAction.CallbackContext context)
    {
        inputVec = context.ReadValue<Vector2>();
    }

    private void OnBlockMoveEnd(InputAction.CallbackContext context)
    {
    }

    /// <summary>
    /// ��ǲ ���� ��ȯ �Լ�
    /// </summary>
    public Vector2 GetInputVec()
    {
        return inputVec;
    }
}