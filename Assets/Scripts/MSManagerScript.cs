using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSManagerScript : MonoBehaviour
{
    public string[] _messages = {"I sense something following me. If you find this, it may already have taken me.\n\nI will try hiding on the fire department roof."};
    public int _noteNum = 0;
    public GameObject _blackoutSquare;
    public bool _storyStart = false;
    float _timeSinceBlackout = 0;
    public Text _pickUp;
    public CameraScript _mainCamera;
    public PlayerMoveScript _playerMove;
    // Start is called before the first frame update
    void Start()
    {
        _playerMove = FindObjectOfType<PlayerMoveScript>();
        _mainCamera = FindObjectOfType<CameraScript>();
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


    }

    public IEnumerator BlackOut(bool fadeToBlack, float fadeSpeed = .3f)
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
}
