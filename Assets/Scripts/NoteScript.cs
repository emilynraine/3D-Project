using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteScript : MonoBehaviour
{
    public Image _noteImage;
    public Text _pickUp;
    public Text _messageText;
    PlayerLookScript _player;
    bool _inNote = false;
    MSManagerScript _manager;
    CameraScript _mainCamera;
    GameObject _lastNote;

    AudioSource _noteSource;
    public AudioClip _noteSound;
    string _message = "";
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerLookScript>();
        _manager = FindObjectOfType<MSManagerScript>();
        _message = _manager._messages[_manager._noteNum];
        _noteSource = GetComponent<AudioSource>();
        _mainCamera = FindObjectOfType<CameraScript>();
        _messageText.text = "";
        _noteImage.enabled = false;
        _pickUp.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If it is a note
        if(_player._hit.tag == "Note" || _player._hit.tag == "StoryNote")
        {
            _lastNote = _player._hit;
            //If you push the input key
            if(Input.GetKeyDown(KeyCode.E))
            {
                //Turn off pick up text
                _pickUp.enabled = false;
                //If the note is already up take it down
                if(_inNote)
                {
                    _messageText.text = "";
                    PlayNote();
                    _noteImage.enabled = false;
                    _inNote = false;

                    if(_player._hit.tag == "StoryNote") 
                    {
                        _manager._storyStart = true;
                        _noteImage.enabled = false;
                        Destroy(gameObject);
                    }
                }
                else //If the note is not up yet
                {
                    PlayNote();
                    _noteImage.enabled = true;
                    _inNote = true;
                    _messageText.text = _message;
                }
            }
            //If not pushing the input key
            if(!_inNote)
            {
                _pickUp.enabled = true;
                _messageText.text = "";
            }
        }
        else if(_inNote) //If not looking and in the note
        {
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayNote();
                _messageText.text = "";
                _noteImage.enabled = false;
                _pickUp.enabled = false;
                _inNote = false;
                if(_lastNote.tag == "StoryNote") 
                {
                    _manager._storyStart = true;
                    _noteImage.enabled = false;
                    Destroy(gameObject);
                }
            }       
        }
        else
        {
            _pickUp.enabled = false;
            _messageText.text = "";
        }

    }

    public void PlayNote()
    {
        _noteSource.PlayOneShot(_noteSound);
    }

}
