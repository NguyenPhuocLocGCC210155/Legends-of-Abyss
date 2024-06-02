using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour
{
    [SerializeField] GameObject effect;
    private float cooldownTime;
    MapUI mapUI;
    Animator animator;
    

    private void Awake() {
        animator = GetComponent<Animator>();
        mapUI = FindObjectOfType<MapUI>();
        cooldownTime = 5f;
    }
    private void Start() {
        Vector2 checkPos = transform.position;
        if (GameManager.Instance.savePoint == checkPos 
        && GameManager.Instance.saveScene.Equals(SceneManager.GetActiveScene().name)
        && PlayerController.Instance.isAlive == false)
        {
            animator.SetTrigger("Active");
            StartCoroutine(ActiveEffect());
        }
        animator.SetBool("Idle", true);
    }
    private void Update() {
        cooldownTime += Time.deltaTime;
    }
    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.UpArrow) && cooldownTime >= 5f){
            GameManager.Instance.saveScene = SceneManager.GetActiveScene().name;
            GameManager.Instance.savePoint = gameObject.transform.position;
            StartCoroutine(ActiveEffect());
            cooldownTime = 0;
            GameManager.Instance.SaveProgress();
            animator.SetTrigger("Active");
        }
    }

    IEnumerator ActiveEffect(){
        yield return new WaitForSeconds(1f);
        GameObject obj = Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(obj, 2f);
    }
}
