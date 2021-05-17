using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBackToMenuButtonManager : MonoBehaviour
{
    public void ChangeSceneToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
