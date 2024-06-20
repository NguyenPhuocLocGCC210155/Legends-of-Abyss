using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class DestroyAfterAnimation : MonoBehaviour
{
    [SerializeField] bool useDissolve = true;
    [SerializeField] bool useVertical = false;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Disslove>())
        {
            float time = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
            StartCoroutine(DissolveAffterAnimation(time));
            Destroy(gameObject, time + 2);
        }
        else
        {
            Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }

    IEnumerator DissolveAffterAnimation(float time){
        yield return new WaitForSeconds(time);
        GetComponent<Disslove>().Vanish(useDissolve, useVertical);
    }

    // Update is called once per frame
}
