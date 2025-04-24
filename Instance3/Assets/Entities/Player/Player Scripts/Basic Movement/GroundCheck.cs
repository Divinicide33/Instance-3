using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [HideInInspector] public static Action<bool> onGrounded;

    [Header("GroundCheck")]
    [SerializeField] private float offsetY;
    [SerializeField] private Vector2 size;
    [SerializeField] private LayerMask groundLayer;
    private Vector3 pos;
    private bool isGrounded = false;
    //private MoveFX moveFX;

    private void Start()
    {
        //moveFX = GetComponentInChildren<MoveFX>();
    }

    void Update()
    {
        Check();
    }
    private void Check()
    {
        pos = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
        Collider2D[] groundCheck = Physics2D.OverlapBoxAll(pos, size, 0, groundLayer);
        bool check = groundCheck.Length > 0;

        if (check != isGrounded)
        {
            isGrounded = check;
            onGrounded?.Invoke(isGrounded);
            //Debug.Log($"isGrounded = {isGrounded}");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos, size);
    }
}
