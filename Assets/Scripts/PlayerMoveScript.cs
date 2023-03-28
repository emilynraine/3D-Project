using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    Rigidbody _rbody;
    Transform _transform;
    float _scale = 4;
    public bool _movement = true;
    public bool _moving = false;

    AudioSource _playerSource;
    public AudioClip _footstepSound;

    bool _playStep = false;
    float _timeSinceFoot = 1;
    // Start is called before the first frame update
    void Start()
    {
        _rbody = GetComponent<Rigidbody>();
        _transform = transform;
        _playerSource = GetComponent<AudioSource>();
        _movement = true;
    }

    // Update is called once per frame
    void Update()
    {    
        if(_movement)
        {
            //Timing for footsteps
            _timeSinceFoot = _timeSinceFoot + Time.deltaTime;
            if (_timeSinceFoot > .70f)
            {
                _playStep = true;
            }

            //Movement
            Vector3 move = new Vector3(_scale * Input.GetAxis("Horizontal"), 0, _scale * Input.GetAxis("Vertical"));
            _transform.position +=  _transform.rotation * (Time.deltaTime * move);
            _moving = true;
            if(Input.GetAxis("Horizontal") < .01 && Input.GetAxis("Vertical") < .01)
            {
                _rbody.velocity = Vector3.zero;
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
    }

    public void PlayFoot()
    {
        _playerSource.PlayOneShot(_footstepSound);
    }
}
