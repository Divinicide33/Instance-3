using System;
using UnityEngine;

public class PlayerPotion : ItemModule
{
    private PlayerController player;
    public int nbPotions;
    [SerializeField] private int nbPotionsMax;
    [SerializeField] private int healValue;
    private bool isGrounded = false;
    public static Action onRecharge { get; set; }
    private bool isUsingPotion = false;
    [SerializeField] private float useDuration = 1f;
    private float timer = 0;

    private void Start()
    {
        Recharge();
    }

    void Update()
    {
        if(isUsingPotion)
        {
            timer += Time.deltaTime;

            if (timer >= useDuration)
            {
                player.Healing(healValue);
                UpdateUi();
                StopPotion(true);
            }
        }
    }

    private void StopPotion(bool hasHealed)
    {
        isUsingPotion = false;
        timer = 0;
        
        if (hasHealed)
        {
            PlayerInputScript.onEnableInput?.Invoke();
        }
    }

    protected override void Use()
    {
        if (!isGrounded)
            return;

        if (player.stat.health == player.stat.healthMax)
            return;

        if (nbPotions <= 0) 
            return;

        nbPotions--;

        isUsingPotion = true;
        timer = 0;

        UpdateUi();

        PlayerInputScript.onDisableInput?.Invoke();
    }

    private void UpdateUi()
    {
        DisplayPotions.onShow?.Invoke();
        DisplayPotions.onPotionDisplay?.Invoke(this);
        DisplayPotions.onUpdate?.Invoke();
    }

    private void OnEnable()
    {
        if (player == null) 
            player = GetComponent<PlayerController>();

        onRecharge += Recharge;

        PlayerController.onUsePotion += Use;
        PlayerController.onPlayerHurt += StopPotion;
        DisplayPotions.onShow?.Invoke();
        GroundCheck.onGrounded += CheckIsGrounded;

        UpdateUi();
    }

    private void OnDisable()
    {
        PlayerController.onUsePotion -= Use;
        PlayerController.onPlayerHurt -= StopPotion;
        DisplayPotions.onHide?.Invoke();
        GroundCheck.onGrounded -= CheckIsGrounded;
    }

    private void CheckIsGrounded(bool value)
    {
        isGrounded = value;
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
