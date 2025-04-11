using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    [SerializeField] private List<SkillModule> playerSkills;

    private void Awake()
    {
        playerSkills = new List<SkillModule>(GetComponents<SkillModule>());
    }

    public bool HaveSkill(string skillName)
    {
        SkillModule skill = playerSkills.Find(skill => skill.SkillName == skillName);
        
        if (skill != null && skill.enabled) return true;
        else if (skill != null && !skill.enabled) return false;
        else
        {
            Debug.Log("Name invalid");
            return false;
        }
    }

    public List<string> GetPossessedSkills()
    {
        List<string> possessedSkills = new List<string>();

        foreach (SkillModule skillModule in playerSkills)
        {
            if (skillModule.enabled)
            {
                possessedSkills.Add(skillModule.SkillName);
            }
        }

        return possessedSkills;
    }

    public List<string> GetMissingSkills()
    {
        List<string> missingSkills = new List<string>();

        foreach (SkillModule skillModule in playerSkills)
        {
            if (!skillModule.enabled) missingSkills.Add(skillModule.SkillName);
        }

        return missingSkills;
    }

    public void UnlockSkills(string skillName)
    {
        SkillModule skill = playerSkills.Find(skill => skill.SkillName == skillName);
        
        if (skill != null && !skill.enabled) 
        {
            skill.enabled = true;
            Debug.Log($"HaveUnlocked = {skillName}");
        }
        else if (skill != null && skill.enabled) Debug.Log($"Have already Unlocked = {skillName}");
        else Debug.Log("Name invalid");
    }
    public void LockSkills(string skillName)
    {
        SkillModule skill = playerSkills.Find(skill => skill.SkillName == skillName);
        
        if (skill != null && skill.enabled) 
        {
            skill.enabled = false;
            Debug.Log($"HaveLocked = {skillName}");
        }
        else if (skill != null && !skill.enabled) Debug.Log($"Have already Locked = {skillName}");
        else Debug.Log("Name invalid");
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         Debug.Log($"HaveSkill : Dash = {HaveSkill("Dash")}");
    //         foreach(string skill in GetPossessedSkills()) Debug.Log($"Have skill : {skill}");
    //         foreach(string skill in GetMissingSkills()) Debug.Log($"Doesn't have skill : {skill}");
    //     }

    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         UnlockSkills("Dash");
    //     }

    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         LockSkills("Dash");
    //     }

    // }

}
