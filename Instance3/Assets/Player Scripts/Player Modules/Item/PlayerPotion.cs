using System;
using UnityEngine;

public class PlayerPotion : ItemModule
{
    PlayerController player;
    Stats stats;
    public int nbPotions;
    [SerializeField] private int nbPotionsMax;
    [SerializeField] private int healValue;
    private void Awake() 
    {
        nbPotions = nbPotionsMax;
        UpdateUi();
    }
    protected override void Use()
    {
        if (stats.health == stats.healthMax)
        {
            Debug.Log("Already full life");
            return;
        }

        nbPotions--;
        if (nbPotions < 0) nbPotions = 0;

        player.Healing(healValue);
        UpdateUi();
    }

    private void UpdateUi()
    {
        DisplayPotions.onPotionDisplay?.Invoke(this);
        DisplayHealth.onUpdate?.Invoke();
        DisplayPotions.onUpdate?.Invoke();
    }

    void OnEnable()
    {
        if (stats == null) stats = GetComponent<Stats>();
        if (player == null) player = GetComponent<PlayerController>();
        PlayerController.onUsePotion += Use;
        DisplayPotions.onShow?.Invoke();
        UpdateUi();
    }

    void OnDisable()
    {
        PlayerController.onUsePotion -= Use;
        DisplayPotions.onHide?.Invoke();
    }

}
