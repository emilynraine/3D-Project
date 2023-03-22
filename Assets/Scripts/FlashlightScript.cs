using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    AudioSource _flashlightSource;
    public AudioClip _clickSound;
    Light _light;
    bool _on = true;
    // Start is called before the first frame update
    void Start()
    {
        _flashlightSource = GetComponent<AudioSource>();
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)) 
        {
            PlayClick();
            if(_on) 
            {
                _light.enabled = false;
                _on = false;
            } else 
            {
                _light.enabled = true;
                _on = true;
            }
        }
    }

    public void PlayClick()
    {
        _flashlightSource.PlayOneShot(_clickSound);
    }
}
