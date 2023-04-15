using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MSManagerScript : MonoBehaviour
{
    public string[] _messages = {"I sense something following me. If you find this, it may already have taken me.\nWill grab gun and take cover on fire dept. roof.", "Seems safe for the time being. Will stay here with gun...\nwait, I just heard something. I'm afraid th", 
        "1/3\nThere's a reason this city is so empty.", "2/3 \nAlways just out of sight.", "3/3 \nWhat a shame, you would have made an excellent meal..."};
    public int _noteNum = 0;
    public GameObject _blackoutSquare;
    public bool _storyStart = false;
    float _timeSinceBlackout = 0;
    public Text _pickUp;
    
    public CameraScript _mainCamera;
    public PlayerMoveScript _playerMove;

    public List<NoteScript> _sortedNotes;
    public NoteScript[] _notes;
    public GameObject[] _spawnPts;

    public int _lastPos = 0;
    public bool _dead = false;

    [SerializeField]
    private AudioClip _roarClip;
    [SerializeField]
    private AudioClip _loopClip;

    // Start is called before the first frame update
    void Start()
    {
        _playerMove = FindObjectOfType<PlayerMoveScript>();
        _mainCamera = FindObjectOfType<CameraScript>();
        _notes = FindObjectsOfType<NoteScript>(true);
        
        StartCoroutine(PlaySounds(_roarClip, _loopClip));
        
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
            _playerMove._zMax = 75f;
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
            Invoke("LoadEnd", 4f);
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
            //Win state goes here?
            print("player has won");
            Invoke("LoadWin", 2f);

        }
    }

    public void LoadEnd()
    {
        SceneManager.LoadScene("EndScene");
    }

    public void LoadWin()
    {
        SceneManager.LoadScene("WinScene");
    }

    IEnumerator PlaySounds(AudioClip _clip1, AudioClip _clip2)
    {
        AudioSource _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _clip1;
        _audioSource.Play();
        StartCoroutine(FadeAudioSource.StartFade(_audioSource, _audioSource.clip.length, 0.0f));
        yield return new WaitForSeconds(_audioSource.clip.length);
        _audioSource.volume = 1.0f;

        _audioSource.clip = _clip2;
        _audioSource.loop = true;
        _audioSource.Play();
        yield return new WaitForSeconds(_audioSource.clip.length);
    }


}


//taken from https://johnleonardfrench.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/#:~:text=There's%20no%20separate%20function%20for,script%20will%20do%20the%20rest.

public static class FadeAudioSource
{
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}

class NoteComparer : IComparer
{
    public int Compare(object x, object y)
    {
        return (new CaseInsensitiveComparer()).Compare(((NoteScript)x)._id, ((NoteScript)y)._id);
    }
}

