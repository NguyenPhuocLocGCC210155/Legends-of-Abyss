using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string _InstanceName;
    [SerializeField] GameObject uiBorder;
    [SerializeField] TextMeshProUGUI uiText;
    [Header("First time dialog")]
    public string[] dialog;
    [Header("Talked dialog")]
    public string[] reapeatDialog;
    bool hasTalked = false;
    public bool isActive = false;
    int indexDialog = 0;
    void Start()
    {
        if (GameManager.Instance.npcName.Contains(_InstanceName))
        {
            hasTalked = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Z))
        {
            if (!isActive)
            {
                isActive = true;
                uiBorder.SetActive(true);
            }
            Interact();
        }
    }
    
    public void Interact()
    {
        string[] currentDialog = hasTalked ? reapeatDialog : dialog;

        ShowDialogue(currentDialog);

        if (!hasTalked && indexDialog == 0)
        {
            hasTalked = true;
        }
    }

    private void ShowDialogue(string[] dialogues)
    {
        if (indexDialog < dialogues.Length)
        {
            uiText.SetText(dialogues[indexDialog]);
            indexDialog++;
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        PlayerController.Instance.canControl = true; 
        uiBorder.SetActive(false); 
        indexDialog = 0; 
        isActive = false;
    }
}
