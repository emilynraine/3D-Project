using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class NoteScript : MonoBehaviour
{
    public Image _noteImage;
    public Text _pickUp;
    public Text _messageText;
    public Text _messageText2;
    public int _id;

    PlayerLookScript _player;
    bool _inNote = false;
    public bool _putDownBefore = false;

    MSManagerScript _manager;
    CameraScript _mainCamera;
    GameObject _lastNote;
    string _messageString;
    string _messageString2;

    AudioSource _noteSource;
    public AudioClip _noteSound;
    public GameObject _notePrefab;
 
    // Start is called before the first frame update
    void Start()
    {

        _player = FindObjectOfType<PlayerLookScript>();
        _manager = FindObjectOfType<MSManagerScript>();;
        _noteSource = GetComponent<AudioSource>();
        _mainCamera = FindObjectOfType<CameraScript>();

        var rand = new Random();
        if (_id > 1)
        {
            if (_manager._spawnInBuilding)
            {
                int _randPos = rand.Next(0, _manager._spawnPts.Length);
                while (_randPos == _manager._lastPos)
                {
                    _randPos = rand.Next(0, _manager._spawnPts.Length);
                }
                transform.position = _manager._spawnPts[_randPos].transform.position;
                _manager._lastPos = _randPos;
            }
            else
            {
                int _randPos = rand.Next(0, 6);
                while (_randPos == _manager._lastPos)
                {
                    _randPos = rand.Next(0, 6);
                }
                transform.position = _manager._spawnPts[_randPos].transform.position;
                _manager._lastPos = _randPos;
            }


            

        }

        // _pickUp = gameObject.AddComponent<Text>();
        // _messageText = this.gameObject.AddComponent<Text>();
        //_messageText2 = this.gameObject.AddComponent<Text>();

        _pickUp.text = "Press 'E' to pick up";
        _messageText.text = "";
        _messageText2.text = "";
        _noteImage.enabled = false;
        _pickUp.enabled = false;
        print("my id is: " + _id);
        //_messageString = _manager._messages[_id];

        print("my message is: " + _messageString2);
        

        if(_id == 0)
        {
            _messageString = _manager._messages[_id];
        }
        else if (_id == 1)
        {
            _messageString = _manager._messages[_id];
            _messageString2 = "Yes, be afraid. The notes are mine now. Find them all before I find you.";
        }
        else
        {
            _messageString2 = _manager._messages[_id];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If it is a note
        if(_player._hit.tag == "Note" || _player._hit.tag == "StoryNote")
        {
            _pickUp.enabled = true;
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
                    _messageText2.text = "";
                    PlayNote();
                    _inNote = false;
                    _noteImage.enabled = false;


                    if (_player._hit.tag == "StoryNote") 
                    {
                        _manager._storyStart = true;
                        _noteImage.enabled = false;


                        //Destroy(gameObject);

                    }
                    if (!_putDownBefore && _id < _manager._notes.Length)
                    {
                        StartCoroutine(PrepareNextNote());
                    }
                }
                else //If the note is not up yet
                {
                    PlayNote();

                        _noteImage.enabled = true;
                        _inNote = true;
                        _messageText.text = _messageString;
                        _messageText2.text = _messageString2;

                }
            }
            //If not pushing the input key
            if(!_inNote)
            {
                _messageText.text = "";
                _messageText2.text = "";
            }
        }
        else if(_inNote) //If not looking and in the note
        {
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayNote();
                _messageText.text = "";
                _messageText2.text = "";
                _noteImage.enabled = false;
                _inNote = false;
                if (!_putDownBefore && _id < _manager._notes.Length)
                {
                    StartCoroutine(PrepareNextNote());
                }
                if (_lastNote.tag == "StoryNote") 
                {
                    _manager._storyStart = true;
                    _noteImage.enabled = false;
                }
            }       
        }
        else
        {
            _messageText.text = "";
            _messageText2.text = "";
        }

    }

    public void PlayNote()
    {
        _noteSource.PlayOneShot(_noteSound);
    }

    IEnumerator PrepareNextNote() {
        _putDownBefore = true;
        _manager.SetNextNoteActive();
        yield return new WaitForSeconds(0.25f);
        print("note " + _id + " about to deactivate");
        gameObject.SetActive(false);
        print("after note " + _id + " deactivate");
        

    }
}

