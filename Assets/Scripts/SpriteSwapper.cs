using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwapper : MonoBehaviour
{
    [SerializeField] Sprite[] presets;

    public void ChangeExpressions(int integer)
    {
        if (presets[integer] != null)
        {
            GetComponent<Image>().sprite = presets[integer];
        }
    }
}
