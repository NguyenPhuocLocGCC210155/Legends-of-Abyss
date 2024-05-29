using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set;}
    public SceneFader sceneFader; 
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
        sceneFader = GetComponentInChildren<SceneFader>();  
    }
}
