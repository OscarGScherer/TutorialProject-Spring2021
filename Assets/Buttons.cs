using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Buttons : MonoBehaviour
{
    public GameObject gm;

    public GameObject controls;
    private bool sc = false;

    public GameObject errorText;

    public void Restart()
    {
        int wdth = gm.GetComponent<GenerateRoom>().width * 2 + 1;
        int hgth = gm.GetComponent<GenerateRoom>().height * 2 + 1;
        int enem = gm.GetComponent<GameManager>().numOfEnemies;
        int obj = gm.GetComponent<GameManager>().numOfObjects;

        if(wdth*hgth - 1 < enem + obj)
        {
            errorText.GetComponent<TextDisplay>().DisplayTxt("Room too small to fit all enemies and boxes");
        }
        else
        {
            errorText.GetComponent<TextDisplay>().DisplayTxt("");
            gm.GetComponent<GameManager>().Restart();
        }

    }

    public void SetWidth(float num)
    {
        int width = (int)num;
        gm.GetComponent<GenerateRoom>().width = width;
    }

    public void SetHeight(float num)
    {
        int height = (int)num;
        gm.GetComponent<GenerateRoom>().height = height;
    }

    public void SetNumEnem(float num)
    {
        int intNum = (int)num;
        gm.GetComponent<GameManager>().numOfEnemies = intNum;
    }

    public void SetNumObs(float num)
    {
        int intNum = (int)num;
        gm.GetComponent<GameManager>().numOfObjects = intNum;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ShowControls()
    {
        if (sc)
        {
            controls.GetComponent<TextMeshProUGUI>().enabled = false;
            sc = false;
        }
        else
        {
            controls.GetComponent<TextMeshProUGUI>().enabled = true;
            sc = true;
        }
    }
}
