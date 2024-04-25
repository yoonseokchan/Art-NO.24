using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Change1 : MonoBehaviour
{
    public void ChangeSceneBTn()
    {
        SceneManager.LoadScene("TextScene");
    }
}
