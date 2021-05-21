using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerManager : MonoBehaviour
{ 
    public void updateGameTimerText(float gameTimer)
    {
        if (gameTimer < 0)
        {
            gameTimer = 0; // To ensure it displays 0 and not negative time
        }

        float minutes = Mathf.Floor(gameTimer / 60);
        float seconds = Mathf.Floor(gameTimer % 60);
        float milliseconds = (gameTimer % 1)* 1000;

        string timeText = string.Format("{0:00}.{1:00}.{2:00}", minutes, seconds, milliseconds);
        GetComponent<Text>().text = "Game Timer: " + timeText;
    }
}