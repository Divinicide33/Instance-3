using System;
using UnityEngine;

public class PlayerGlide : SkillModule
{
    private Rigidbody2D rb;
    [SerializeField] private float maxVelocityY;
    private bool isGliding = false;
    private bool canGlide = true;
    public static Action<bool> onCanGlide {get; set;}

    private void OnEnable() 
    {
        if (rb == null) 
            rb = GetComponent<Rigidbody2D>();
        
        PlayerController.onGlide += Glide;
        onCanGlide += SetCanGlide;
    }

    private void OnDisable() 
    {
        PlayerController.onGlide -= Glide;
        onCanGlide -= SetCanGlide;
    }
    
    private void SetCanGlide(bool value)
    {
        canGlide = value;
    }

    private void Update() 
    {
        if (!isGliding || !canGlide) 
            return;

        if (rb.linearVelocityY < -maxVelocityY) 
            rb.linearVelocityY = -maxVelocityY;
    }

    /// <summary>
    ///  if you are falling too fast while gliding , you are caped to the max velocity
    /// </summary>
    /// <param name="value"></param>
    public void Glide(bool value) 
    {
        isGliding = value;
    }
}
