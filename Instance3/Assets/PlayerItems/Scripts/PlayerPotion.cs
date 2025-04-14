using System;
using UnityEngine;

public class PlayerPotion : ItemModule
{
    Stats stats;
    public int nbPotions;
    [SerializeField] private int nbPotionsMax;
    [SerializeField] private int healValue;
    private void Awake() 
    {
        stats = GetComponent<Stats>();
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

        stats.health += healValue;
        if (stats.healthMax < stats.health) stats.health = stats.healthMax;

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
        DisplayPotions.onShow?.Invoke();
    }

    void OnDisable()
    {
        DisplayPotions.onHide?.Invoke();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Use();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            UiManager.HideAllUI();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            UiManager.ShowAllUI();
        }
    }

}
