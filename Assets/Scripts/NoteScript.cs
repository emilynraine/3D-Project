using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteScript : MonoBehaviour
{
    public Image _noteImage;
    public Text _pickUp;
    PlayerLookScript _player;
    bool _inNote = false;

    AudioSource _noteSource;
    public AudioClip _noteSound;
    // Start is called before the first frame update
    void Start()
    {
        _player = _player = FindObjectOfType<PlayerLookScript>();
        _noteSource = GetComponent<AudioSource>();
        _noteImage.enabled = false;
        _pickUp.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If it is a note
        if(_player._hit.tag == "Note")
        {
            //If you push the input key
            if(Input.GetKeyDown(KeyCode.E))
            {
                //Turn off pick up text
                _pickUp.enabled = false;
                //If the note is already up take it down
                if(_inNote)
                {
                    PlayNote();
                    _noteImage.enabled = false;
                    _inNote = false;

                }
                else //If the note is not up yet
                {
                    PlayNote();
                    _noteImage.enabled = true;
                    _inNote = true;
                }
            }
            //If not pushing the input key
            if(!_inNote)
            {
                _pickUp.enabled = true;
            }
        }
        else if(_inNote) //If not looking and in the note
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                PlayNote();
                _noteImage.enabled = false;
                _pickUp.enabled = false;
                _inNote = false;
            }       
        }
        else
        {
            _pickUp.enabled = false;
        }

    }

    public void PlayNote()
    {
        _noteSource.PlayOneShot(_noteSound);
    }

}
