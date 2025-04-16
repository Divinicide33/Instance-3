using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour 
{
    [SerializeField] private ModuleManager<SkillModule> playerSkills;
    [SerializeField] private ModuleManager<ItemModule> playerItems;

    public static Action<ItemsName,bool> onSetItem {get; set;}
    public static Action<SkillsName,bool> onSetSkill {get; set;}

    private void Awake()
    {
        playerSkills = new ModuleManager<SkillModule>(GetComponents<SkillModule>().ToList());
        playerItems = new ModuleManager<ItemModule>(GetComponents<ItemModule>().ToList());
    }

    private void Start() 
    {
        foreach (var skill in playerSkills.playerModules)
        {
            if (PlayerPrefs.HasKey(skill.ModuleName) && PlayerPrefs.GetInt(skill.ModuleName) == 1)
            {
                playerSkills.UnlockModule(skill.ModuleName);
            }
        }

        foreach (var item in playerItems.playerModules)
        {
            if (PlayerPrefs.HasKey(item.ModuleName) && PlayerPrefs.GetInt(item.ModuleName) == 1)
            {
                playerItems.UnlockModule(item.ModuleName);
            }
        }
    }

    void SetSkill(SkillsName name, bool value)
    {
        if (value)
        {
            playerSkills.UnlockModule(name.ToString());
        }
        else
        {
            playerSkills.LockModule(name.ToString());
        }
    }

    void SetItem(ItemsName name, bool value)
    {
        if (value)
        {
            playerItems.UnlockModule(name.ToString());
        }
        else
        {
            playerItems.LockModule(name.ToString());
        }
    }

    private void OnEnable() 
    {
        onSetItem += SetItem; 
        onSetSkill += SetSkill;
    }
    
    private void OnDisable() 
    {
        onSetItem -= SetItem; 
        onSetSkill -= SetSkill;
    }
}
