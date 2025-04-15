using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    
    private void OnEnable() 
    {
        PlayerController.onAttack += Attack;
    }
    private void OnDisable() 
    {
        PlayerController.onAttack -= Attack;
    }

    void Attack()
    {
        PlayerSword.onSword?.Invoke();
    }
}
