using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputScript : MonoBehaviour
{
    private bool isMoving = false;
    private bool isEnable = true;
    private bool isDead = false;
    public static Action onEnableInput { get; set; }
    public static Action onDisableInput { get; set; }
    public static Action<bool> onIsPlayerDead { get; set; }

    private void OnEnable()
    {
        onEnableInput += EnableInput;
        onDisableInput += DisableInput;
        onIsPlayerDead += IsPlayerDead;
    }

    private void OnDisable()
    {
        onEnableInput -= EnableInput;
        onDisableInput -= DisableInput;
        onIsPlayerDead -= IsPlayerDead;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isMoving = true;
        }
        else if (context.canceled)
        {
            isMoving = false;
        }

        if (isMoving)
        {
            PlayerController.onMove?.Invoke(context.ReadValue<Vector2>());
        }
        else
        {
            PlayerController.onMove?.Invoke(Vector2.zero);
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerController.onGlide?.Invoke(true);
            if (!isEnable) return;
            PlayerController.onJump?.Invoke(true);
        }
        else if (context.canceled)
        {
            PlayerController.onGlide?.Invoke(false);
            if (!isEnable) return;
            PlayerController.onJump?.Invoke(false);
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!isEnable) return;
        if (context.started)
        {
            PlayerController.onDash?.Invoke();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!isEnable) return;
        if (context.started)
        {
            PlayerController.onAttack?.Invoke();
        }
    }

    public void OnUsePotion(InputAction.CallbackContext context)
    {
        if (!isEnable) return;
        if (context.started)
        {
            PlayerController.onUsePotion?.Invoke();
        }
    }

    private void EnableInput()
    {
        if (isDead) return;

        isEnable = true;
        PlayerMove.onSetMove?.Invoke(true);
        PlayerGlide.onCanGlide?.Invoke(true);
        //Debug.Log("Input enabled");
    }

    private void DisableInput()
    {
        isEnable = false;
        PlayerDash.onStopDash?.Invoke();
        PlayerAttack.onStopAction?.Invoke();
        PlayerMove.onSetMove?.Invoke(false);
        PlayerGlide.onCanGlide?.Invoke(false);
        //Debug.Log("Input disabled");
    }

    private void IsPlayerDead(bool value)
    {
        isDead = value;

        if (isDead) DisableInput();
        else EnableInput();
    }

    public void StartPause(InputAction.CallbackContext context)
    {
        if (context.started)
            PauseMenu.onStartPause?.Invoke();
    }

    public void HandleCancel(InputAction.CallbackContext context)
    {
        if (context.started)
            PauseMenu.onCancel?.Invoke();
    }
}
