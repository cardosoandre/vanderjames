using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    public static GameUI Instance { get; private set; }


    public Text text;

    public Transform endPanel;

    public Text EndPanelText;


    private void Awake()
    {
        Instance = this;
    }

    public void DisableCountdown()
    {
        text.gameObject.SetActive(false);
    }

    public void EnableCountdown()
    {
        text.gameObject.SetActive(true);
    }
    public void SetCountdown(int val)
    {
        text.text = val.ToString();
    }

    public void ShowResults(float score)
    {
        endPanel.gameObject.SetActive(true);

        EndPanelText.text = string.Format("Sua pontuação foi {0}", score);
    }

    public void ShowVictory()
    {
        text.gameObject.SetActive(true);
        text.text = "Parabens!";
    }
}
