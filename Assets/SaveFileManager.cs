using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileManager : MonoBehaviour
{
    public static SaveFileManager Instance { get; private set; }
    public string SaveFileName { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
    