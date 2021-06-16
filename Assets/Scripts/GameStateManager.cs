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
    [SerializeField] Image bartender, gedeon, dolores, claudius;
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

        if (currentDialog == 0)
        {
            AudioManager.Instance.SetMusicVolume(0.5f);
            AudioManager.Instance.PlayMusicWithFade("Theme1", 1.5f);
            AudioManager.Instance.SetMusicVolume(0.65f);
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
            bartender.DOFade(1.0f, 1.5f);
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
            AudioManager.Instance.PlayMusicWithCrossFade("Theme2", 4f);
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
            gedeon.GetComponent<SpriteSwapper>().ChangeExpressions(1);
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
            drinkManager.ParameterRatio(out float hap, out float sad, out float surp, out float excite, out float anger);
            drinkManager.GetAllIngredientAdded(out bool lemon, out bool lime, out bool mint, out bool ice, out bool poison);

            if (hap > 0.5 || poison)
            {
                dialogueManager.RegisterDialogue("Now this is what I'm talking about!", "???");
            }
            else if (sad > 0.5f)
            {
                dialogueManager.RegisterDialogue("This is making me feel freaking depressed, man.", "???");
            }
            else if (surp > 0.5f)
            {
                dialogueManager.RegisterDialogue("This isn't bad...surprisingly. I wasn't expecting much from you.", "???");
            }
            else if (excite > 0.5f)
            {
                dialogueManager.RegisterDialogue("This reminds me of that time whe-! Ah… never mind.", "???");
            }
            else if (anger > 0.5f)
            {
                dialogueManager.RegisterDialogue("What are you waiting for? Some kind of praise? Piss off.", "???");
            }

            gedeon.GetComponent<SpriteSwapper>().ChangeExpressions(0);
            dialogueManager.RegisterDialogue("...", "Felix");
            dialogueManager.RegisterDialogue("I never enjoyed killing… so I guess I should be grateful for this break.", "???");
            dialogueManager.RegisterDialogue("Though I can’t help but feel I have wasted my short time in this world ending other people’s lives.", "???");
            dialogueManager.RegisterDialogue("Haha! Yeah, you’re right. A killer like me deserves to die. Shit… what a waste.", "???");
            dialogueManager.RegisterDialogue("Tell me a joke before I leave.", "???");
            dialogueManager.RegisterDialogue("...", "Felix");
            dialogueManager.RegisterDialogue("Was that supposed to be funny? It sucked. And I thought you were alright. How embarrassing.", "???");
            dialogueManager.RegisterDialogue("You owe me another round for that awful joke.", "???");
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
            drinkManager.ParameterRatio(out float hap, out float sad, out float surp, out float excite, out float anger);
            drinkManager.GetAllIngredientAdded(out bool lemon, out bool lime, out bool mint, out bool ice, out bool poison);

            if (hap > 0.5 || poison)
            {
                dialogueManager.RegisterDialogue("Now this is what I'm talking about!", "???");
            }
            else if (sad > 0.5f)
            {
                dialogueManager.RegisterDialogue("This is making me feel freaking depressed, man.", "???");
            }
            else if (surp > 0.5f)
            {
                dialogueManager.RegisterDialogue("This isn't bad...surprisingly. I wasn't expecting much from you.", "???");
            }
            else if (excite > 0.5f)
            {
                dialogueManager.RegisterDialogue("This reminds me of that time whe-! Ah… never mind.", "???");
            }
            else if (anger > 0.5f)
            {
                dialogueManager.RegisterDialogue("What are you waiting for? Some kind of praise? Piss off.", "???");
            }

            gedeon.GetComponent<SpriteSwapper>().ChangeExpressions(3);
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
            StartCoroutine(Wait(2.5f));
            return;
        }

        if (currentDialog == 15)
        {
            AudioManager.Instance.PlaySFX("succeed");
            dialogueManager.RegisterDialogue("Gedeon walks out of the room and it suddenly feels easier to breathe. It was hard not to feel intimidated by his presence.");
            dialogueManager.RegisterDialogue("Maybe I should brush up on my comedy routine in case he ever decides to return. Not that I have the time for that...");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 16)
        {
            dialogueManager.RegisterDialogue("A perfect timing to relax. Why not make yourself a +Take A Break+?");
            dialogueManager.RegisterDialogue("Fill up half of the bottle with bitter Ōdachi, add an ice cubes and a lemon. The beverages should have a beautiful magenta color with some mixes of white color.");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 17)
        {
            drinkManager.ResetDrinkManager();
            drinkManager.EnableDrink(true);
            drinkManager.GetDrinkMenu().EnableButton(true);
            shaker.SetActive(true);
            shaker.GetComponent<CanvasGroup>().DOFade(1.0f, 0.45f);
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 18)
        {
            drinkManager.ParameterRatio(out float hap, out float sad, out float surp, out float excite, out float anger);
            drinkManager.GetAllIngredientAdded(out bool lemon, out bool lime, out bool mint, out bool ice, out bool poison);
            if (excite < anger && anger > 0.5f && lemon && ice && !poison)
            {
                AudioManager.Instance.PlaySFX("missioncomplete", 0.7f);
                dialogueManager.RegisterDialogue("You did it!");
                dialogueManager.RegisterDialogue("It is also about time for the next customer to come in. Let's clean the mess.");
                showedDialog = currentDialog;
            }
            else
            {
                drinkManager.ResetDrinkManager();
                drinkManager.EnableDrink(true);
                drinkManager.GetDrinkMenu().EnableButton(true);
                dialogueManager.RegisterDialogue("That does not look like +Take A Break+.", "", false);
                dialogueManager.RegisterDialogue("Fill up half of the bottle with bitter Ōdachi, add an ice cubes and a lemon. The beverages should have a beautiful magenta color with some mixes of white color.", "", false);
                currentDialog = 17;
                showedDialog = -currentDialog;
            }
            return;
        }

        if (currentDialog == 19)
        {
            showedDialog = currentDialog;
            StartCoroutine(Wait(1.0f));
            return;
        }

        if (currentDialog == 20)
        {
            AudioManager.Instance.PlaySFX("DoorOpenFast");
            showedDialog = currentDialog;
            StartCoroutine(Wait(2.5f));
            return;
        }

        if (currentDialog == 21)
        {
            AudioManager.Instance.PlayMusicWithCrossFade("Theme3", 4f);
            dolores.DOFade(1.0f, 1.5f);
            dialogueManager.RegisterDialogue("A young woman walks into the bar and takes a seat. She looks miserable and silently stares at the counter.");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 22)
        {
            dialogueManager.RegisterDialogue("Oh, I’m sorry... I didn’t mean to ignore you. I have a lot on my mind right now.", "???");
            dialogueManager.RegisterDialogue("It's nice to meet you, Felix. My name is Dolores.", "Dolores");
            dialogueManager.RegisterDialogue("I passed a strange person on my way in. They gave me such an uneasy feeling, I can't explain it.", "Dolores");
            dialogueManager.RegisterDialogue("Sure, I’ll have a drink. Why don’t you surprise me? I need something to help calm my nerves.", "Dolores");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 23)
        {
            drinkManager.ResetDrinkManager();
            drinkManager.EnableDrink(true);
            drinkManager.GetDrinkMenu().EnableButton(true);
            shaker.SetActive(true);
            shaker.GetComponent<CanvasGroup>().DOFade(1.0f, 0.45f);
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 24)
        {
            dolores.GetComponent<SpriteSwapper>().ChangeExpressions(3);
            dialogueManager.RegisterDialogue("Thank you… It has been so long since I’ve had a drink in a bar like this. The last time was with...", "Dolores");
            dialogueManager.RegisterDialogue("With my late husband. He was fond of places like this. But I never really cared for them.", "Dolores");
            dialogueManager.RegisterDialogue("We were trying to conceive but were having no such luck. When we heard the news, well… we didn’t think there was much point in trying anymore.", "Dolores");
            dialogueManager.RegisterDialogue("Another drink, please. Anything…", "Dolores");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 25)
        {
            drinkManager.ResetDrinkManager();
            drinkManager.EnableDrink(true);
            drinkManager.GetDrinkMenu().EnableButton(true);
            shaker.SetActive(true);
            shaker.GetComponent<CanvasGroup>().DOFade(1.0f, 0.45f);
            showedDialog = currentDialog;
            return;
        }
        
        if (currentDialog == 26)
        {
            drinkManager.ParameterRatio(out float hap, out float sad, out float surp, out float excite, out float anger);
            drinkManager.GetAllIngredientAdded(out bool lemon, out bool lime, out bool mint, out bool ice, out bool poison);

            if (hap > 0.5 || poison)
            {
                dialogueManager.RegisterDialogue("Oh my… I haven't felt this way in so long. I had forgotten what it felt like to be happy.", "Dolores");
            }
            else if (sad > 0.5f)
            {
                dialogueManager.RegisterDialogue("Hmm, I was expecting something more.", "Dolores");
            }
            else if (surp > 0.5f)
            {
                dialogueManager.RegisterDialogue("Wow, I think I have a new favorite.", "Dolores");
            }
            else if (excite > 0.5f)
            {
                dialogueManager.RegisterDialogue("They took him from me and no justice was served. It's not fair! Oh, I'm sorry. I lost my temper...", "Dolores");
            }
            else if (anger > 0.5f)
            {
                dialogueManager.RegisterDialogue("Is everything alright?", "Dolores");
            }

            dolores.GetComponent<SpriteSwapper>().ChangeExpressions(2);
            dialogueManager.RegisterDialogue("I bet you’re wondering how he died. It wasn’t long after the riots started.", "Dolores");
            dialogueManager.RegisterDialogue("Intruders, they broke into our home. My husband told me to hide, so I did. I assumed he’d hide with me but he didn’t.", "Dolores");
            dialogueManager.RegisterDialogue("They quickly found him and… I’m sure you can imagine the outcome.", "Dolores");
            dialogueManager.RegisterDialogue("I want to join him in the afterlife. But I want it to be as painless as possible. I guess that makes me a terrible coward after all he went through.", "Dolores");
            dialogueManager.RegisterDialogue("Poison, please...", "Dolores");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 27)
        {
            drinkManager.ResetDrinkManager();
            drinkManager.EnableDrink(true);
            drinkManager.GetDrinkMenu().EnableButton(true);
            shaker.SetActive(true);
            shaker.GetComponent<CanvasGroup>().DOFade(1.0f, 0.45f);
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 28)
        {
            drinkManager.ParameterRatio(out float hap, out float sad, out float surp, out float excite, out float anger);
            drinkManager.GetAllIngredientAdded(out bool lemon, out bool lime, out bool mint, out bool ice, out bool poison);
            if (poison)
            {
                dolores.GetComponent<SpriteSwapper>().ChangeExpressions(2);
                dialogueManager.RegisterDialogue("Thank you. You have been so helpful.", "Dolores");
                dialogueManager.RegisterDialogue("I feel very sleepy suddenly. You don’t mind if I take a nap in the lounge, do you?", "Dolores");
                dialogueManager.RegisterDialogue("Such a gentleman.", "Dolores");
                dialogueManager.RegisterDialogue("I will be with you soon, my love.", "Dolores");
                showedDialog = currentDialog;
            }
            else
            {
                drinkManager.ResetDrinkManager();
                drinkManager.EnableDrink(true);
                drinkManager.GetDrinkMenu().EnableButton(true);
                dolores.GetComponent<SpriteSwapper>().ChangeExpressions(2);
                dialogueManager.RegisterDialogue("Thank you, it was a nice cocktail.", "Dolores", false);
                dialogueManager.RegisterDialogue("But I was asking for something that could end my suffer.", "Dolores", false);
                dialogueManager.RegisterDialogue("Poison, please...", "Dolores", false);
                currentDialog = 27;
                showedDialog = -currentDialog;
            }
            return;
        }

        if (currentDialog == 29)
        {
            AudioManager.Instance.PlaySFX("doorclose");
            dolores.DOFade(0f, 1.5f);
            showedDialog = currentDialog;
            StartCoroutine(Wait(2.5f));
            return;
        }

        if (currentDialog == 30)
        {
            AudioManager.Instance.PlaySFX("succeed");
            dialogueManager.RegisterDialogue("Dolores stumbles out of the room, almost tripping on her own feet a few times.");
            dialogueManager.RegisterDialogue("I hope she will be alright. She isn't the first person to come here asking me to serve them poison. Will she be the last?");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 31)
        {
            dialogueManager.RegisterDialogue("Let's do some exercise! Make something challenging.");
            dialogueManager.RegisterDialogue("+Long Island Ice Tea+, a balance mix of all type of alcohol. Lastly, add some ice cubes.");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 32)
        {
            drinkManager.ResetDrinkManager();
            drinkManager.EnableDrink(true);
            drinkManager.GetDrinkMenu().EnableButton(true);
            shaker.SetActive(true);
            shaker.GetComponent<CanvasGroup>().DOFade(1.0f, 0.45f);
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 33)
        {
            drinkManager.ParameterRatio(out float hap, out float sad, out float surp, out float excite, out float anger);
            drinkManager.GetAllIngredientAdded(out bool lemon, out bool lime, out bool mint, out bool ice, out bool poison);
            if (hap > 0.10f && sad > 0.10f && surp > 0.10f && excite > 0.10f && anger > 0.10f && ice && !poison)
            {
                AudioManager.Instance.PlaySFX("missioncomplete", 0.7f);
                dialogueManager.RegisterDialogue("You did it! +Long Island Ice Tea+ is such a boozy cocktail, but taste like tea. How bizarre!");
                dialogueManager.RegisterDialogue("A man slowly makes his way to the bar. He appears to be dragging some kind of machine behind him. ");
                showedDialog = currentDialog;
            }
            else
            {
                drinkManager.ResetDrinkManager();
                drinkManager.EnableDrink(true);
                drinkManager.GetDrinkMenu().EnableButton(true);
                dialogueManager.RegisterDialogue("That does not look like +Long Island Ice Tea+.", "", false);
                dialogueManager.RegisterDialogue("It is a balance mix of all type of alcohol. Lastly, add some ice cubes.", "", false);
                currentDialog = 32;
                showedDialog = -currentDialog;
            }
            return;
        }

        if (currentDialog == 34)
        {
            showedDialog = currentDialog;
            StartCoroutine(Wait(1.0f));
            return;
        }

        if (currentDialog == 35)
        {
            AudioManager.Instance.PlaySFX("DoorOpenFast");
            showedDialog = currentDialog;
            StartCoroutine(Wait(2.5f));
            return;
        }

        if (currentDialog == 36)
        {
            AudioManager.Instance.PlayMusicWithCrossFade("Theme4", 4f);
            claudius.DOFade(1.0f, 1.5f);
            dialogueManager.RegisterDialogue("Good evening. What a fine establishment you have here.", "???");
            dialogueManager.RegisterDialogue("You can call me Claudius. I'm pleased to meet you.", "Claudius");
            dialogueManager.RegisterDialogue("Now, with the introductions out of the way. Please give me a taste of your finest cocktail.", "Claudius");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 37)
        {
            drinkManager.ResetDrinkManager();
            drinkManager.EnableDrink(true);
            drinkManager.GetDrinkMenu().EnableButton(true);
            shaker.SetActive(true);
            shaker.GetComponent<CanvasGroup>().DOFade(1.0f, 0.45f);
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 38)
        {
            drinkManager.ParameterRatio(out float hap, out float sad, out float surp, out float excite, out float anger);
            drinkManager.GetAllIngredientAdded(out bool lemon, out bool lime, out bool mint, out bool ice, out bool poison);

            if (hap > 0.5)
            {
                dialogueManager.RegisterDialogue("This is marvelous! I admire your craftsmanship.", "Claudius");
            }
            else if (sad > 0.5f)
            {
                dialogueManager.RegisterDialogue("I expected better than this…", "Claudius");
            }

            claudius.GetComponent<SpriteSwapper>().ChangeExpressions(3);
            dialogueManager.RegisterDialogue("Tell me something, Felix. You know what's coming, don't you?", "Claudius");
            dialogueManager.RegisterDialogue("Yes, it's inevitable. Though that doesn't make it any less terrifying.", "Claudius");
            showedDialog = currentDialog;
            return;
        }


        if (currentDialog == 39)
        {
            ShakeObjects(10f, 1.5f);
            AudioManager.Instance.PlaySFX("explosion", 0.10f);
            dialogueManager.RegisterDialogue("Tell me something, Felix. You know what's coming, don't you?", "Claudius");
            dialogueManager.RegisterDialogue("Yes, it's inevitable. Though that doesn't make it any less terrifying.", "Claudius");
            dialogueManager.RegisterDialogue("Believe it or not, I'm a lot older than I look. I've lived a long life and yet... the concept of 'the end' still frightens me.", "Claudius");
            dialogueManager.RegisterDialogue("The worst part is, all we can do is wait.", "Claudius");
            dialogueManager.RegisterDialogue("One more please and make it a double.", "Claudius");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 40)
        {
            drinkManager.ResetDrinkManager();
            drinkManager.EnableDrink(true);
            drinkManager.GetDrinkMenu().EnableButton(true);
            shaker.SetActive(true);
            shaker.GetComponent<CanvasGroup>().DOFade(1.0f, 0.45f);
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 41)
        {
            ShakeObjects(14f, 3f);
            AudioManager.Instance.PlaySFX("explosion", 0.40f);
            claudius.GetComponent<SpriteSwapper>().ChangeExpressions(1);
            dialogueManager.RegisterDialogue("You are very quiet, even at a time like this. Don't you have any questions you'd like to ask me?", "Claudius");
            dialogueManager.RegisterDialogue("Why do I still smile? Hmm… It's because I want my last moments in this world to be happy ones. Regardless of our hopeless situation.", "Claudius");
            showedDialog = currentDialog;
            return;
        }


        if (currentDialog == 42)
        {

            dialogueManager.RegisterDialogue("Anything else? You must be wondering what this machine is for… and how I got these scars?", "Claudius");
            dialogueManager.RegisterDialogue("Well, I suppose I can tell you. After you make me another drink.", "Claudius");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 43)
        {
            drinkManager.ResetDrinkManager();
            drinkManager.EnableDrink(true);
            drinkManager.GetDrinkMenu().EnableButton(true);
            shaker.SetActive(true);
            shaker.GetComponent<CanvasGroup>().DOFade(1.0f, 0.45f);
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 44)
        {
            claudius.GetComponent<SpriteSwapper>().ChangeExpressions(2);
            dialogueManager.RegisterDialogue("Don't hold back. It may well be my last.", "Claudius");
            dialogueManager.RegisterDialogue("Say, Felix, why don't you drink this one with me?", "Claudius");
            dialogueManager.RegisterDialogue("The ground shakes beneath my feet, and the liquid in our glasses spills a little.");
            dialogueManager.RegisterDialogue("It's happening… I guess I got here too late. That's too bad...", "Claudius");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 45)
        {
            ShakeObjects(5f, 2f);
            AudioManager.Instance.PlaySFX("explosion", 0.50f);
            dialogueManager.RegisterDialogue("Blasted machine! I had to drag it all the way here.", "Claudius");
            dialogueManager.RegisterDialogue("A toast to us having met, on the cusp of the world's end.", "Claudius");
            dialogueManager.RegisterDialogue("I'll see you on the other side, my friend.", "Claudius");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 46)
        {
            AudioManager.Instance.SetMusicVolume(0);
            screenAlpha.DOFade(1.0f, 5f);
            ShakeObjects(5f, 2f);
            AudioManager.Instance.PlaySFX("explosion", 0.50f);
            dialogueManager.RegisterDialogue("This is it then.");
            dialogueManager.RegisterDialogue("It's truly the end of the world.");
            dialogueManager.RegisterDialogue("The end of everything I know and love.");
            dialogueManager.RegisterDialogue("Then why do I feel so numb?");
            showedDialog = currentDialog;
            return;
        }

        if (currentDialog == 47)
        {
            showedDialog = currentDialog;
            StartCoroutine(Wait(1f));
            return;
        }


        if (currentDialog == 48)
        {
            dialogueManager.RegisterDialogue("+Thank you for playing.+");
            return;
        }
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        DialogEnded();
    }

    private IEnumerator FadeMusic(float time)
    {
        float timeElapsed = 0.0f;
        do
        {
            timeElapsed += Time.deltaTime;
            AudioManager.Instance.SetMusicVolume(time/timeElapsed);
            yield return new WaitForFixedUpdate();
        } while (timeElapsed > 0.0f);
    }

    public void DialogEnded(int increment = 1)
    {
        currentDialog += increment;
    }
    public int GetCurrentDialog()
    {
         return currentDialog;
    }

    public void ShakeObjects(float magnitude, float time)
    {
        foreach (RectTransform obj in shakableObjects)
        {
            if (obj.gameObject.activeInHierarchy)
            {
                obj.gameObject.AddComponent<Shake>();
                obj.gameObject.GetComponent<Shake>().SetUp(magnitude, time);
            }
        }
    }
}
