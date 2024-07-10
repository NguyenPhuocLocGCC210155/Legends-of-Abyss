using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioUI : MonoBehaviour
{
    [SerializeField] AudioClip hover, click;
    AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void SoundOnClick(){
        audioSource.PlayOneShot(click);
    }

    public void SoundOnHover(){
        audioSource.PlayOneShot(hover);
    }
}
