using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public MSManagerScript _manager;

    // Start is called before the first frame update
    void Start()
    {
       // _manager = FindObjectOfType<MSManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_manager._paused)
        {
            SkipWhilePaused();
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
       
        DoWhilePaused();
    }

    protected virtual void DoWhilePaused()
    {

    }

    protected virtual void SkipWhilePaused()
    {

    }
}
