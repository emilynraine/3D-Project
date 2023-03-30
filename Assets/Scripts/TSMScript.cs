using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TSMScript : MonoBehaviour
{
    //AudioSource _titleAudioSource;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("DemoScene");
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
