using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeToUnlock : ItemToUnlock
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        PlayerController.onUpdatePlayerMaxHealth?.Invoke();
    }
}
