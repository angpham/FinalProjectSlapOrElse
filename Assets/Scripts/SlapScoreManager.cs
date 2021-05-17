using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlapScoreManager : MonoBehaviour
{
    public void updateSlapScoreText(int slapScore)
    {
        GetComponent<Text>().text = "SlapScore: " + slapScore;
    }
}
