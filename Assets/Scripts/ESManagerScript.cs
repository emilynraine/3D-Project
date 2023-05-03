using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESManagerScript : MonoBehaviour
{
    public GameObject _blackoutSquare;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        StartCoroutine(BlackOut(false));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void OnRestartButtonClick()
    {
        print("Restart");
        StartCoroutine(BlackOut(true));
        Invoke("MainScene", 3f);
    }
    public void OnQuitButtonClick()
    {
        print("MainMenu");
        StartCoroutine(BlackOut(true));
        Invoke("TitleScene", 3f);
    }

    public void MainScene()
    {
        SceneManager.LoadScene("DemoScene");
    }

    public void TitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public IEnumerator BlackOut(bool fadeToBlack, float fadeSpeed = .5f)
    {
        Color objectColor = _blackoutSquare.GetComponent<Image>().color;
        float fadeAmount;

        if(fadeToBlack)
        {
            while(_blackoutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.b, objectColor.g, fadeAmount);
                _blackoutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }

        if(!fadeToBlack)
        {
            while(_blackoutSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.b, objectColor.g, fadeAmount);
                _blackoutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
    }
}
