using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MSManagerScript : MonoBehaviour
{
    public string[] _messages = {"I sense something following me. If you find this, it may already have taken me.\nWill grab gun and take cover on fire dept. roof.", "Seems safe for the time being. Will stay here with gun...\nwait, I just heard something. I'm afraid th", "I am Note 2, the first randomly spawned note", "I am Note 3, the second randomly spawned note", "I am Note 4, the third randomly spawned note"};
    public int _noteNum = 0;
    public GameObject _blackoutSquare;
    public bool _storyStart = false;
    float _timeSinceBlackout = 0;
    public Text _pickUp;
    public CameraScript _mainCamera;
    public PlayerMoveScript _playerMove;
    public NoteScript[] _notes;
    public GameObject[] _spawnPts;
    public List<NoteScript> _sortedNotes;
    public int _lastPos = 0;
    public bool _dead = false;

    // Start is called before the first frame update
    void Start()
    {
        _playerMove = FindObjectOfType<PlayerMoveScript>();
        _mainCamera = FindObjectOfType<CameraScript>();
        _notes = FindObjectsOfType<NoteScript>(true);
        Array.Sort(_notes, new NoteComparer());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //Blackout and Wake up the player for the story
        if(_storyStart) 
        {
            _playerMove._xMin = -330f;
            _playerMove._xMax = -203f;
            _playerMove._zMin = 0f;
            _playerMove._zMax = 65f;
            _playerMove.PlayTense1();
            _playerMove.KnockBack();
            _playerMove._movement = false;
            _pickUp.enabled = false;
            StartCoroutine(BlackOut(true));
            _timeSinceBlackout += Time.deltaTime;
            if(_timeSinceBlackout > 3.5) 
            {
                _storyStart = false;
                StartCoroutine(BlackOut(false));
                _playerMove._movement = true;
            }
        }

        if(_dead)
        {
            print("DEAD");
            StartCoroutine(BlackOut(true));
        }
    }

    public IEnumerator BlackOut(bool fadeToBlack, float fadeSpeed = .4f)
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


   public void SetNextNoteActive()
    {
        if (_noteNum + 1 <= _notes.Length - 1)
        {
            _noteNum++;
            print("now on note: " + _noteNum);
            _notes[_noteNum].gameObject.SetActive(true);

            print("set note " + _noteNum + " active");
        }
        else
        {
            print("no more notes, oob");
        }
    }
}


class NoteComparer : IComparer
{
    public int Compare(object x, object y)
    {
        return (new CaseInsensitiveComparer()).Compare(((NoteScript)x)._id, ((NoteScript)y)._id);
    }
}
