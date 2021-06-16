using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DrinkMenu : MonoBehaviour
{
    private DrinkManager drinkManager;
    private string windowName, otherWindowName;

    private void Start()
    {
        drinkManager = FindObjectOfType<DrinkManager>();
    }

    public void OnClickChange()
    {
        drinkManager.EnableDrink(false, windowName);
        drinkManager.EnableDrink(true, otherWindowName);
        StartCoroutine(Change());
        AudioManager.Instance.PlaySFX("onclick", 0.5f); 
    }

    public void OnClickMix()
    {
        drinkManager.Mix();
        AudioManager.Instance.PlaySFX("onclick", 0.5f);
    }

    public void OnClickFairiesKiss(bool boolean = true)
    {
        drinkManager.waterSmall.mute = !boolean;
        if (boolean)
        {
            drinkManager.FillShaker(0.1f * Time.deltaTime);
            drinkManager.ChangeParameter(0.05f * Time.deltaTime, 0, 0, 0, 0);
        }
    }

    public void OnClickBlackMagic(bool boolean = true)
    {
        drinkManager.waterBig.mute = !boolean;
        if (boolean)
        {
            drinkManager.FillShaker(0.1f * Time.deltaTime);
            drinkManager.ChangeParameter(0, 0.05f * Time.deltaTime, 0, 0, 0);
        }
    }

    public void OnClickGoblinPiss(bool boolean = true)
    {
        drinkManager.waterSmall.mute = !boolean;
        if (boolean)
        {
            drinkManager.FillShaker(0.1f * Time.deltaTime);
            drinkManager.ChangeParameter(0, 0, 0.05f * Time.deltaTime, 0, 0);
        }
    }

    public void OnClickCrystalMaiden(bool boolean = true)
    {
        drinkManager.waterShower.mute = !boolean;
        if (boolean)
        {
            drinkManager.FillShaker(0.1f * Time.deltaTime);
            drinkManager.ChangeParameter(0, 0, 0, 0.05f * Time.deltaTime, 0);
        }
    }

    public void OnClickOdachi(bool boolean = true)
    {
        drinkManager.waterBig.mute = !boolean;
        if (boolean)
        {
            drinkManager.FillShaker(0.1f * Time.deltaTime);
            drinkManager.ChangeParameter(0, 0, 0, 0, 0.05f * Time.deltaTime);
        }
    }

    public void OnClickLime()
    {
        if (drinkManager.AddedIngredient(1, 0, 0, 0, 0))
        {
            StartCoroutine(FillShakerOverTime(0.1f, 0.2f));
            AudioManager.Instance.PlaySFX("mint", 0.3f);
        }
    }

    public void OnClickLemon()
    {
        if (drinkManager.AddedIngredient(0, 1, 0, 0, 0))
        {
            StartCoroutine(FillShakerOverTime(0.15f, 0.2f));
            AudioManager.Instance.PlaySFX("mint", 0.3f);
        }
    }

    public void OnClickMint()
    {
        if (drinkManager.AddedIngredient(0, 0, 1, 0, 0))
        {
            StartCoroutine(FillShakerOverTime(0.15f, 0.2f));
            AudioManager.Instance.PlaySFX("mint", 0.4f);
        }
    }

    public void OnClickColoredIceCube()
    {
        if (drinkManager.AddedIngredient(0, 0, 0, 1, 0))
        {
            StartCoroutine(FillShakerOverTime(0.2f, 0.25f));
            AudioManager.Instance.PlaySFX("IceCube", 0.5f);
        }
    }

    public void OnClickPoison()
    {
        if (drinkManager.AddedIngredient(0, 0, 0, 0, 1))
        {
            StartCoroutine(FillShakerOverTime(0.4f, 0.5f));
            AudioManager.Instance.PlaySFX("poison", 0.5f);
        }
    }
    private IEnumerator FillShakerOverTime(float amount, float time, string playSE = "")
    {
        float timeElapsed = 0.0f;
        do
        {
            if (!drinkManager.FillShaker(amount / time * Time.deltaTime))
            {
                timeElapsed = time;
            };
            AudioManager.Instance.PlaySFX(playSE);
            timeElapsed += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        } while (timeElapsed < time);
    }


    private IEnumerator Change()
    {
        WindowManager.Instance.GetWindowObject(windowName).transform.DOScale(0.95f, 0.5f);

        yield return new WaitForSeconds(0.5f);

        //swap
        Window tmp = WindowManager.Instance.GetWindowObject(windowName);
        tmp.GetComponent<RectTransform>().DOAnchorPosX(drinkManager.GetPos().x + drinkManager.GetSize().x, 0.5f);

        tmp = WindowManager.Instance.GetWindowObject(otherWindowName);
        tmp.GetComponent<RectTransform>().DOAnchorPosX(drinkManager.GetPos().x, 0.5f);
        AudioManager.Instance.PlaySFX("wind", 0.4f);

        yield return new WaitForSeconds(0.5f);

        WindowManager.Instance.GetWindowObject(windowName).transform.DOScale(1.0f, 0.0f);
    }

    public void Setup(string name, DrinkManager managerReference)
    {
        drinkManager = managerReference;
        windowName = name;
        otherWindowName = drinkManager.GetOtherName(name);
    }

    public void EnableButton(bool boolean)
    {
        var tmp = FindObjectsOfType<Button>();
        foreach (Button btn in tmp)
        {
            btn.interactable = boolean;
            OnHoldButton comp = btn.GetComponent<OnHoldButton>();
            if (comp != null)
            {
                comp.SetEnable(boolean);
            }
        }

        if (!boolean)
        {
            drinkManager.waterSmall.mute = true;
            drinkManager.waterBig.mute = true;
            drinkManager.waterShower.mute = true;
        }
    }

    public bool IsEnableButton()
    {
        var tmp = FindObjectsOfType<Button>();
        foreach (Button btn in tmp)
        {
           if ( btn.interactable )
            {
                return true;
            }
        }

        return false;
    }
}
