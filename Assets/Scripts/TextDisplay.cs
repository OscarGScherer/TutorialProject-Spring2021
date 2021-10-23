using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    
    public void DisplayTxt(string txt)
    {
        tmp.SetText(txt);
    }
}
