using UnityEngine;

public class PlayerGlide : SkillModule
{
    private Rigidbody2D rb;
    [SerializeField] private float maxVelocityY;
    private bool isGliding = false;
    private void OnEnable() 
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        PlayerController.onGlide += Glide;
    }
    private void OnDisable() 
    {
        PlayerController.onGlide -= Glide;
    }
    private void Update() 
    {
        if (!isGliding) return;
        if (rb.linearVelocityY < -maxVelocityY) rb.linearVelocityY = -maxVelocityY;
    }
    public void Glide(bool value) // if you are falling too fast while gliding , you are caped to the max velocity
    {
        isGliding = value;
    }
}
