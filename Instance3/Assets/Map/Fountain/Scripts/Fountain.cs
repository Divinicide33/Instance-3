using System;
using UnityEngine;

namespace Fountain
{
    public class Fountain : MonoBehaviour
    {
        [Header("Destination")] 
        [SerializeField] private FountainData fountain;
        
        [Header("Settings")]
        [SerializeField] private float inputReactivateDelay = 1f;
        
        public static Action onUseFountain { get; set; }
        
        private PlayerController playerControllerInZone = null;
        private bool isWaitingToActivateInput = false;
        private float timer = 0f;

        private void OnEnable()
        {
            PlayerController.onMove += OnPlayerMove;
        }

        private void OnDisable()
        {
            PlayerController.onMove -= OnPlayerMove;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Enemy>(out _))
                return;
            
            other.gameObject.transform.parent.TryGetComponent<PlayerController>(out PlayerController playerController);
            if (!playerController)
                return;

            playerControllerInZone = playerController;
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!playerControllerInZone) 
                return;
            
            if (other.gameObject.transform.parent != playerControllerInZone.transform)
                return;
            
            playerControllerInZone = null;
        }
        
        private void OnPlayerMove(Vector2 direction)
        {
            if (playerControllerInZone == null) return;

            if (direction.y > 0.5f && !isWaitingToActivateInput)
            {
                UseFountain();
            }
        }
        
        private void UseFountain() 
        {
            Debug.Log("Utilisation de la fontaine");
            FxPlayerHeal.onHeal?.Invoke(inputReactivateDelay);
            PlayerInputScript.onDisableInput?.Invoke();

            fountain.position = fountain.transform.position;
            PlayerController.onSavefountain?.Invoke(fountain);
            
            
            PlayerPotion.onRecharge?.Invoke();
            playerControllerInZone.stat.SetHpToHpMax();

            onUseFountain?.Invoke(); // sound ? fx utilisation de la fontaine
            ReactiveTimer();
        }
        
        private void ReactiveTimer()
        {
            timer = inputReactivateDelay;
            isWaitingToActivateInput = true;
        }
        
        private void Update()
        { 
            if (!isWaitingToActivateInput) return;

            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                //Debug.Log("Input reactivated");
                PlayerInputScript.onEnableInput?.Invoke();
                isWaitingToActivateInput = false;
            }
        }
    }
}