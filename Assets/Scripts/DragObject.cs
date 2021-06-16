using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DragObject : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image shakerbottle;
    bool isDragging;
    Vector2 posDif;
    RectTransform rectT;

    [SerializeField] DrinkManager drinkManager;
    bool isMixingMode = false;
    float shakeProgress = 0.0f;
    float soundInterval;
    float cooldown;

    void Update()
    {
        if (!isDragging && !isMixingMode) return;
        Vector2 originalPos = rectT.anchoredPosition;
        if (Input.GetMouseButton(0))
        {
            rectT.anchoredPosition = new Vector2(Mathf.Clamp((-Screen.width / 2f + Input.mousePosition.x) - posDif.x, (-Screen.width / 2f) + rectT.sizeDelta.x / 2f, (Screen.width / 2f) - ((Screen.width / 5.25f) / 2f) - rectT.sizeDelta.x / 2f),
                Mathf.Clamp((-Screen.height / 2f + Input.mousePosition.y) - posDif.y, (-Screen.height / 2f) + rectT.sizeDelta.y / 2f, Screen.height / 2f - rectT.sizeDelta.y / 2f));
        }
        else
        {
            isDragging = false;

            if (!isMixingMode)
            {
                enabled = false;
            }
        }


        // shake
        if (isMixingMode)
        {
            cooldown -= Time.deltaTime;
            float dist = Mathf.Abs(originalPos.x - rectT.anchoredPosition.x);
            if (dist > 40.0f)
            {
                shakeProgress = Mathf.Min(shakeProgress + (0.25f * Time.deltaTime), 1.0f);
                soundInterval -= Time.deltaTime;
                cooldown = 0.15f;
            }
            else if (dist < 40.0f && cooldown <= 0.0f)
            {
                shakeProgress = Mathf.Max(shakeProgress - (0.1f * Time.deltaTime), 0.0f);
                soundInterval = 0.1f;
            }

            if (soundInterval <= 0.0f)
            {
                AudioManager.Instance.PlaySFX("shake" + Random.Range(0, 2).ToString());
                soundInterval = 0.1f;
            }
        }

        slider.value = shakeProgress;

        if (shakeProgress > 0.0f)
        {
            slider.GetComponent<CanvasGroup>().DOFade(1.0f, 0.25f);
        }
        else
        {
            slider.GetComponent<CanvasGroup>().DOFade(0.0f, 0.25f);
        }

        if (shakeProgress == 1)
        {
            SetMixingMode(false);
            AudioManager.Instance.PlaySFX("succeed");

            StartCoroutine(SetActiveDelayLoop(false, 0.5f));
            GetComponent<CanvasGroup>().DOFade(0.0f, 0.5f);
            drinkManager.DrinkSuccess();
            shakerbottle.DOFade(0f, 0.5f);
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        rectT = GetComponent<RectTransform>();
        posDif = new Vector2(-Screen.width/2f +  Input.mousePosition.x, -Screen.height / 2f + Input.mousePosition.y) - rectT.anchoredPosition;

        if (isMixingMode)
        {
            soundInterval = 0.1f;
        }

        enabled = true;
    }

    public void SetMixingMode(bool boolean)
    {
        isMixingMode = boolean;
        shakeProgress = 0.0f;
        slider.gameObject.SetActive(boolean);
        slider.GetComponent<CanvasGroup>().DOFade(0.0f, 0.0f);

        if (boolean)
        {
            shakerbottle.DOFade(1f, 0.5f);
        }
        else
        {
            shakerbottle.DOFade(0f, 0.5f);
        }
    }

    private IEnumerator SetActiveDelayLoop(bool boolean, float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(boolean);
    }
}
