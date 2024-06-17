using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkilSlotUIContainer : MonoBehaviour
{
    public Image Slot1;
    public Image Slot2;
    public Image Slot3;
    [SerializeField] Sprite noneSprite;
    [SerializeField] Sprite defaultSprite;

    // Start is called before the first frame update
    void Start()
    {
        UpdateSlotUI();
    }

    private void OnEnable()
    {
        UpdateSlotUI();
    }

    void UpdateSlotUI()
    {
        switch (PlayerController.Instance.chamberCount)
        {
            case 1:
                Slot1.GetComponent<SkillSlot>().isUnlock = true;
                Slot2.GetComponent<SkillSlot>().isUnlock = false;
                Slot3.GetComponent<SkillSlot>().isUnlock = false;
                Slot2.sprite = noneSprite;
                Slot3.sprite = noneSprite;
                break;
            case 2:
                Slot1.GetComponent<SkillSlot>().isUnlock = true;
                Slot2.GetComponent<SkillSlot>().isUnlock = true;
                Slot3.GetComponent<SkillSlot>().isUnlock = false;
                Slot3.sprite = noneSprite;
                break;
            case 3:
                Slot1.GetComponent<SkillSlot>().isUnlock = true;
                Slot2.GetComponent<SkillSlot>().isUnlock = true;
                Slot3.GetComponent<SkillSlot>().isUnlock = true;
                break;
            default:
                Slot1.GetComponent<SkillSlot>().isUnlock = false;
                Slot2.GetComponent<SkillSlot>().isUnlock = false;
                Slot3.GetComponent<SkillSlot>().isUnlock = false;
                Slot1.sprite = noneSprite;
                Slot2.sprite = noneSprite;
                Slot3.sprite = noneSprite;
                break;
        }
    }
}
