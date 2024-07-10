using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillContainer : MonoBehaviour
{
    SkillDragHandler[] childs;
    void Awake()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        childs = GetComponentsInChildren<SkillDragHandler>();
        for (int i = 0; i < childs.Length; i++)
        {
            if (PlayerController.Instance.skillManager.GetSkillUnlocked().Contains(childs[i].gameObject.name))
            {
                childs[i].SetActive(true);
                childs[i].GetComponent<Image>().sprite = childs[i].defaultImage;
            }
            else
            {
                childs[i].SetActive(false);
                childs[i].GetComponent<Image>().sprite = childs[i].noneImage;
            }
        }
    }

    void OnEnable()
    {
        UpdateUI();
    }
}
