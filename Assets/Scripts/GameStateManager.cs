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
    [SerializeField] List<RectTransform> shakableObjects;
    [SerializeField] GameObject shaker;
    [SerializeField] Image bartender, gedeon;
    private bool isScreenShaking;


    [Header("Debug")]
    [SerializeField] private int currentDialog, showedDialog;
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
        if (showedDialog >= currentDialog) return;

        if (Input.GetKeyDown(KeyCode.Space) && currentDialog == 0)
        {
            AudioManager.Instance.PlayMusicWithFade("Theme1", 1.5f);
            AudioManager.Instance.SetMusicVolume(0.65f);
            bartender.DOFade(1.0f, 1.5f);
            dialogueManager.RegisterDialogue("It's 7 o'clock in the evening. The Traverse Bar is open.");
            dialogueManager.RegisterDialogue("This is a place where people go to drink and socialize.");
            dialogueManager.RegisterDialogue("Your job is to provide entertainment and fulfill the customer's needs.");
            dialogueManager.RegisterDialogue("Last but not least, create a suitable cocktail that suites the mood.");
            dialogueManager.RegisterDialogue("Ever heard of cocktails?");
            dialogueManager.RegisterDialogue("It's an alchoholic beverage with different methods of preparation.");
            dialogueManager.RegisterDialogue("By using several types of ingredients, they +JOIN TOGETHER+ to create a symphony of " +
                                             "relaxation as it calms the mood of the consumer.");
            dialogueManager.RegisterDialogue("And here it is! You get a cocktail.");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 1)
        {
            drinkManager.OpenWindow(true);
            drinkManager.EnableDrink(true);
            shaker.SetActive(true);
            shaker.GetComponent<CanvasGroup>().DOFade(1.0f, 0.45f);
            showedDialog = currentDialog;
            StartCoroutine(Wait(1.5f));
            return;
        }

        if (currentDialog == 2)
        {
            dialogueManager.RegisterDialogue("As a warm up, let's make something for yourself.");
            dialogueManager.RegisterDialogue("What is the best drink that will represent your mood right now?");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 4)
        {
            drinkManager.ParameterRatio(out float hap, out float sad, out float surp, out float excite, out float anger);
            if (Mathf.Max(hap, sad, surp, excite, anger) == hap)
            {
                dialogueManager.RegisterDialogue("Nice, look like you're starting your day in a happy mood.");
            }
            else if (Mathf.Max(hap, sad, surp, excite, anger) == surp)
            {
                dialogueManager.RegisterDialogue("Nicely done. Maybe too much goblin piss this time.");
            }
            else if (Mathf.Max(hap, sad, surp, excite, anger) == excite)
            {
                dialogueManager.RegisterDialogue("Nice, that is a really gassy drink!");
            }
            else if (Mathf.Max(hap, sad, surp, excite, anger) == anger)
            {
                dialogueManager.RegisterDialogue("Nice, the eastern favorite.");
            }
            else
            {
                dialogueManager.RegisterDialogue("Nice, just like always.");
            }
            dialogueManager.RegisterDialogue("...", "Felix");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 5)
        {
            showedDialog = currentDialog;
            StartCoroutine(Wait(1.0f));
            return;
        }

        if (currentDialog == 6)
        {
            AudioManager.Instance.PlaySFX("DoorOpenFast");
            showedDialog = currentDialog;
            StartCoroutine(Wait(2.5f));
            return;
        }

        if (currentDialog == 7)
        {
            gedeon.DOFade(1.0f, 1.5f);
            dialogueManager.RegisterDialogue("The first customer has walked into the bar. It’s time to get to work.");
            dialogueManager.RegisterDialogue("What the hell are you staring at?", "???");
            dialogueManager.RegisterDialogue("Isn’t this dump supposed to be a bar? Make me a freaking cocktail already.", "???");
            dialogueManager.RegisterDialogue("...", "Felix");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 8)
        {
            drinkManager.ResetDrinkManager();
            drinkManager.EnableDrink(true);
            drinkManager.GetDrinkMenu().EnableButton(true);
            shaker.SetActive(true);
            shaker.GetComponent<CanvasGroup>().DOFade(1.0f, 0.45f);
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 9)
        {
            dialogueManager.RegisterDialogue("...", "Felix");
            dialogueManager.RegisterDialogue("My name? You really think I’d just tell you.");
            dialogueManager.RegisterDialogue("I’m an assassin, dumbass. Can’t you tell by my blades?", "???");
            dialogueManager.RegisterDialogue("I have always killed people for a living. I’ve killed so many I’ve lost count... or I stopped counting. I can’t remember...", "???");
            dialogueManager.RegisterDialogue("Soon, that won’t be possible anymore. So, what the heck am I supposed to do now?", "???");
            dialogueManager.RegisterDialogue("...", "Felix");
            dialogueManager.RegisterDialogue("Ahahaha! Get a new hobby?! Is that some kind of joke?", "???");
            dialogueManager.RegisterDialogue("Not bad...you’re not as boring as I thought. Give me something strong, will you?", "???");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 10)
        {
            drinkManager.ResetDrinkManager();
            drinkManager.EnableDrink(true);
            drinkManager.GetDrinkMenu().EnableButton(true);
            shaker.SetActive(true);
            shaker.GetComponent<CanvasGroup>().DOFade(1.0f, 0.45f);
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 11)
        {
            dialogueManager.RegisterDialogue("...", "Felix");
            dialogueManager.RegisterDialogue("I never enjoyed killing… so I guess I should be grateful for this break.");
            dialogueManager.RegisterDialogue("Though I can’t help but feel I have wasted my short time in this world ending other people’s lives.", "???");
            dialogueManager.RegisterDialogue("Haha! Yeah, you’re right. A killer like me deserves to die. Shit… what a waste.", "???");
            dialogueManager.RegisterDialogue("Tell me a joke before I leave.", "???");
            dialogueManager.RegisterDialogue("...", "Felix");
            dialogueManager.RegisterDialogue("Was that supposed to be funny? It sucked. And I thought you were alright. How embarrassing.", "???");
            dialogueManager.RegisterDialogue("“You owe me another round for that awful joke.”", "???");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 12)
        {
            drinkManager.ResetDrinkManager();
            drinkManager.EnableDrink(true);
            drinkManager.GetDrinkMenu().EnableButton(true);
            shaker.SetActive(true);
            shaker.GetComponent<CanvasGroup>().DOFade(1.0f, 0.45f);
            showedDialog = currentDialog;
            return;
        }


        if (currentDialog == 13)
        {
            dialogueManager.RegisterDialogue("So, tell me about yourself?", "???");
            dialogueManager.RegisterDialogue("...", "Felix");
            dialogueManager.RegisterDialogue("The quiet type, huh… I bet you get a lot of people come here spilling their guts out to you. All while you just stand there and listen. Kinda pathetic really.", "???");
            dialogueManager.RegisterDialogue("Whatever, I’m out of this shithole.", "???");
            dialogueManager.RegisterDialogue("Thanks for the drinks, Felix...", "???");
            dialogueManager.RegisterDialogue("Oh, and the name’s Gedeon. No point in hiding it anymore.", "Gedeon");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 14)
        {
            AudioManager.Instance.PlaySFX("doorclose");
            gedeon.DOFade(0f, 1.5f);
            showedDialog = currentDialog;
            return;
        }
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        DialogEnded();
    }

    public void DialogEnded(int increment = 1)
    {
        currentDialog += increment;
    }

    public void ShakeObjects(float magnitude, float time)
    {
        foreach (RectTransform obj in shakableObjects)
        {
            obj.gameObject.AddComponent<Shake>();
            obj.gameObject.GetComponent<Shake>().SetUp(magnitude, time);
        }
    }
}
