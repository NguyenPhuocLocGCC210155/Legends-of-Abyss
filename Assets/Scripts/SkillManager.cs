using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] List<Skills> listSkill;
    public List<Skills> unlockSkills;
    public Skills[] equippedSkills = new Skills[3];

    public void EquipSkill(int slot, string nameSkill)
    {
        Skills newSkill = GetSkillByName(nameSkill);
        if (slot >= 0 && slot < equippedSkills.Length)
        {
            equippedSkills[slot] = newSkill;
        }
    }

    public void ActivateSkill(int slot, GameObject user)
    {
        if (PlayerController.Instance.Mana >= (equippedSkills[slot].manaCosumed/100))
        {
            if (slot >= 0 && slot < equippedSkills.Length && equippedSkills[slot] != null)
            {
                if (PlayerController.Instance.chamberCount >= slot + 1)
                {
                    equippedSkills[slot].Activate();
                    PlayerController.Instance.Mana -= equippedSkills[slot].manaCosumed/100;
                }
            }
        }
    }

    public List<string> GetSkillUnlocked()
    {
        List<string> tempt = new List<string>();
        foreach (Skills name in unlockSkills)
        {
            tempt.Add(name.skillName);
        }
        return tempt;
    }

    public string[] GetEquippedSkill()
    {
        string[] tempt = new string[3];
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            if (equippedSkills[i] != null)
            {
                tempt[i] = equippedSkills[i].skillName;
            }
        }
        return tempt;
    }

    public void LoadUnlockSkill(List<string> listSkillName)
    {
        foreach (Skills skill in listSkill)
        {
            if (listSkillName.Contains(skill.skillName))
            {
                unlockSkills.Add(skill);
            }
        }
    }

    public void LoadEquippedSkill(string[] equipedSkill)
    {
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            equippedSkills[i] = GetSkillByName(equipedSkill[i]);
        }
    }

    public Skills GetSkillByName(string name)
    {
        foreach (Skills s in listSkill)
        {
            if (s.skillName.Equals(name))
            {
                return s;
            }
        }
        return null;
    }

    public bool GetUnlockSkillByName(string name)
    {
        Skills skills = GetSkillByName(name);
        if (unlockSkills.Contains(skills))
        {
            return true;
        }
        return false;
    }

    public void AddUnlockSkill(string name)
    {
        if (GetSkillByName(name) != null)
        {
            unlockSkills.Add(GetSkillByName(name));
        }
    }
}
