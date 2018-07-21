using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    public static GameUI Instance { get; private set; }


    public Text text;


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
}
