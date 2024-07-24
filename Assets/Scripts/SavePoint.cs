using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour
{
    [SerializeField] Transform savePoint;
    [SerializeField] GameObject effect;
    [SerializeField] float minVolumeDistance = 12f;
    [SerializeField] float maxVolumeDistance = 5f;
    private float cooldownTime;
    MapUI mapUI;
    Animator animator;
    AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mapUI = FindObjectOfType<MapUI>();
        audioSource = GetComponent<AudioSource>();
        cooldownTime = 5f;
    }

    private void Update()
    {
         cooldownTime += Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(PlayerController.Instance.transform.position, transform.position);

        // Cập nhật âm lượng dựa trên khoảng cách
        if (distanceToPlayer < minVolumeDistance)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            float volume = 1 - (distanceToPlayer - maxVolumeDistance) / (minVolumeDistance - maxVolumeDistance);
            volume = Mathf.Clamp(volume, 0f, 1f); // Giới hạn âm lượng từ 0 đến 1
            audioSource.volume = volume;
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Start()
    {
        Vector3 checkPos = transform.position;
        if (GameManager.Instance.savePoint == checkPos
        && GameManager.Instance.saveScene.Equals(SceneManager.GetActiveScene().name)
        && PlayerController.Instance.isAlive == false)
        {
            animator.SetTrigger("Active");
            StartCoroutine(ActiveEffect());
        }
        animator.SetBool("Idle", true);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.UpArrow) && cooldownTime >= 3f && PlayerController.Instance.canControl == true && PlayerController.Instance.isLie != true)
        {
            GameManager.Instance.saveScene = SceneManager.GetActiveScene().name;
            GameManager.Instance.savePoint = savePoint.position;
            StartCoroutine(ActiveEffect());
            cooldownTime = 0;
            GameManager.Instance.SaveProgress();
            animator.SetTrigger("Active");
        }
    }

    IEnumerator ActiveEffect()
    {
        yield return new WaitForSeconds(1f);
        GameObject obj = Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(obj, 2f);
    }
}
