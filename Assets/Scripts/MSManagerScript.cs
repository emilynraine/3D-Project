using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MSManagerScript : MonoBehaviour
{
    public bool _paused = false;
    public bool _isTransitioning = false;
    public GameObject _pauseCanvas;
    public GameObject _controlsPanel;
    public GameObject _mainPanel;
    public GameObject _inventoryCanvas;

    Color _selectedBrown = new Color(81 / 255f, 56 / 255f, 23 / 255f, 255 / 255f);
    Color _unselectedTan = new Color(232 / 255f, 213 / 255f, 183 / 255f, 255 / 255f);
    // public GameObject _controlsCanvas;


    public string[] _messages = {"I sense something following me. If you find this, it may already have taken me.\nWill grab gun and take cover on fire dept. roof.", "Seems safe for the time being. Will stay here with gun...\nwait, I just heard something. I'm afraid th",
        "1/4\nThere's a reason this city is so empty.", "2/4 \nAlways just out of sight.", "3/4 \nYou're not special. You won't escape the fate of the others.", "4/4 \nWhat a shame, you would have made an excellent meal..."};
    public int _noteNum = 0;
    public int _gunNum = 0;
    public GameObject _blackoutSquare;
    public bool _storyStart = false;
    float _timeSinceBlackout = 0;
    public Text _pickUp;
    public Text _carText;

    public CameraScript _mainCamera;
    public PlayerMoveScript _playerMove;
    public PlayerLookScript _playerLook;

    public List<NoteScript> _sortedNotes;
    public NoteScript[] _notes;
    public GameObject[] _guns;
    public GameObject[] _spawnPts;

    public Rigidbody _endCar;
    public bool _notesLeft = true;
    public bool _won = false;
    public GameObject _realCamera;
    public GameObject _cutsceneCamera;
    public bool _playingDrive = false;
    public Camera _secondCamera;

    public int _lastPos = 0;
    public bool _dead = false;

    [SerializeField]
    private AudioClip _roarClip;
    [SerializeField]
    private AudioClip _loopClip;

    public GameObject _car;
    public AudioClip _carStart;
    public AudioClip _carDrive;
    public bool _playedCar = false;

    public bool _spawnInBuilding = false;
    public Button[] _buttons;
    AudioSource _audioSource;

    public bool _hasMoved = false;

    // Start is called before the first frame update
    void Start()
    {
        _cutsceneCamera.SetActive(false);
        _realCamera.SetActive(true);
        _audioSource = GetComponent<AudioSource>();
        _pauseCanvas.SetActive(false);
        _controlsPanel.SetActive(false);
        _realCamera.SetActive(true);
        _cutsceneCamera.SetActive(false);

        _playerMove = FindObjectOfType<PlayerMoveScript>();
        _mainCamera = FindObjectOfType<CameraScript>();
        _notes = FindObjectsOfType<NoteScript>(true);
        _guns = GameObject.FindGameObjectsWithTag("Gun");

        StartCoroutine(PlaySounds(_roarClip, _loopClip));
        
        Array.Sort(_notes, new NoteComparer());
        StartCoroutine(BlackOut(false));

        Cursor.visible = false;
        _carText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (_paused)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _paused = !_paused;
            Pause();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _won = true;
        }

        if(_playedCar)
        {
            _car.GetComponent<AudioSource>().volume = _car.GetComponent<AudioSource>().volume - 0.02f;
        }

        if(_playingDrive)
        {
            _endCar.transform.position = new Vector3(_endCar.transform.position.x - .09f, _endCar.transform.position.y, _endCar.transform.position.z);
            _secondCamera.transform.position = new Vector3(_secondCamera.transform.position.x + .07f, _secondCamera.transform.position.y, _secondCamera.transform.position.z);
        }

        //Blackout and Wake up the player for the story
        if(_storyStart && !_isTransitioning) 
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

        if(_won)
        {
            _won = true;
            if(!_playedCar)
            {
                _car.GetComponent<AudioSource>().Play();
                _playedCar = true;
            }
            Invoke("EndCutScene", 3.5f);
        }

        if(!_notesLeft && !_playingDrive)
        {
            _carText.enabled = true;
        }
        else
        {
            _carText.enabled = false;
        }
    }


    public void SetNextNoteActive()
    {
        print("Notes length: " + _notes.Length);
        if (_noteNum + 1 < _notes.Length)
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
            _pickUp.text = "I should head back to the car at the gas station and leave...";
            _pickUp.enabled = true;
            _notesLeft = false;
        }
    }

    public void SetNextGunActive()
    {
        if (_gunNum + 1 < _guns.Length)
        {
            _gunNum++;
            print("now on gun: " + _gunNum);
            _guns[_gunNum].gameObject.SetActive(true);

            print("set gun " + _gunNum + " active");
        }
        else
        {
            print("no more guns, oob");
        }
    }

    public void EndCutScene()
    {
        _cutsceneCamera.SetActive(true);
        _realCamera.SetActive(false);
        _pickUp.enabled = false;
        _playingDrive = true;
        StartCoroutine(BlackOut(true, .12f));

        Invoke("LoadWin", 4f);
    }

    public void StopText()
    {
        _pickUp.enabled = false;
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




    void Pause()
    {
        /*if (_paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }*/
        _inventoryCanvas.SetActive(!_paused);
        _pauseCanvas.SetActive(_paused);
    }

    public void OnResumeButtonClick(GameObject button)
    {
        button.GetComponentInChildren<Text>().color = _selectedBrown;
        //button.GetComponentInChildren<Image>().color = _unselectedTan;
        _paused = !_paused;
        Pause();
    }

    public void OnRestartButtonClick(GameObject button)
    {
        button.GetComponentInChildren<Text>().color = _selectedBrown;
        // button.GetComponentInChildren<Image>().color = _unselectedTan;
        _pauseCanvas.SetActive(false);
        _isTransitioning = true;
        print("Restart");
        StartCoroutine(BlackOut(true));
        _paused = false;
        Invoke("MainScene", 3f);
    }

    public void OnControlsButtonClick(GameObject button)
    {
        button.GetComponentInChildren<Text>().color = _selectedBrown;
        _mainPanel.SetActive(false);
        _controlsPanel.SetActive(true);

    }

    public void OnBackButtonClick(GameObject button)
    {
        _mainPanel.SetActive(true);
        _controlsPanel.SetActive(false);
    }

    public void OnMenuButtonClick(GameObject button)
    {
        button.GetComponentInChildren<Text>().color = _selectedBrown;
        _pauseCanvas.SetActive(false);
        _isTransitioning = true;
        StartCoroutine(BlackOut(true));
        Time.timeScale = 0f;
        Invoke("TitleScene", 3f);
        _paused = false;

    }


    public void MainScene()
    {
        _pauseCanvas.SetActive(false);
        SceneManager.LoadScene("DemoScene");
    }

    public void TitleScene()
    {
        _pauseCanvas.SetActive(false);
        SceneManager.LoadScene("TitleScene");
    }

    public IEnumerator BlackOut(bool fadeToBlack, float fadeSpeed = .5f)
    {
        Color objectColor = _blackoutSquare.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (_blackoutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.b, objectColor.g, fadeAmount);
                _blackoutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }

        if (!fadeToBlack)
        {
            while (_blackoutSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.b, objectColor.g, fadeAmount);
                _blackoutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
        _isTransitioning = false;
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

