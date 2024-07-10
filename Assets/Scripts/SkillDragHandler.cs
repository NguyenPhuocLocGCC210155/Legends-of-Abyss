using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Canvas canvas; // Tham chiếu đến canvas
    public Sprite noneImage;
    public Sprite defaultImage;
    [HideInInspector] public bool isActive;
    private CanvasGroup canvasGroup;
    private Image skillImage;
    private GameObject draggingIcon;
    private RectTransform draggingPlane;
    CanvasGroup canvasGroupTemp;
    Image draggingImage;
    InventoryManager inventoryManager;
    private bool isDragging = false;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        skillImage = GetComponent<Image>();
        inventoryManager = GetComponentInParent<InventoryManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isActive)
        {
            isDragging = true;
            // Tạo hình ảnh đại diện cho skill đang kéo
            draggingIcon = new GameObject("Dragging Icon");
            draggingIcon.transform.SetParent(canvas.transform, false);
            draggingIcon.transform.SetAsLastSibling();

            draggingImage = draggingIcon.AddComponent<Image>();
            draggingImage.raycastTarget = false;
            draggingImage.sprite = skillImage.sprite;
            draggingImage.SetNativeSize();
            draggingImage.transform.localScale = new Vector3(0.25f, 0.25f, 1);
            canvasGroupTemp = draggingIcon.AddComponent<CanvasGroup>();

            draggingImage.sprite = skillImage.sprite;
            canvasGroup.blocksRaycasts = false; // Cho phép xuyên qua khi kéo
            canvasGroup.alpha = 0.6f;
            canvasGroupTemp.blocksRaycasts = false;
            canvasGroupTemp.alpha = 0.6f;
            draggingPlane = canvas.transform as RectTransform;
            SetDraggedPosition(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            SetDraggedPosition(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isActive)
        {
            isDragging = false;
            canvasGroup.blocksRaycasts = true; // Chặn xuyên qua khi thả
            canvasGroup.alpha = 1f;
            draggingImage.raycastTarget = true;
            if (draggingIcon != null)
            {
                Destroy(draggingIcon); // Hủy hình ảnh đại diện khi thả
            }
        }
    }

    private void SetDraggedPosition(PointerEventData eventData)
    {
        if (draggingPlane != null)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingPlane, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos);
            draggingIcon.transform.position = globalMousePos;
        }
    }

    private void OnDisable()
    {
        if (isDragging)
        {
            // Hủy động tác kéo nếu đối tượng cha bị vô hiệu hóa
            OnEndDrag(null);
        }
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }
}
