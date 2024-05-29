using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeartController : MonoBehaviour
{
    PlayerController player;
    private GameObject[] heartContainer;
    private Image[] heartFill;
    public Transform heartsParent;
    public GameObject heartContainerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.Instance;
        heartContainer = new GameObject[PlayerController.Instance.maxHp];
        heartFill = new Image[PlayerController.Instance.maxHp];

        PlayerController.Instance.OnhealthChangeCallBack += UpdateHeartsUI;
        InstantiateHeartContainer();
        UpdateHeartsUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetHeartContainer(){
        for (int i = 0; i < heartContainer.Length; i++)
        {
            if (i < PlayerController.Instance.maxHp)
            {
                heartContainer[i].SetActive(true);
            }else{
                heartContainer[i].SetActive(false);
            }
        }
    }

    void SetFieldHeart(){
        for (int i = 0; i < heartFill.Length; i++)
        {
            if (i < PlayerController.Instance.hp)
            {
                heartFill[i].fillAmount = 1;
            }else{
                heartFill[i].fillAmount = 0;
            }
        }
    }

    void InstantiateHeartContainer(){
        for (int i = 0; i < player.maxHp; i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab);
            temp.transform.SetParent(heartsParent, false);
            heartContainer[i] = temp;
            heartFill[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }

    void UpdateHeartsUI(){
        SetHeartContainer();
        SetFieldHeart();
    }

}
