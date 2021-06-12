using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] DrinkManager drinkManager;
    [SerializeField] Image screenAlpha;

    private int currentDialog, showedDialog;

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
            drinkManager.EnableDrink(false);
        }

        if (screenAlpha == null)
        {
            Debug.Log("<color=red>screenAlpha reference not found.</color>");
        }
        else
        {
            screenAlpha.DOFade(0.0f, 3f);
        }

        currentDialog = 0;
        showedDialog = -1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentDialog == 0 && showedDialog < 0)
        {
            dialogueManager.RegisterDialogue("It's 7 o'clock in the evening. The Traverse Bar is open.");
            dialogueManager.RegisterDialogue("This is a place where people go to drink and socialize.");
            dialogueManager.RegisterDialogue("Your job is to provide entertainment and fulfill the customer's needs.");
            dialogueManager.RegisterDialogue("Last but not least, create a suitable cocktail that suites the mood.");
            dialogueManager.RegisterDialogue("Ever heard of cocktails?");
            dialogueManager.RegisterDialogue("It's an alchoholic beverage with different methods of preparation.");
            dialogueManager.RegisterDialogue("By using several types of ingredients, they +JOIN TOGETHER+ to create a symphony of " +
                                             "relaxation as it calms the mood of the consumer.");
            dialogueManager.RegisterDialogue("And here it is! You get a cocktail.");
            showedDialog++;
            return;
        }

        if (currentDialog == 1 && showedDialog < 1)
        {
            drinkManager.OpenWindow(true);
            drinkManager.EnableDrink(true);
            StartCoroutine(Wait(1.5f));
            DialogEnded();
            showedDialog++;
            return;
        }

        if (currentDialog == 2 && showedDialog < 2)
        {
            dialogueManager.RegisterDialogue("As a warm up for the day, let's make something for yourself.");
            dialogueManager.RegisterDialogue("What is the best drink you think that will represent your feeling right now?");
            DialogEnded();
            showedDialog++;
            return;
        }
    }

    private IEnumerator Wait(float time)
    {
        enabled = false;

        yield return new WaitForSeconds(time);

        enabled = true;
    }

    public void DialogEnded()
    {
        currentDialog++;
    }
}
