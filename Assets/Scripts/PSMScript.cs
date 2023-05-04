using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PSMScript : PauseScript
{
    Color _selectedBrown = new Color(81 / 255f, 56 / 255f, 23 / 255f, 255 / 255f);
    Color _unselectedTan = new Color(232 / 255f, 213 / 255f, 183 / 255f, 255 / 255f);

    AudioSource _audioSource;
    [SerializeField]
    private AudioClip _slotSwitchClip;
    Button[] _buttons;
    public GameObject _pausePanel;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _buttons = _pausePanel.GetComponentsInChildren<Button>();
    }

    override protected void SkipWhilePaused()
    {
        foreach(Button button in _buttons)
        {
            button.GetComponentInChildren<Text>().color = _selectedBrown;
        }
    }

        public IEnumerator Hover()
    {
        _audioSource.clip = _slotSwitchClip;
        _audioSource.Play();
        yield return new WaitForSeconds(_audioSource.clip.length);
    }

    public void PlayHover(GameObject button)
    {
        button.GetComponentInChildren<Text>().color = _unselectedTan;
        StartCoroutine(Hover());
    }

    public void PlayHoverExit(GameObject button)
    {
        button.GetComponentInChildren<Text>().color = _selectedBrown;
    }

}
