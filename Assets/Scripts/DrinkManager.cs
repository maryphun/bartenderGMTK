using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DrinkManager : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] string windowNameBase;
    [SerializeField] string windowNameLayer;
    [SerializeField, Range(0.0f, 1.0f)] float alphaAfterDisable = 0.15f;
    [SerializeField, Range(0.0f, 1.0f)] float liquidFillPercentageMin = 0.9f;

    [Header("References")]
    [SerializeField] GameObject buttonsBase;
    [SerializeField] GameObject buttonsLayer;
    [SerializeField] GameStateManager gameStateManager;
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] DragObject shakerUI;
    [SerializeField] RectTransform liquidFill;
    [SerializeField] public AudioSource waterSmall, waterBig, waterShower;

    [Header("Debug")]
    [SerializeField] private float liquidFillPercentage;
    [SerializeField] private float _happiness;
    [SerializeField] private float _sadness;
    [SerializeField] private float _surprise;
    [SerializeField] private float _excited;
    [SerializeField] private float _anger;
    [SerializeField] private int _lime;
    [SerializeField] private int _lemon;
    [SerializeField] private int _mint;
    [SerializeField] private int _icecube;
    [SerializeField] private int _poison;

    // privates
    private string currentWindowName;
    private float liquidCount, colorLerp;
    private Color[] liquidLayer = new Color[4];
    private Color[] previousColor = new Color[4];
    private bool mixTipMessageShown, mixTutorialMessageShown, showingSuccessfulProduct;

    public void Initialization()
    {
        // initialize pos and size
        Vector2 size = GetSize();
        Vector2 pos = GetPos();

        // create window
        WindowManager.Instance.CreateWindow(windowNameBase, pos, size);
        WindowManager.Instance.CreateWindow(windowNameLayer, new Vector2(pos.x + size.x, pos.y), size);

        // setup buttons
        GameObject[] tmp = new GameObject[2];
        tmp[0] = Instantiate(buttonsBase, WindowManager.Instance.GetWindowObject(windowNameBase).transform);
        tmp[0].GetComponent<DrinkMenu>().Setup(windowNameBase, this);

        tmp[1] = Instantiate(buttonsLayer, WindowManager.Instance.GetWindowObject(windowNameLayer).transform);
        tmp[1].GetComponent<DrinkMenu>().Setup(windowNameLayer, this);

        // set default
        currentWindowName = windowNameBase;
        liquidFillPercentage = 0.0f;
        mixTipMessageShown = false;
        mixTutorialMessageShown = false;
        showingSuccessfulProduct = false;
    }

    public Vector2 GetSize()
    {
        return new Vector2(Screen.width / 5.25f, Screen.height);
    }
    public Vector2 GetPos()
    {
        return new Vector2((Screen.width / 2f) - GetSize().x / 2f, (Screen.height / 2f) - GetSize().y / 2f);
    }

    public void OpenWindow(bool boolean, string name = "")
    {
        if (name == "")
        {
            // affect both
            if (boolean)
            {
                WindowManager.Instance.Open(windowNameBase, 0.0f);
                WindowManager.Instance.Open(windowNameLayer, 0.0f);
            }
            else
            {
                WindowManager.Instance.Close(windowNameBase, 0.0f, false);
                WindowManager.Instance.Close(windowNameLayer, 0.0f, false);
            }
            return;
        }

        if (boolean)
        {
            WindowManager.Instance.Open(name, 0.0f);
        }
        else
        {
            WindowManager.Instance.Close(name, 0.0f, false);
        }
    }

    public void EnableDrink(bool boolean, string name = "default", float time = 0.45f)
    {
        if (name == "default")
        {
            name = windowNameBase;
        }

        Window window = WindowManager.Instance.GetWindowObject(name);
        float alpha = boolean ? 1.0f : alphaAfterDisable;

        Transform tmp = window.transform.Find("Alpha");
        if (tmp == null)
        {
            Debug.Log("Alpha not found!");
        }

        if (boolean)
        {
            window.transform.SetSiblingIndex(1);
            WindowManager.Instance.GetWindowObject(GetOtherName(name)).transform.SetSiblingIndex(0);
        }

        tmp.SetSiblingIndex(window.transform.childCount);
        tmp.GetComponent<Image>().DOFade(1f-alpha, time);
    }

    public string GetOtherName(string name)
    {
        string rtn = "";

        if (name == windowNameBase)
        {
            rtn = windowNameLayer;
        }
        if (name == windowNameLayer)
        {
            rtn = windowNameBase;
        }

        return rtn;
    }

    public void Mix()
    {
        if (liquidFillPercentage >= liquidFillPercentageMin)
        {
            GetDrinkMenu().EnableButton(false);
            EnableDrink(false);
            // TODO: add shaker cover
            shakerUI.SetMixingMode(true);

            if (!mixTutorialMessageShown)
            {
                mixTutorialMessageShown = true;
                dialogueManager.RegisterDialogue("Good timing! Now %shake% your cocktail to chill the drink and add dilution.","",false);
                dialogueManager.RegisterDialogue("Remember, consistency and control are very important in this case.", "", false);
            }
        }
        else
        {
            // not enough.
            if (!dialogueManager.IsShowingDialogue())
            {
                // Add dialog if possible.
                string[] randomString = new string[3];
                randomString[0] = "Not enough ingredients, add more!";
                randomString[1] = "The drink is not finish yet.";
                randomString[2] = "No, not yet.";
                dialogueManager.RegisterDialogue(randomString[Random.Range(0, randomString.Length-1)], "", false);

                AudioManager.Instance.PlaySFX("fail");
            }
        }
    }

    public void ChangeParameter(float happy, float sad, float surprise, float excited, float anger)
    {
        _happiness = Mathf.Clamp(_happiness + happy, 0.0f, 1.0f);
        _sadness = Mathf.Clamp(_sadness + sad, 0.0f, 1.0f);
        _surprise = Mathf.Clamp(_surprise + surprise, 0.0f, 1.0f);
        _excited = Mathf.Clamp(_excited + excited, 0.0f, 1.0f);
        _anger = Mathf.Clamp(_anger + anger, 0.0f, 1.0f);
    }

    public void ResetParameter()
    {
        _happiness = 0f;
        _sadness = 0f;
        _surprise = 0f;
        _excited = 0f;
        _anger = 0f;
    }

    public bool AddedIngredient(int lime, int lemon, int mint, int ice, int poison)
    {
        bool rtn;

        _lime += lime;
        _lemon += lemon;
        _mint += mint;
        _icecube += ice;
        _poison += poison;

        if (_lime > 1 || _lemon > 1|| _mint > 1 || _icecube > 1 || _poison > 1)
        {
            rtn = false;
            // TODO: play SE
            if (!dialogueManager.IsShowingDialogue())
            {
                // Add dialog if possible.
                string[] randomString = new string[2];
                randomString[0] = "You don't need too much of this.";
                randomString[1] = "That is too much.";
                dialogueManager.RegisterDialogue(randomString[Random.Range(0, randomString.Length)], "", false);
            }
        }
        else
        {
            rtn = true;
        }

        _lime = Mathf.Clamp(_lime, 0, 1);
        _lemon = Mathf.Clamp(_lemon, 0, 1);
        _mint = Mathf.Clamp(_mint, 0, 1);
        _icecube = Mathf.Clamp(_icecube, 0, 1);
        _poison = Mathf.Clamp(_poison, 0, 1);

        return rtn;
    }

    private void ResetIngredient()
    {
        _lime = 0;
        _lemon = 0;
        _mint = 0;
        _icecube = 0;
        _poison = 0;
    }

    public bool FillShaker(float value)
    {
        liquidFillPercentage = Mathf.Clamp(liquidFillPercentage + value, 0.0f, 1.0f);
        colorLerp = 0.5f;
        liquidCount = 0.0f;

        if (liquidFillPercentage == 1.0f)
        {
            gameStateManager.ShakeObjects(15f, 1f);
            if (!dialogueManager.IsShowingDialogue())
            {
                // Add dialog if possible.
                string[] randomString = new string[4];
                randomString[0] = "That would not work, let's make one more.";
                randomString[1] = "Try again.";
                randomString[2] = "They won't like this.";
                randomString[3] = "Too strong!";
                dialogueManager.RegisterDialogue(randomString[Random.Range(0, randomString.Length - 1)], "", false);
            }
            liquidFillPercentage = 0.0f;
            ResetParameter();
            ResetIngredient();
            for (int i = 0; i < liquidLayer.Length; i++) liquidLayer[i] = Color.black;

            AudioManager.Instance.PlaySFX("glassbreak", 0.6f);
            AudioManager.Instance.PlaySFX("explosion", 0.2f);
            waterSmall.mute = true;
            waterBig.mute = true;
            waterShower.mute = true;

            return false;
        }

        if (!mixTipMessageShown)
        {
            if (liquidFillPercentage >= 0.75f)
            {
                dialogueManager.RegisterDialogue("Your drink is about to finish, how about adding an ice cube for the last touch? Don't over do it.", "", false);
                dialogueManager.RegisterDialogue("When you feel like it's ready, it is time to +mix+ it.", "", false);
                mixTipMessageShown = true;
            }
        }

        return true;
    }

    public void ResetDrinkManager()
    {
        liquidFillPercentage = 0.0f;
        ResetParameter();
        ResetIngredient();
        for (int i = 0; i < liquidLayer.Length; i++) liquidLayer[i] = Color.black;
        waterSmall.mute = true;
        waterBig.mute = true;
        waterShower.mute = true;
    }

    public DrinkMenu GetDrinkMenu(string name = "")
    {
        if (name == "")
        {
            name = currentWindowName;
        }

        return WindowManager.Instance.GetWindowObject(name).GetComponentInChildren<DrinkMenu>();
    }

    private void Update()
    {
        // key input
        if (showingSuccessfulProduct)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (WindowManager.Instance.IsTypeWriting("Success"))
                {
                    // typewriting in effect. skip it.
                    WindowManager.Instance.SkipTextWriter("Success");
                }
                else
                {
                    // typewriter is ended. close this dialogue

                    WindowManager.Instance.Close("Success", 0.45f, true);

                    gameStateManager.DialogEnded();

                    showingSuccessfulProduct = false;
                }
            }
        }

        // control fill
        float fillSize = 255.55f;
        liquidCount += Random.Range(-0.5f, 2.0f) * Time.deltaTime;
        float fill = liquidFillPercentage + ((Mathf.Min(liquidFillPercentage * 0.01f, 0.05f)) * Mathf.Sin(liquidCount));

        liquidFill.sizeDelta = new Vector2(liquidFill.sizeDelta.x, Mathf.Min(fillSize, fill * fillSize));
        liquidFill.anchoredPosition = new Vector2(0, (-fillSize / 2f) + Mathf.Min(fillSize/2f, fill * fillSize /2f));

        ParameterRatio(out float hap, out float sad, out float surp, out float excite, out float anger);

        //Debug.Log("<color=green>Happy:</color> " + hap.ToString());
        //Debug.Log("<color=cyan>Sad:</color> " + sad.ToString());
        //Debug.Log("<color=blue>Surprise:</color> " + surp.ToString());
        //Debug.Log("<color=yellow>Excited:</color> " + excite.ToString());
        //Debug.Log("<color=lightblue>Anger:</color> " + anger.ToString());

        float red = Mathf.Clamp((hap/2f) + (anger/1.5f) - (surp / 8f) - (sad / 8f) - (excite / 8f), 0f, 1f);
        float green = Mathf.Clamp((excite/2f) + (surp/1.5f) - (anger / 4f) - (hap / 4f), 0f, 1f);
        float blue = Mathf.Clamp((sad/1.5f) + (excite/2f) - (hap / 4f) - (anger / 4f), 0f, 1f);

        if (liquidFillPercentage < 0.25f || _lime == 1 || _lemon == 1 || _mint == 1 || _icecube == 1 || _poison == 1)
        {
            liquidLayer[0] = new Color(red, green, blue);
            liquidLayer[1] = new Color(red, green, blue);
            liquidLayer[2] = new Color(red, green, blue);
            liquidLayer[3] = new Color(red, green, blue);
        }
        else if (liquidFillPercentage < 0.5f)
        {
            liquidLayer[1] = new Color(red, green, blue);
            liquidLayer[2] = new Color(red, green, blue);
            liquidLayer[3] = new Color(red, green, blue);
        }
        else if (liquidFillPercentage < 0.75f)
        {
            liquidLayer[2] = new Color(red, green, blue);
            liquidLayer[3] = new Color(red, green, blue);
        }
        else if (_lime == 1 && _lemon == 1 && _mint == 1 && _icecube == 1 && _poison == 1)
        {
            liquidLayer[3] = new Color(red, green, blue);
        }
        
        for (int i = 0; i < 4; i++) previousColor[i] = new Color(1f, 1f, 1f, 1.0f);
        Color topleft = Color.Lerp(previousColor[0], liquidLayer[3], 1f);
        Color topright = Color.Lerp(previousColor[1], liquidLayer[2] + liquidLayer[3], 1f);
        Color btmleft = Color.Lerp(previousColor[2], (liquidLayer[0] * (1 - liquidFillPercentage)) + liquidLayer[1] + liquidLayer[2] + liquidLayer[3], 1f);
        Color btmright = Color.Lerp(previousColor[3], liquidLayer[1] + liquidLayer[2] + liquidLayer[3], 1f);

        liquidFill.GetComponent<Image>().material.SetColor("_GradTopLeftCol", topleft);
        liquidFill.GetComponent<Image>().material.SetColor("_GradTopRightCol", topright);
        liquidFill.GetComponent<Image>().material.SetColor("_GradBotLeftCol", btmleft);
        liquidFill.GetComponent<Image>().material.SetColor("_GradBotRightCol", btmright);

        previousColor[0] = topleft;
        previousColor[1] = topright;
        previousColor[2] = btmleft;
        previousColor[3] = btmright;
    }

    public void ParameterRatio(out float hap, out float sad, out float surp, out float excite, out float anger)
    {
        hap = Mathf.Clamp(_happiness / (_happiness + _sadness + _surprise + _excited + _anger), 0f, 1f);
        sad = Mathf.Clamp(_sadness / (_happiness + _sadness + _surprise + _excited + _anger), 0f, 1f);
        surp = Mathf.Clamp(_surprise / (_happiness + _sadness + _surprise + _excited + _anger), 0f, 1f);
        excite = Mathf.Clamp(_excited / (_happiness + _sadness + _surprise + _excited + _anger), 0f, 1f);
        anger = Mathf.Clamp(_anger / (_happiness + _sadness + _surprise + _excited + _anger), 0f, 1f);
    }

    public void DrinkSuccess()
    {
        Vector2 size = new Vector2(700f, 600f);
        string drinkname = "";
        Color nameColor = Color.white;
        string imageName = "";
        string description = "";

        WindowManager.Instance.CreateWindow("Success", new Vector2(0.0f, 0.0f), size);
        WindowManager.Instance.Open("Success", 2f);

        // determine drink
        ParameterRatio(out float hap, out float sad, out float surp, out float excite, out float anger);
        if (hap > 0.8f && _icecube == 1)
        {
            drinkname = "Iced Dragonica";
            nameColor = Color.magenta;
            imageName = "01";
            description = "People drink this during the celebration of traditional Dragonica ceremony.";
        }
        else if (hap > 0.75f && NoLayer())
        {
            drinkname = "Fairies Kiss";
            nameColor = Color.red;
            imageName = "01";
            description = "Sometimes you get lazy too. But the good thing is it does actually taste good.";
        }
        else if (sad > 0.75f && NoLayer())
        {
            drinkname = "Black Magic";
            nameColor = Color.black;
            imageName = "01";
            description = "When you're in a depression, you lost interest of making a good cocktail.";
        }
        else if (surp > 0.75f && NoLayer())
        {
            drinkname = "Goblin Piss";
            nameColor = Color.yellow;
            imageName = "01";
            description = "what a surprise, your customer surely are not ready for it.";
        }
        else if (excite > 0.75f && NoLayer())
        {
            drinkname = "Crystal Maiden";
            nameColor = Color.cyan;
            imageName = "01";
            description = "Are you really serving this?";
        }
        else if (anger > 0.75f && NoLayer())
        {
            drinkname = "Ōdachi";
            nameColor = Color.white;
            imageName = "01";
            description = "A traditional primary liquid comes from the ancient east.";
        }
        else if (hap + sad + anger + excite + surp == 0.0f)
        {
            drinkname = "Mocktail";
            nameColor = Color.gray;
            imageName = "01";
            description = "Also known as non-alcohol softdrink.";
        }
        else if (hap > 0.08f && sad > 0.2f && excite > 0.2f && anger > 0.25f && !NoLayer())
        {
            drinkname = "Angry Bird";
            nameColor = Color.red;
            imageName = "01";
            description = "One of the modern invented drink, the youngster's favorite.";
        }
        else if (hap > 0.4f && sad > 0.4f && NoLayer())
        {
            drinkname = "Smiling Depression";
            nameColor = Color.yellow;
            imageName = "01";
            description = "Is it possible to feel happy and sad at the same time?";
        }
        else if (hap > 0.1f && sad > 0.5f && excite > 0.12f)
        {
            drinkname = "Blue Magic";
            nameColor = Color.blue;
            imageName = "01";
            description = "Taste like blue-berry.";
        }
        else if (hap > 0.10f && sad > 0.10f && surp > 0.15f && excite > 0.10f && anger > 0.10f && _icecube == 1)
        {
            drinkname = "Long Island Ice Tea";
            nameColor = new Color(238, 130, 238);   // violet
            imageName = "01";
            description = "Who know it will taste like a ice tea after you mix everything?";
        }
        else
        {
            string[] Alphabet = new string[26] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            drinkname = "Beverage that Start with " + Alphabet[Random.Range(0, Alphabet.Length)];
            nameColor = Color.gray;
            imageName = "01";
            description = "You invented some new stuff this time.";
        }

        showingSuccessfulProduct = true;
        TMP_Text name = WindowManager.Instance.AddNewText("Success", drinkname, new Vector2(size.x / 2f, size.y / 2f), 72f, nameColor);
        name.alignment = TextAlignmentOptions.TopGeoAligned;
        WindowManager.Instance.AddNewImage("Success", "Drinks/" + imageName, new Vector2(size.x / 2f, size.y / 2f), new Vector2(size.x / 3f, size.x / 3f));
        WindowManager.Instance.SetText("Success", description, 0.1f);
        WindowManager.Instance.SetTextSize("Success", 50f);
        WindowManager.Instance.SetTextAlignment("Success", CustomTextAlignment.bottomCenter);
        WindowManager.Instance.SetTextWrappingMode("Success", true);

        Debug.Log("<color=green>Happy:</color> " + hap.ToString());
        Debug.Log("<color=cyan>Sad:</color> " + sad.ToString());
        Debug.Log("<color=blue>Surprise:</color> " + surp.ToString());
        Debug.Log("<color=yellow>Excited:</color> " + excite.ToString());
        Debug.Log("<color=lightblue>Anger:</color> " + anger.ToString());
    }

    private bool NoLayer()
    {
        return _icecube + _lemon + _lime + _mint + _poison == 0;
    }
}
