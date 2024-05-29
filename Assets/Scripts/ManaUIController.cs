using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaUIController : MonoBehaviour
{
    Image image;
    void Start()
    {
        image =  GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = PlayerController.Instance.mana;
    }
}
