using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatusManager : MonoBehaviour
{
    public void updateGameStatus(string text)
    {
        GetComponent<Text>().text = text;
    }
}