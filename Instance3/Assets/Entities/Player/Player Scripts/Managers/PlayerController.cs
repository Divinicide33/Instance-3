using System;
using UnityEngine;
using Fountain;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(PlayerInputScript))]
public class PlayerController : Entity
{
    public static Action<Vector2> onMove { get; set; }
    public static Action<bool> onJump { get; set; }
    public static Action onAttack { get; set; }
    public static Action onDash { get; set; }
    public static Action<bool> onGlide { get; set; }
    public static Action onUsePotion { get; set; }
    public static Action onEndOfInvincibility { get; set; }
    public static Action<FountainData> onSavefountain { get; set; }
    public static Action<DoorData> onSaveDoor { get; set; }
    public static Action<bool> onIsDead { get; set; }

    [HideInInspector] public bool isFacingRight = true;
    [HideInInspector] public bool isFacingUp = true;
    private bool isInvincible = false;

    private FountainData lastFountainSaved;
    private DoorData lastDoorUsed;
    public DoorData GetLastDoorUsed => lastDoorUsed;

    [Header("FX")]
    private PlayerHurtFX playerHurtFX;
    private string sfxHurtName = "PlayerHurt";

    void Start()
    {
        playerHurtFX = GetComponentInChildren<PlayerHurtFX>();

        PlayerInputScript.onDisableInput?.Invoke();

        UpdatePlayerUi();
        LoadSavedFountain();
        LoadSavedDoor();

        RoomManager.Instance.ChangeRoomWithFade(lastFountainSaved.room, transform, lastFountainSaved.position);
    }

    private void OnEnable() 
    {
        onEndOfInvincibility += EndOfInvincibility;
        onSavefountain += SaveFountain;
        onSaveDoor += SaveDoor;
        onIsDead += IsDead;
    }

    private void OnDisable() 
    {
        onEndOfInvincibility -= EndOfInvincibility;
        onSavefountain -= SaveFountain;
        onSaveDoor -= SaveDoor;
        onIsDead -= IsDead;
    }

    void EndOfInvincibility()
    {
        isInvincible = false;
    }

    void UpdatePlayerUi()
    {
        DisplayHealth.onUpdateHpMax?.Invoke(stat);
    }

    public override void TakeDamage(int damage, Vector3 originPosOfDamage, float power)
    {
        if (isInvincible || isDead) 
            return;

        isInvincible = true; // to only get hit once

        base.TakeDamage(damage, originPosOfDamage, power); // to make him take damage et do the defeat if he die

        DisplayHealth.onUpdate?.Invoke(); // update the Ui
        
        PlayerState.onInvincible?.Invoke();
        PlayerState.onKnockBack?.Invoke(originPosOfDamage, power);

        playerHurtFX?.ShowVFX();
        playerHurtFX?.ShowSFX(sfxHurtName);
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
        
        PlayerMove.onResetVelocity?.Invoke();
        PlayerPotion.onRecharge?.Invoke();

        stat.SetHpToHpMax();
        
        RoomManager.Instance.ChangeRoomWithFade(lastFountainSaved.room, transform,lastFountainSaved.position);
    }

    #region FountainSave
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
        if (PlayerPrefs.HasKey("FountainRoom"))
        {
            string roomStr = PlayerPrefs.GetString("FountainRoom");
            RoomId room = (RoomId)Enum.Parse(typeof(RoomId), roomStr);

            float x = PlayerPrefs.GetFloat("FountainPosX");
            float y = PlayerPrefs.GetFloat("FountainPosY");
            float z = PlayerPrefs.GetFloat("FountainPosZ");

            lastFountainSaved = new FountainData(room, new Vector3(x, y, z));
            return;
        }
        
        lastFountainSaved = new FountainData(RoomManager.Instance.rooms, transform.position);
        //Debug.LogWarning("🟡 Aucune fontaine sauvegardée trouvée. Position actuelle utilisée comme point de réapparition.");

    }
    #endregion
    
    #region DoorSave
    private void SaveDoor(DoorData newValue)
    {
        lastDoorUsed = newValue;

        PlayerPrefs.SetString("LastDoor", newValue.room.ToString());
        PlayerPrefs.SetFloat("DoorPosX", newValue.position.x);
        PlayerPrefs.SetFloat("DoorPosY", newValue.position.y);
        PlayerPrefs.SetFloat("DoorPosZ", newValue.position.z);
        //Debug.Log($"Room Name : {newValue.room.ToString()} -- Position : {newValue.position}");
        PlayerPrefs.Save();
    }
    
    private void LoadSavedDoor()
    {
        if (PlayerPrefs.HasKey("LastDoor"))
        {
            string roomStr = PlayerPrefs.GetString("LastDoor");
            RoomId room = (RoomId)System.Enum.Parse(typeof(RoomId), roomStr);

            float x = PlayerPrefs.GetFloat("DoorPosX");
            float y = PlayerPrefs.GetFloat("DoorPosY");
            float z = PlayerPrefs.GetFloat("DoorPosZ");

            lastDoorUsed = new DoorData(room, new Vector3(x, y, z));
            //Debug.Log("✅ Dernière porte chargée depuis les PlayerPrefs.");
            return;
        }

        lastDoorUsed = new DoorData(RoomManager.Instance.rooms, transform.position);
        //Debug.LogWarning("🟡 Aucune porte sauvegardée trouvée. Utilisation de la position actuelle.");

    }
    #endregion
    
    private void IsDead(bool value)
    {
        isDead = value;

        if (isDead)
        {
            PlayerInputScript.onDisableInput?.Invoke();
            return;
        }
        
        PlayerInputScript.onEnableInput?.Invoke();
    }
}