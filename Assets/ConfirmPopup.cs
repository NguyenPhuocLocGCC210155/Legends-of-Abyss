using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfirmPopup : MonoBehaviour
{
    public string saveFileName { get; set; }

    public void ClosePopup(){
        gameObject.SetActive(false);
    }

    public void ClearProfile(){
        string path = Path.Combine(Application.persistentDataPath, saveFileName + ".json");
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        ClosePopup();
    }
}
