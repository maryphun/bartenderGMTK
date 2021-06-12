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
    }

    public void OnClickMix()
    {
        drinkManager.Mix();
    }

    public void OnClickFairiesKiss()
    {
        drinkManager.FillShaker(0.1f * Time.deltaTime);
        drinkManager.ChangeParameter(0.05f * Time.deltaTime, -0.05f * Time.deltaTime, 0, 0.01f * Time.deltaTime, -0.01f * Time.deltaTime);
    }

    public void OnClickBlackMagic()
    {
        drinkManager.FillShaker(0.1f * Time.deltaTime);
        drinkManager.ChangeParameter(-0.05f * Time.deltaTime, 0.05f * Time.deltaTime, -0.005f * Time.deltaTime, -0.025f * Time.deltaTime, -0.01f * Time.deltaTime);
    }

    public void OnClickGoblinPiss()
    {
        drinkManager.FillShaker(0.1f * Time.deltaTime);
        drinkManager.ChangeParameter(0.01f * Time.deltaTime, -0.01f * Time.deltaTime, 0.05f * Time.deltaTime, 0.012f * Time.deltaTime, 0.012f * Time.deltaTime);
    }

    public void OnClickCrystalMaiden()
    {
        drinkManager.FillShaker(0.1f * Time.deltaTime);
        drinkManager.ChangeParameter(0, 0, 0, 0.05f * Time.deltaTime, -0.01f * Time.deltaTime);
    }

    public void OnClickOdachi()
    {
        drinkManager.FillShaker(0.1f * Time.deltaTime);
        drinkManager.ChangeParameter(-0.05f * Time.deltaTime, -0.01f * Time.deltaTime, 0, -0.01f * Time.deltaTime, 0.05f * Time.deltaTime);
    }

    public void OnClickLime()
    {

    }

    public void OnClickLemon()
    {

    }

    public void OnClickMint()
    {

    }

    public void OnClickColoredIceCube()
    {

    }

    public void OnClickPoison()
    {

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
        }
    }
}
