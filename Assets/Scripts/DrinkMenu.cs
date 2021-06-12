using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
