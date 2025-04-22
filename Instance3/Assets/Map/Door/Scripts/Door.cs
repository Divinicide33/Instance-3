using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    
    [SerializeField] private RoomId nextRoom; // Salle de destination pour cette porte
    public RoomId NextRoom => nextRoom;
    [SerializeField] private string selectedDoorName; // Nom de la porte sélectionnée dans la scène cible (renseigné via l'éditeur personnalisé)
    [SerializeField] private Vector3 targetPosition; // Position cible de la porte (renseignée par le DoorEditor)
    [SerializeField] protected SpriteRenderer sprite;
    protected string refSave = $"";
    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }

    private void Awake()
    {
        refSave = $"{SceneManager.GetSceneAt(1).name} is {GetType().Name}";
    }

    protected virtual void OnEnable()
    {
        TryGetComponent(out sprite);
        
        DisableSprite();
    }


    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out _))
            return;

        if (!other.gameObject.transform.parent.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            if (controller.IsDead)
                return;
            
            PlayerState.onInvincible?.Invoke();
            return;
        }
        
        EnterDoor(other.transform.parent);
    }

    protected virtual void EnterDoor(Transform player)
    {
        //Debug.Log($"Position cible définie (targetPosition) : {targetPosition}");
        PlayerController.onSaveDoor?.Invoke(new DoorData(nextRoom, targetPosition));
        AudioManager.OnStopAllSFX?.Invoke();
        RoomManager.Instance.ChangeRoomWithFade(nextRoom, player, targetPosition);
        
        
        
        // Vous pouvez ajouter d'autres actions après la transition, par exemple révéler la mini-map
        // MiniMapRoomManager.instance.RevealRoom();
    }

    protected virtual void DisableSprite()
    {
        if (sprite != null)
        {
            Debug.Log($"Disable Sprite : {GetType().Name}");
            sprite.enabled = false;
        }
    }
    
    protected virtual void EnableSprite()
    {
        if (sprite != null)
        {
            Debug.Log($"Enable Sprite : {GetType().Name}");
            sprite.enabled = true;
        }
    }
    
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(targetPosition, 0.2f);
        Gizmos.DrawLine(transform.position, targetPosition);
    }
}