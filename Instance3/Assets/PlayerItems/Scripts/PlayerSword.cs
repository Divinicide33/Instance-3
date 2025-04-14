using System;
using UnityEngine;

public class PlayerSword : ItemModule
{
    Stats stats;
    private void Awake() 
    {
        stats = GetComponent<Stats>();
    }
    protected override void Use()
    {
        // 

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Use();
        }
    }

}
