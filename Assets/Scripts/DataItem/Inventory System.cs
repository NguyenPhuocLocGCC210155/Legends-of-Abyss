using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public GameObject slotHolder;
    public GameObject slotItems;
    public Canvas canvas;
    public List<SlotClass> listItem = new List<SlotClass>();
    private Collider2D objectDetected;
    private bool isOpenInventory = false;
    private bool isItemPickUp;


    private List<GameObject> listGuiItem = new List<GameObject>();

    private void Start()
    {
        RefreshUI();
        canvas.gameObject.SetActive(isOpenInventory);
    }

    private void Update()
    {
        Inventory();
        PickUpItem();
        GetClosetSlot();
    }

    public void PickUpItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && isItemPickUp == true && objectDetected != null && isOpenInventory == false)
        {
            List<DropItem> itemClasses = objectDetected.GetComponent<ListDropItem>().dropItems;
            foreach (DropItem i in itemClasses)
            {
                RandomQuantityItemFromObject(i.minCount, i.maxCount, i.item);
            }
            Destroy(objectDetected.gameObject);
        }
    }

    public void Inventory()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (isOpenInventory == true)
            {
                isOpenInventory = false;
                canvas.gameObject.SetActive(isOpenInventory);
            }
            else
            {
                isOpenInventory = true;
                canvas.gameObject.SetActive(isOpenInventory);
            }
        }
    }

    private void RefreshUI()
    {
        ClearUI(slotHolder.transform);
        for (int i = 0; i < listItem.Count; i++)
        {
            GameObject item = Instantiate(slotItems, slotHolder.transform);
            listGuiItem.Add(item);
            Image image = item.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            TextMeshProUGUI quantity = item.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
            try
            {
                image.sprite = listItem[i].GetItem().itemIcon;
                quantity.text = listItem[i].GetQuantity().ToString();
            }
            catch
            {
                // item.active = false;
            }
        }
    }

    void ClearUI(Transform parent)
    {
        listGuiItem.Clear();
        // Duyệt qua từng đối tượng con của parent và xóa chúng
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        // Sau khi xóa hết, làm sạch danh sách con của parent
        parent.DetachChildren();
    }

    public SlotClass IsContainItem(ItemClass itemClass)
    {
        foreach (SlotClass item in listItem)
        {
            if (item.GetItem().Equals(itemClass))
            {
                return item;
            }
        }
        return null;
    }

    public void AddItem(ItemClass item, int quantity)
    {
        SlotClass slotClass = IsContainItem(item);
        if (slotClass != null)
        {
            slotClass.PlusQuantity(quantity);
        }
        else
        {
            listItem.Add(new SlotClass(item, quantity));
        }
        RefreshUI();
    }

    public void RemoveItem(ItemClass itemClass, int quantity)
    {
        SlotClass tempt = IsContainItem(itemClass);
        if (tempt != null && quantity >= 0)
        {
            if (tempt.GetQuantity() > quantity)
            {
                tempt.SubQuantity(quantity);
            }
            else
            {
                SlotClass slotRemove = new SlotClass();
                foreach (SlotClass slot in listItem)
                {
                    if (slot.GetItem() == itemClass)
                    {
                        slotRemove = slot;
                        break;
                    }
                }
                listItem.Remove(slotRemove);
            }
        }
        RefreshUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Itempickup")
        {
            isItemPickUp = true;
            objectDetected = other;
        }
        if (other.gameObject.tag == "Item")
        {
            List<DropItem> itemClasses = other.GetComponent<ListDropItem>().dropItems;
            foreach (DropItem i in itemClasses)
            {
                RandomQuantityItemFromObject(i.minCount, i.maxCount, i.item);
            }
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Itempickup")
        {
            isItemPickUp = false;
            objectDetected = null;
        }
    }

    public void RandomQuantityItemFromObject(int min, int max, ItemClass item)
    {
        int quantity = UnityEngine.Random.Range(min, max);
        AddItem(item, quantity);
    }

    private SlotClass GetClosetSlot()
    {
        for (int i = 0; i < listGuiItem.Count; i++)
        {
            if (Vector2.Distance(listGuiItem[i].transform.position, Input.mousePosition) <= 25)
            {
                TextMeshProUGUI quantity = listGuiItem[i].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
                // Debug.Log(quantity.text );
                return listItem[i];
            }
        }
        return null;
    }
}
