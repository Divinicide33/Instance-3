using System;
using UnityEngine;
using Fountain;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(PlayerInputScript))]
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
    public static Action<bool> onIsDead { get; set; }


    [HideInInspector] public bool isFacingRight = true;
    [HideInInspector] public bool isFacingUp = true;
    private bool isInvincible = false;

    private FountainData lastFountainSaved;

    [Header("FX")]
    private PlayerHurtFX playerHurtFX;

    void Start()
    {
        playerHurtFX = GetComponentInChildren<PlayerHurtFX>();

        stats = GetComponent<Stats>();
        PlayerInputScript.onDisableInput?.Invoke();

        UpdatePlayerUi();
        LoadSavedFountain();
    }

    private void OnEnable() 
    {
        onEndOfInvincibility += EndOfInvincibility;
        onSavefountain += SaveFountain;
        onIsDead += IsDead;
    }

    private void OnDisable() 
    {
        onEndOfInvincibility -= EndOfInvincibility;
        onSavefountain -= SaveFountain;
        onIsDead -= IsDead;
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
        
        // invoke ();
        PlayerState.onInvincible?.Invoke();
        PlayerState.onKnockBack?.Invoke(originPosOfDamage, power);

        playerHurtFX.ShowFX();
    }
    

    public override void Healing(int heal)
    {
        base.Healing(heal);
        DisplayHealth.onUpdate?.Invoke();
    }

    public override void Defeat()
    {
        base.Defeat();

        if (lastFountainSaved == null)
            return;
        
        PlayerMove.onResetVelocity?.Invoke(); // ne fonctionne pas
        PlayerPotion.onRecharge?.Invoke();
        stats.SetHpToHpMax();
        
        RoomManager.Instance.ChangeRoomWithFade(lastFountainSaved.room, transform,lastFountainSaved.position);
    }

    #region Fountain
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
        else
        {
            lastFountainSaved = new FountainData(RoomManager.Instance.rooms, transform.position);
            Debug.LogWarning("🟡 Aucune fontaine sauvegardée trouvée. Position actuelle utilisée comme point de réapparition.");
        }
    }
    #endregion
    
    
    private void IsDead(bool value)
    {
        isDead = value;

        if (isDead) PlayerInputScript.onDisableInput?.Invoke();
        else PlayerInputScript.onEnableInput?.Invoke();
    }
}