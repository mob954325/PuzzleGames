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

    Player player;
    PlayerInputAction playerInputAction;

    /// <summary>
    /// ��ǲ ����
    /// </summary>
    private Vector2 inputVec = Vector2.zero;

    /// <summary>
    /// ��ǲ ������ �ִ��� Ȯ�� ����
    /// </summary>
    public bool allowInput = false;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        playerInputAction.Enable();

        playerInputAction.Player.Move.performed += OnBlockMove;
        playerInputAction.Player.Move.canceled += OnBlockMove;

        playerInputAction.Player.Drop.performed += OnDrop;
        playerInputAction.Player.Drop.canceled += OnDrop;
    }


    private void OnDisable()
    {
        playerInputAction.Player.Move.performed -= OnBlockMove;
        playerInputAction.Player.Move.canceled -= OnBlockMove;

        playerInputAction.Player.Drop.performed -= OnDrop;
        playerInputAction.Player.Drop.canceled -= OnDrop;

        playerInputAction.Disable();
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
        player.OnSpace?.Invoke();
    }

    private void OnBlockMove(InputAction.CallbackContext context)
    {
        inputVec = context.ReadValue<Vector2>();
        player.GetPlayerTetromino().MoveObjet(inputVec);
    }
}