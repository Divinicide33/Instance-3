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
            Player.onMove?.Invoke(context.ReadValue<Vector2>());
        }
        else
        {
            Player.onMove?.Invoke(Vector2.zero);
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Jump");
            Player.onJump?.Invoke(true);
            Player.onGlide?.Invoke(true);
        }
        else if (context.canceled)
        {
            Player.onJump?.Invoke(false);
            Player.onGlide?.Invoke(false);
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Player.onDash?.Invoke();
        }
    }
}
