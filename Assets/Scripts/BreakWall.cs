using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : MonoBehaviour
{
    [SerializeField] string InstanceName = "";
    [SerializeField] int hp = 3;
    [SerializeField] GameObject breakEffect;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip breakSound;
    [SerializeField] List<GameObject> hiddemRoomLayer;

    public int Hp
    {
        get => hp;
        set
        {
            if (hp != value)
            {
                hp -= 1;
                if (hp <= 0)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<Collider2D>().enabled = false;
                    Instantiate(breakEffect);
                    Destroy(gameObject, 1f);
                    GameManager.Instance.breakwalls.Add(InstanceName);
                    GameManager.Instance.audioSource.PlayOneShot(breakSound);
                }
                else
                {
                    GameManager.Instance.audioSource.PlayOneShot(hitSound);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.breakwalls.Contains(InstanceName))
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage()
    {
        Hp -= 1;
    }
}
