using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputScript : MonoBehaviour
{
    bool isMoving = false;
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
            Debug.Log("Jump");
            PlayerController.onJump?.Invoke(true);
            PlayerController.onGlide?.Invoke(true);
        }
        else if (context.canceled)
        {
            PlayerController.onJump?.Invoke(false);
            PlayerController.onGlide?.Invoke(false);
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerController.onDash?.Invoke();
        }
    }
}
