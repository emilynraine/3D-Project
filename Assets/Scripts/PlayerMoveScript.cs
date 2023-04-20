using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveScript : MonoBehaviour
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

    MSManagerScript _manager;
    AudioSource _playerSource;
    public AudioClip _footstepSound;
    public AudioClip _tenseSound1;
    public Text _boundsText;

    bool _playStep = false;
    float _timeSinceFoot = 1;
    float _timeSinceTense1 = 2;
    // Start is called before the first frame update
    void Start()
    {
        _rbody = GetComponent<Rigidbody>();
        _transform = transform;
        _playerSource = GetComponent<AudioSource>();
        _movement = true;
        _ladder = FindObjectOfType<LadderScript>();
        _manager = FindObjectOfType<MSManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {    
        if(_movement && !_climbingLadder)
        {
            //Timing for footsteps
            _timeSinceFoot = _timeSinceFoot + Time.deltaTime;
            _timeSinceTense1 = _timeSinceTense1 + Time.deltaTime;
            if (_timeSinceFoot > .72f)
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

            //Movement
            Vector3 move = new Vector3(_scale * Input.GetAxis("Horizontal"), 0, _scale * Input.GetAxis("Vertical"));
            _transform.position +=  _transform.rotation * (Time.deltaTime * move);
            _moving = true;
            if(Input.GetAxis("Horizontal") < .1f && Input.GetAxis("Vertical") < .1f)
            {
                _rbody.velocity = new Vector3(0, _rbody.velocity.y, 0);;
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
           _rbody.velocity = new Vector3(1, 3, 0);
            
        }
        else if (_ladder._charIsThere && Input.GetKey(KeyCode.S))
        {
            _climbingLadder = true;
            _rbody.velocity = new Vector3(1, -3, 0);
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
