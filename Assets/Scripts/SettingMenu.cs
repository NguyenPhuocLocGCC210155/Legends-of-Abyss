using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public void SetVolume(float volume){
        audioMixer.SetFloat("Volume", volume);
    }
    
    public void SetQuality(int index){
        QualitySettings.SetQualityLevel(index);
    }

    public void SetFullScreen(bool isFullScreen){
        Screen.fullScreen = isFullScreen;
    }

    public void Quit(){
        Application.Quit();
    }
}
