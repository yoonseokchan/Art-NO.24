using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initalizer : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.Initialize();
        StageGameManager.instance.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
