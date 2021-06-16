using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameStateManager gameStateManager;
    [SerializeField] DrinkManager drinkManager;

    [Header("Parameters")]
    [SerializeField] string windowName = "DialogueBox";
    [SerializeField, Range(0.0f, 128f)] float nameSize = 64f;
    [SerializeField] Color nameColor = Color.cyan;
    [SerializeField] Vector2 nameOffset = new Vector2(0.0f, -5f);
    [SerializeField, Range(0.0f, 0.5f)] float typeWriterInterval = 0.1f;
    [SerializeField, Range(0.0f, 1f)] float windowOpenSpeed = 0.5f;
    [SerializeField, Range(0.0f, 128f)] float textSize = 64f;
    [SerializeField] Image bartender, gedeon;

    // private
    private Vector2 windowPos, windowSize, nameWindowPos, nameWindowSize, portraitPos, portraitSize, facePos, faceSize;
    private bool isShowingDialogue, isFirstDialogue, isNameWindowClosed, wasEnabledDrinkMenu;
    private int indexCount;
    private string currentDialogueName;
    Dalogue currentDialogData;

    public struct Dalogue
    {
        public string name;
        public string text;
        public string index;

        public string portrait;
        public string face;

        public bool isDialog;
    }

    private List<Dalogue> dialogueList = new List<Dalogue>();

    public void RegisterDialogue(string text, string name = "", bool isDialog = true)
    {
        Dalogue tmp;

        tmp.name = name;
        tmp.text = text;
        tmp.portrait = "";  // canceled features
        tmp.face = ""; // canceled features
        tmp.index = indexCount.ToString();
        tmp.isDialog = isDialog;

        dialogueList.Add(tmp);

        indexCount++;
        if (indexCount == 1)
        {
            isFirstDialogue = true;
            isNameWindowClosed = true;
        }
    }

    public void Initialization()
    {
        dialogueList.Clear();
        windowPos = new Vector2(-Screen.width / 10, (-Screen.height / 2) + Screen.height / 8);
        windowSize = new Vector2(Screen.width / 1.25f, Screen.height / 4);

        nameWindowPos = new Vector2(-Screen.width / 2 + Screen.width / 10, (-Screen.height / 2) + Screen.height / 4 + (nameSize/2f) + 10f);
        nameWindowSize = new Vector2(Screen.width / 5, (nameSize));

        portraitPos = new Vector2(Screen.width / 10, Screen.height / 4 + Screen.width / 10);
        portraitSize = new Vector2(Screen.width / 5, Screen.width / 5);

        facePos = new Vector2(Screen.width / 12, Screen.height / 8);
        faceSize = new Vector2(Screen.width / 8, Screen.width / 8);

        indexCount = 0;
        isFirstDialogue = true;
        isNameWindowClosed = true;
    }

    private void Update()
    {
        if (isShowingDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (WindowManager.Instance.IsTypeWriting(currentDialogueName))
                {
                    // typewriting in effect. skip it.
                    WindowManager.Instance.SkipTextWriter(currentDialogueName);
                }
                else
                { 
                    // typewriter is ended. close this dialogue
                    float closeSpeed = !IsLastDialogue() ?  0.0f : windowOpenSpeed;
                    WindowManager.Instance.Close(currentDialogueName, closeSpeed, true);
                    if (WindowManager.Instance.IsWindowExist(currentDialogueName + "(name)"))
                    {
                        closeSpeed = !ShouldCloseNameWindow() ? 0.0f : windowOpenSpeed;
                        WindowManager.Instance.Close(currentDialogueName + "(name)", closeSpeed, true);

                        // highlight character
                        //HighlightCharacter(currentDialogData.name, 1.0f);
                    }
                    isShowingDialogue = false;

                    if (dialogueList.Count == 0)
                    {
                        if (currentDialogData.isDialog)
                        {
                            StartCoroutine(NextDialogueDelay(windowOpenSpeed));
                        }
                        isFirstDialogue = true;
                        // Enable drink menu
                        if (isFirstDialogue)
                        {
                            drinkManager.GetDrinkMenu().EnableButton(wasEnabledDrinkMenu);
                            drinkManager.EnableDrink(wasEnabledDrinkMenu);
                        }
                    }
                }
            }
        }
        
        if (!isShowingDialogue && dialogueList.Count > 0)    // there are no dialogue showing, and there are registered dialogue left.
        {
            float openSpeed = isFirstDialogue ? windowOpenSpeed : 0.0f;
            // instantiate new window
            currentDialogueName = windowName + dialogueList[0].index;
            WindowManager.Instance.CreateWindow(currentDialogueName, windowPos, windowSize);
            WindowManager.Instance.Open(currentDialogueName, openSpeed);
            // show dialog
            WindowManager.Instance.SetText(currentDialogueName, dialogueList[0].text, typeWriterInterval);
            WindowManager.Instance.SetTextSize(currentDialogueName, textSize);
            WindowManager.Instance.SetTextAlignment(currentDialogueName, CustomTextAlignment.topLeft);
            WindowManager.Instance.SetTextWrappingMode(currentDialogueName, true);
            // show name
            if (dialogueList[0].name.Length > 0)
            {
                openSpeed = isNameWindowClosed ? windowOpenSpeed : 0.0f;
                WindowManager.Instance.CreateWindow(currentDialogueName + "(name)", nameWindowPos, nameWindowSize);
                WindowManager.Instance.Open(currentDialogueName + "(name)", openSpeed);
                WindowManager.Instance.SetText(currentDialogueName + "(name)", dialogueList[0].name, typeWriterInterval);
                WindowManager.Instance.SetTextColor(currentDialogueName + "(name)", nameColor);
                WindowManager.Instance.SetTextSize(currentDialogueName + "(name)", nameSize);
                WindowManager.Instance.SetTextOffset(currentDialogueName + "(name)", nameOffset);
                WindowManager.Instance.SetTextAlignment(currentDialogueName + "(name)", CustomTextAlignment.center);
                WindowManager.Instance.SetTextWrappingMode(currentDialogueName + "(name)", false);
            }
            // show portrait
            if (dialogueList[0].portrait.Length > 0)
            {
                WindowManager.Instance.AddNewImage(currentDialogueName, "Portrait/" + dialogueList[0].portrait, portraitPos, portraitSize, true);
            }
            // show face
            if (dialogueList[0].face.Length > 0)
            {
                WindowManager.Instance.AddNewImage(currentDialogueName, "Face/" + dialogueList[0].face, facePos, faceSize, false);
            }
            // Disable drink menu
            if (isFirstDialogue)
            {
                wasEnabledDrinkMenu = drinkManager.GetDrinkMenu().IsEnableButton();
                drinkManager.GetDrinkMenu().EnableButton(false);
                drinkManager.EnableDrink(false);
            }
            // highlight character
            //HighlightCharacter(dialogueList[0].name, 0.75f);

            // registered dialogue list
            currentDialogData = dialogueList[0];
            dialogueList.RemoveAt(0);
            isShowingDialogue = true;
            isFirstDialogue = false;
        }
    }

    private IEnumerator NextDialogueDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameStateManager.DialogEnded();
    }

    private bool IsLastDialogue()
    {
        return dialogueList.Count == 0;
    }

    private bool ShouldCloseNameWindow()
    {
        if (IsLastDialogue())
        {
            return true;
        }
        else
        {
            // check if next dialogue have a name
            return dialogueList[0].name.Length == 0;
        }
    }

    public bool IsShowingDialogue()
    {
        return isShowingDialogue;
    }

    private void HighlightCharacter(string name, float value)
    {
        if (dialogueList[0].name.Length > 0)
        {
            float val = 1.0f;
            if (dialogueList[0].name != "Felix")
            {
                val = value;
            }
            bartender.DOColor(new Color(val, val, val, bartender.color.a), 0.25f);
            val = 1.0f;
            if (dialogueList[0].name != "???" && dialogueList[0].name != "Gedeon")
            {
                val = value;
            }
            gedeon.DOColor(new Color(val, val, val, gedeon.color.a), 0.25f);
        }
    }
}

