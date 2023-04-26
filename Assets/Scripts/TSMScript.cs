using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TSMScript : MonoBehaviour
{
    bool _isLoading = false;
    public Text _loadText;
    public Text _percentText;
    AsyncOperation _loadOp;
    public GameObject _panel;
    public Image _img;

    void Start()
    {
        _panel.SetActive(false);
        _img = _panel.GetComponent<Image>();
    }

    void Update()
    {
        
    }

    public void OnPlayButtonClick()
    {
        _panel.SetActive(true);
        if (!_isLoading)
        {
            _isLoading = true;
            _loadText.text = "Loading...";
            StartCoroutine(Fade());
            StartCoroutine(LoadScene());
        }
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2f);

        _loadOp = SceneManager.LoadSceneAsync(1);

        while (!_loadOp.isDone)
        {
            _percentText.text = (_loadOp.progress * 100).ToString("F0") + "%";
            yield return null;

        }
    }

    IEnumerator Fade()
    {
        while (_img.color.a < 1)
        {
            Color c = _img.color;
            c.a += 0.005f;
            _img.color = c;
            yield return null;
        }
    }
}
