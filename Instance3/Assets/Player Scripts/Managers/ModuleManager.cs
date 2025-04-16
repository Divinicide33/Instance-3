using System.Collections.Generic;
using UnityEngine;

public class ModuleManager<T> where T : PlayerModule 
{
    public List<T> playerModules;

    public ModuleManager(List<T> Modules)
    {
        playerModules = Modules;
    }

    public bool HaveModule(string moduleName)
    {
        T module = playerModules.Find(module => module.ModuleName == moduleName);
        
        if (module != null && module.enabled) return true;
        else if (module != null && !module.enabled) return false;
        else
        {
            Debug.Log("Name invalid");
            return false;
        }
    }

    public List<string> GetUnlockedModule()
    {
        List<string> possessedModule = new List<string>();

        foreach (T module in playerModules)
        {
            if (module.enabled)
            {
                possessedModule.Add(module.ModuleName);
            }
        }

        return possessedModule;
    }

    public List<string> GetLockedModule()
    {
        List<string> missingModule = new List<string>();

        foreach (T module in playerModules)
        {
            if (!module.enabled) missingModule.Add(module.ModuleName);
        }

        return missingModule;
    }

    public void UnlockModule(string moduleName)
    {
        T module = playerModules.Find(module => module.ModuleName == moduleName);

        if (module != null && !module.enabled) 
        {
            module.enabled = true;
            Debug.Log($"HaveUnlocked = {moduleName}");
        }
        else if (module != null && module.enabled) Debug.Log($"Have already Unlocked = {moduleName}");
        else Debug.Log("Name invalid");
    }
    public void LockModule(string moduleName)
    {
        T module = playerModules.Find(module => module.ModuleName == moduleName);
        
        if (module != null && module.enabled) 
        {
            module.enabled = false;
            Debug.Log($"HaveLocked = {moduleName}");
        }
        else if (module != null && !module.enabled) Debug.Log($"Have already Locked = {moduleName}");
        else Debug.Log("Name invalid");
    }
}
