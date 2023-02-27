using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionScript : MonoBehaviour
{
    
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt(ConstantData.SaveControl,0)==0)
        {
            gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, 450);
        }
        else
        {
            gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 450);
        }
    }
}
