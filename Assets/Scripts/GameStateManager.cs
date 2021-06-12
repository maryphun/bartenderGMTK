using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] DrinkManager drinkManager;

    private bool dialogueRegistered;

    void Start()
    {
        WindowManager.Instance.Initialization();

        if (dialogueManager == null)
        {
            Debug.Log("<color=red>dialogueManager reference not found.</color>");
        }
        else
        {
            dialogueManager.Initialization();
        }

        if (drinkManager == null)
        {
            Debug.Log("<color=red>drinkManager reference not found.</color>");
        }
        else
        {
            drinkManager.Initialization();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !dialogueRegistered)
        {
            dialogueManager.RegisterDialogue("Hello, give me a drink.", "Helaen");
            dialogueManager.RegisterDialogue("What?");
            dialogueManager.RegisterDialogue("Hello again!", "Cheebye");
            dialogueRegistered = true;
        }
    }
}
