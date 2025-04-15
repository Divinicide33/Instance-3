using AI.Hermes;
using System;
using UnityEngine;

public class ArrowForHermesFX : FX<ArrowForHermesFX>
{
    [SerializeField] Transform player;

    [SerializeField] int directions = 1;
    [SerializeField] GameObject arrow;
    public SpriteRenderer arrowDown;
    public SpriteRenderer arrowUp;
    public bool isActivate = false;
    public static Action<bool> onLockArrow { get; set; }

    public void OnEnable()
    {
        onLockArrow += LockOn;
    }

    public void OnDisable()
    {
        onLockArrow -= LockOn;
    }
    void Update()
    {
        //if (isActivate)
        //{
        //    RotateArrow();
        //}
        //else
        //{
        //    arrow.gameObject.SetActive(false);
        //}


    }

    public void RotateArrow()
    {
        arrow.gameObject.SetActive(true);
        Vector2 direction = player.position;
        float angle = Mathf.Atan2(direction.y - transform.position.y, direction.x - transform.position.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 90 + angle);
    }

    public void LockOn(bool isLock)
    {
        if (isLock)
        {
            arrowDown.color = Color.red;
            arrowUp.color = Color.red;
            arrow.gameObject.SetActive(true);
        }
        else
        {
            arrowDown.color = Color.white;
            arrowUp.color = Color.white;
            arrow.gameObject.SetActive(false);
        }
    }

    protected override void EnableFX()
    {
    }

    protected override void DisableFX()
    {

    }
}
