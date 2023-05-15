using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMoveScript : PauseScript
{
    public Rigidbody _rbody;
    Transform _transform;
    float _scale = 4;
    public bool _movement = true;
    public bool _moving = false;
    public bool _climbingLadder = false;
    public LadderScript _ladder;

    public float _xMin = -322f;
    public float _xMax = -302f;
    public float _zMin = 12f;
    public float _zMax = 36f;

    MSManagerScript _moveManager;
    //fPauseScript _pauseManager;
    AudioSource _playerSource;
    public AudioClip _footstepSound;
    public AudioClip _tenseSound1;
    public Text _boundsText;
    Camera _mainCamera;

    public AudioSource _mouth;
    public AudioClip _sprintBreath;
    public AudioClip _tiredBreath;

    bool _playStep = false;
    float _timeSinceFoot = 1;
    float _timeSinceTense1 = 2;
    float _timeBetweenSteps = .72f;
    float _timeSprinting = 0;
    bool _playTired = false;
    bool _slowingBreath = false;
    bool _outOfBreath = false;

    public GameObject _stepRayUpper;
    public GameObject _stepRayLower;
    public float _stepHeight = 0.3f;
    public float _stepSmooth = 0.1f;

    bool _touchingStair = false;

    PostProcessVolume _volume;
    Vignette _vignette;

    // Start is called before the first frame update
    void Start()
    {
        _volume = gameObject.GetComponent<PostProcessVolume>();

        _mainCamera = Camera.main;
        _rbody = GetComponent<Rigidbody>();
        _transform = transform;
        _playerSource = GetComponent<AudioSource>();
        _movement = true;
        _ladder = FindObjectOfType<LadderScript>();
        _moveManager = FindObjectOfType<MSManagerScript>();
       // _pauseManager = FindObjectOfType<PauseScript>();

        _stepRayUpper.transform.position = new Vector3(_stepRayUpper.transform.position.x, _stepHeight, _stepRayUpper.transform.position.z);
    }

    // Update is called once per frame
    override protected void SkipWhilePaused()
    {

        if (_movement && !_climbingLadder && !_moveManager._isTransitioning)
        {
            //Timing for footsteps
            _timeSinceFoot = _timeSinceFoot + Time.deltaTime;
            _timeSinceTense1 = _timeSinceTense1 + Time.deltaTime;
            if (_timeSinceFoot > _timeBetweenSteps)
            {
                _playStep = true;
            }

            _boundsText.enabled = false;
            //Bound Checking
            if(_transform.position.x <= _xMin)
            {
                _transform.position = new Vector3(_xMin, _transform.position.y, _transform.position.z);
                _boundsText.enabled = true;
            }
            if(_transform.position.x >= _xMax)
            {
                _transform.position = new Vector3(_xMax, _transform.position.y, _transform.position.z);
                _boundsText.enabled = true;
            }
            if(_transform.position.z <= _zMin)
            {
                _transform.position = new Vector3(_transform.position.x, _transform.position.y, _zMin);
                _boundsText.enabled = true;
            }
            if(_transform.position.z >= _zMax)
            {
                _transform.position = new Vector3(_transform.position.x, _transform.position.y, _zMax);
                _boundsText.enabled = true;
            }

            //SPRINT =====
            //Check to see if the player has run out of sprint or has regained it
            if(_timeSprinting > 10)
            {
                _outOfBreath = false;
                _timeSprinting = 0;
                _playTired = false;
                _mouth.Stop();
            }
            else if(_timeSprinting > 5 || _outOfBreath)
            {
                _outOfBreath = true;
                if(!_playTired)
                {
                    _mouth.clip = _tiredBreath;
                    _mouth.pitch = .9f;
                    _mouth.Play();
                    _playTired = true;
                }
                _timeSprinting += Time.deltaTime;
            }

            //Check to see if the player is trying to sprint
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if(!_playTired)
                {
                    _slowingBreath = true;
                }

                if(!_outOfBreath)
                {
                    _timeSprinting -= Time.deltaTime;
                }
            }

            if(_slowingBreath)
            {
                if(_mouth.volume > 0.0)
                {
                    _mouth.volume -= .01f;
                }
                else
                {
                    _mouth.Stop();
                    _mouth.volume = 1;
                    _slowingBreath = false;
                }
            }

            //Decide the player's speed based on their sprint
            if(Input.GetKey(KeyCode.LeftShift) && _timeSprinting < 5)
            {
                _slowingBreath = false;
                if(Input.GetKeyDown(KeyCode.LeftShift) || _timeSprinting == 0)
                {
                    _mouth.volume = 1f;
                    _mouth.clip = _sprintBreath;
                    _mouth.pitch = 1f;
                    _mouth.Play();
                }
                _scale = 8;
                _timeBetweenSteps = .4f;
                _timeSprinting += Time.deltaTime;
            }
            else
            {
                if(!Input.GetKey(KeyCode.LeftShift) && _timeSprinting > 0 && !_outOfBreath)
                {
                    _timeSprinting -= Time.deltaTime;
                }
                _scale = 4;
                _timeBetweenSteps = .72f;
            }
            print(_timeSprinting);
            //SPRINT =====

            //Movement
            Vector3 move = new Vector3(_scale * Input.GetAxis("Horizontal"), 0, _scale * Input.GetAxis("Vertical"));
            _transform.position +=  _transform.rotation * (Time.deltaTime * move);
            _moving = true;
            if(Input.GetAxis("Horizontal") < .1f && Input.GetAxis("Vertical") < .1f)
            {
                if(_touchingStair)
                {
                    print("HERE");
                    _rbody.velocity = new Vector3(0, 0, 0);
                }
                else
                {
                    _rbody.velocity = new Vector3(0, _rbody.velocity.y, 0);
                }
                _moving = false;
            } 
            else
            {
                //If the player isn't moving play the footstep sound every half second
                if(_playStep)
                {
                    PlayFoot();
                    _timeSinceFoot = 0;
                    _playStep = false;
                }
            }
        }

        if (_ladder._charIsThere && Input.GetKey(KeyCode.W)){
           _climbingLadder = true;
           _rbody.velocity = new Vector3(0, 3, 0);
            
        }
        else if (_ladder._charIsThere && Input.GetKey(KeyCode.S))
        {
            _climbingLadder = true;
            _rbody.velocity = new Vector3(0, -3, 0);
        }
        else
        {
            _climbingLadder = false;
        }

    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Monster")
        {
            _manager._dead = true;
            _movement = false;
            KnockBack();
            PlayTense1();
        }
        else if(other.gameObject.tag == "Stair")
        {
            _touchingStair = true;
        }
    }

    private void OnCollisionExit(Collision other) 
    {
        if(other.gameObject.tag == "Stair")
        {
            _touchingStair = false;
        }
    }

    public void PlayFoot()
    {
        _playerSource.PlayOneShot(_footstepSound);
    }

    public void PlayTense1()
    {
        if(_timeSinceTense1 > 2)
        {
            _timeSinceTense1 = 0;
            _playerSource.PlayOneShot(_tenseSound1);
        }
    }

    public void KnockBack()
    {
        _rbody.AddForce(_transform.right * 100000f);
    }
}
