using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour 
{
    [SerializeField] private ModuleManager<SkillModule> playerSkills;
    [SerializeField] private ModuleManager<ItemModule> playerItems;

    private void Awake()
    {
        playerSkills = new ModuleManager<SkillModule>(GetComponents<SkillModule>().ToList());
        playerItems = new ModuleManager<ItemModule>(GetComponents<ItemModule>().ToList());

        // playerSkills.UnlockModule(SkillsName.Dash.ToString());
        // playerSkills.GetUnlockedModule();

        // playerItems.LockModule(ItemsName.Potion.ToString());
        // playerItems.GetLockedModule();
    }



}
