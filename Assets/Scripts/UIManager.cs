using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set;}
    public SceneFader sceneFader; 
    public GameObject mapHandler;
    public GameObject emptyMap;
    public GameObject map;
    private void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }else{
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        sceneFader = GetComponentInChildren<SceneFader>();  
    }

    public void OpenMap(bool isOpen){
        mapHandler.SetActive(isOpen);
        CheckHaveMapOrEmpty();
    }

    private void Start() {
        CheckHaveMapOrEmpty();
    }

    public void CheckHaveMapOrEmpty(){
        if (GameManager.Instance.savedMap.Count > 0){
            emptyMap.SetActive(false);
        }else{
            emptyMap.SetActive(true);;
        }
    }
}
