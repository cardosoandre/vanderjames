using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VDJ.BuilderGame.GameState;

public class TimerText : MonoBehaviour {
    public SuperTextMesh text;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        text.enabled = StageManager.Instance.TimerStarted;
        if (text.enabled)
        {
            var time = StageManager.Instance.RemainingTime;
            ShowTime(time);
        }
        
	}

    private void ShowTime(float time)
    {
        int asInt = (int)time;

        int minutes = asInt/ 60;
        int seconds = asInt % 60;
        if(minutes == 0)
        {
            text.text = seconds.ToString();
        } else
        {
            text.text = string.Format("{0}:{1}", minutes.ToString(), seconds.ToString());
        }
    }
}
