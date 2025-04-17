using System;
using UnityEngine;
using Fountain;

[RequireComponent(typeof(Stats))]
public class PlayerController : Entity
{
    [HideInInspector] public Stats stats;

    public static Action<Vector2> onMove { get; set; }
    public static Action<bool> onJump { get; set; }
    public static Action onAttack { get; set; }
    public static Action onDash { get; set; }
    public static Action<bool> onGlide { get; set; }
    public static Action onUsePotion { get; set; }
    public static Action onEndOfInvincibility { get; set; }
    public static Action<FountainData> onSavefountain { get; set; }
    

    [HideInInspector] public bool isFacingRight = true;
    [HideInInspector] public bool isFacingUp = true;
    private bool isInvincible = false;

    private FountainData lastFountainSaved;


    void Start()
    {
        stats = GetComponent<Stats>();

        UpdatePlayerUi();
        LoadSavedFountain();
    }

    private void OnEnable() 
    {
        onEndOfInvincibility += EndOfInvincibility;
        onSavefountain += SaveFountain;
    }

    private void OnDisable() 
    {
        onEndOfInvincibility -= EndOfInvincibility;
        onSavefountain -= SaveFountain;
    }

    void EndOfInvincibility()
    {
        isInvincible = false;
    }

    void UpdatePlayerUi()
    {
        DisplayHealth.onUpdateHpMax?.Invoke(stats);
    }

    public override void TakeDamage(int damage, Vector3 originPosOfDamage, float power)
    {
        if (isInvincible || isDead) return;

        isInvincible = true; // to only get hit once

        base.TakeDamage(damage, originPosOfDamage, power); // to make him take damage et do the defeat if he die

        DisplayHealth.onUpdate?.Invoke(); // update the Ui
        
        PlayerState.onInvincible?.Invoke();
        PlayerState.onKnockBack?.Invoke(originPosOfDamage, power);
    }
    

    public override void Healing(int heal)
    {
        base.Healing(heal);
        DisplayHealth.onUpdate?.Invoke();
    }

    public override void Defeat()
    {
        base.Defeat();
        PlayerInputScript.onIsPlayerDead?.Invoke(true);

        if (lastFountainSaved == null)
            return;
        
        PlayerMove.onResetVelocity?.Invoke();
        PlayerPotion.onRecharge?.Invoke();
        stats.SetHpToHpMax();
        RoomManager.Instance.ChangeRoom(lastFountainSaved.room, transform,lastFountainSaved.position);
    }

    private void SaveFountain(FountainData newValue)
    {
        lastFountainSaved = newValue;

        PlayerPrefs.SetString("FountainRoom", newValue.room.ToString());
        PlayerPrefs.SetFloat("FountainPosX", newValue.position.x);
        PlayerPrefs.SetFloat("FountainPosY", newValue.position.y);
        PlayerPrefs.SetFloat("FountainPosZ", newValue.position.z);
    }
    
    private void LoadSavedFountain()
    {
        if (PlayerPrefs.HasKey("LastFountain"))
        {
            string json = PlayerPrefs.GetString("LastFountain");
            FountainData loaded = JsonUtility.FromJson<FountainData>(json);
            lastFountainSaved = loaded;
        }
        
        if (PlayerPrefs.HasKey("FountainRoom"))
        {
            string roomStr = PlayerPrefs.GetString("FountainRoom");
            RoomId room = (RoomId)Enum.Parse(typeof(RoomId), roomStr);

            float x = PlayerPrefs.GetFloat("FountainPosX");
            float y = PlayerPrefs.GetFloat("FountainPosY");
            float z = PlayerPrefs.GetFloat("FountainPosZ");

            lastFountainSaved = new FountainData(room, new Vector3(x, y, z));
        }
    }

}