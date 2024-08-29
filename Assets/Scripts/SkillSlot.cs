using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] Sprite defaulImage;
    Image slotImage;
    public bool isUse;
    public bool isUnlock;
    [SerializeField]
    [Range(0, 2)]
    int slotIndex;

    private void Start()
    {
        slotImage = GetComponent<Image>();
        if (PlayerController.Instance.skillManager.equippedSkills[slotIndex] != null && isUnlock)
        {
            slotImage.sprite = PlayerController.Instance.skillManager.equippedSkills[slotIndex].skillIcon;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (isUnlock)
        {
            //Lấy kỹ năng được kéo thả
            SkillDragHandler skill = eventData.pointerDrag.GetComponent<SkillDragHandler>();
            if (skill != null && skill.isActive)
            {
                bool canEquip = PlayerController.Instance.skillManager.EquipSkill(slotIndex, skill.gameObject.name);
                // Thay đổi hình ảnh của slot thành hình ảnh của kỹ năng
                if (canEquip)
                {
                    slotImage.sprite = skill.GetComponent<Image>().sprite;
                    isUse = true;
                }
            }
        }
    }

    public void UnEquip()
    {
        Debug.Log("ádasbd");
        // Thực hiện hành động unequip
        PlayerController.Instance.skillManager.UnEquipSkill(slotIndex);
        // Đặt lại hình ảnh mặc định
        slotImage.sprite = defaulImage;
        isUse = false;
    }
}
