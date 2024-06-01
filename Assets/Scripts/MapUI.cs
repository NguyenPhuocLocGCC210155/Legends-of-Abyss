using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    public GameObject mapContainer;
    private Dictionary<string, GameObject> sceneMapImages = new Dictionary<string, GameObject>();

    void Start()
    {
        foreach (Transform child in mapContainer.transform)
        {
            sceneMapImages.Add(child.name, child.gameObject);
            child.gameObject.SetActive(false); // Ẩn tất cả các hình ảnh bản đồ nhỏ
        }
        UpdateMapUI();
    }

    public void UpdateMapUI()
    {
        foreach (string scene in GameManager.Instance.savedMap)
        {
            if (sceneMapImages.ContainsKey(scene))
            {
                sceneMapImages[scene].SetActive(true); // Hiển thị các hình ảnh bản đồ nhỏ đã mở khóa
            }
        }
    }

    void OnEnable()
    {
        UpdateMapUI(); // Cập nhật bản đồ khi MapUI được bật
    }
}
