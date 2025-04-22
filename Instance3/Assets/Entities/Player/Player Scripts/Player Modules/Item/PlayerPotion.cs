using System;
using UnityEngine;

public class PlayerPotion : ItemModule
{
    PlayerController player;
    Stats stats;
    public int nbPotions;
    [SerializeField] private int nbPotionsMax;
    [SerializeField] private int healValue;
    public static Action onRecharge { get; set; }

    private void Start()
    {
        Recharge();
    }

    protected override void Use()
    {
        if (stats.health == stats.healthMax)
            return;

        if (nbPotions <= 0) 
            return;

        nbPotions--;

        player.Healing(healValue);
        UpdateUi();
    }

    private void UpdateUi()
    {
        DisplayPotions.onShow?.Invoke();
        DisplayPotions.onPotionDisplay?.Invoke(this);
        DisplayPotions.onUpdate?.Invoke();
    }

    private void OnEnable()
    {
        if (stats == null) 
            stats = GetComponent<Stats>();

        if (player == null) 
            player = GetComponent<PlayerController>();

        onRecharge += Recharge;

        PlayerController.onUsePotion += Use;
        DisplayPotions.onShow?.Invoke();

        UpdateUi();
    }

    private void OnDisable()
    {
        PlayerController.onUsePotion -= Use;
        DisplayPotions.onHide?.Invoke();
    }

    private void Recharge()
    {
        if (PlayerPrefs.HasKey(ItemsName.Potion.ToString())) 
            nbPotionsMax = PlayerPrefs.GetInt(ItemsName.Potion.ToString());
        else
        {
            PlayerPrefs.SetInt(ItemsName.Potion.ToString(), nbPotionsMax);
            PlayerPrefs.Save();
        }

        nbPotions = nbPotionsMax;
        UpdateUi();
    }

    public int GetMaxPotion()
    {
        nbPotionsMax = PlayerPrefs.GetInt(ItemsName.Potion.ToString(), nbPotionsMax);
        return nbPotionsMax;
    }
}
