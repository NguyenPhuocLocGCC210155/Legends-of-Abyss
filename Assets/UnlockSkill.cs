using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSkill : MonoBehaviour
{
    [SerializeField] string InstanceName;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerController.Instance.skillManager.GetUnlockSkillByName(InstanceName)){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;
            PlayerController.Instance.skillManager.AddUnlockSkill(InstanceName);
        }
    }
}
