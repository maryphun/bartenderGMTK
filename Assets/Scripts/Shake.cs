using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    private bool isSetUpDone = false;
    float time, timeElapse, magnitude;
    RectTransform rectT;
    Vector2 originalPos;

    int[] dice = new int[2];

    private void FixedUpdate()
    {
        if (!isSetUpDone)
        {
            enabled = false;
            return;
        }

        if (time > timeElapse)
        {
            timeElapse += Time.deltaTime;
            int f = dice[Random.Range(0, 2)];
            int s = dice[Random.Range(0, 2)];
            float newMag = Mathf.Lerp(magnitude, 0.0f, timeElapse / time);
            rectT.anchoredPosition = originalPos + new Vector2(newMag * f, newMag * s);
        }
        else
        {
            rectT.anchoredPosition = originalPos;
            Destroy(this);
        }
    }

    public void SetUp(float mag, float t)
    {
        magnitude = mag;
        timeElapse = 0.0f;
        time = t;
        dice[0] = -1;
        dice[1] = 1;
        rectT = GetComponent<RectTransform>();
        originalPos = rectT.anchoredPosition;
        isSetUpDone = true;
        enabled = true;
    }
}
