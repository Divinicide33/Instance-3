using System;
using TMPro;
using UnityEngine;

public class DisplayPotions : UIElement
{
    [SerializeField] private Transform PotionsPanel;
    [SerializeField] TMP_Text nbPotions;
    PlayerPotion playerPotion;
    public static Action<PlayerPotion> onPotionDisplay { get; set; }
    protected override void Show()
    {
        PotionsPanel.gameObject.SetActive(true);
    }
    protected override void Hide()
    {
        PotionsPanel.gameObject.SetActive(false);
    }
    protected override void UpdateDisplay()
    {
        UpdateImage();
    }

    private void UpdateImage()
    {
        if (playerPotion == null)
        {
            Debug.Log($"playerPotion is null");
            return;
        }

        nbPotions.text = "X " + playerPotion.nbPotions.ToString();
    }

    private void GetPotionScript(PlayerPotion script)
    {
        playerPotion = script;
    }
    protected override void OnEnable() 
    {
        base.OnEnable();
        onPotionDisplay += GetPotionScript;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        onPotionDisplay -= GetPotionScript;
    }
}
