using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterScript : MonoBehaviour
{
    AudioSource _audioSource;
    public AudioClip _fleeSound;
    public bool _playingMonster = false;
    public Animator _animator;
    Rigidbody _rbody;
    public GameObject _playerObject;
    PlayerShootScript _player;
    MSManagerScript _manager;
    Transform _transform;
    public bool _monsterActive = false;
    public bool _followingPlayer = false;
    NavMeshAgent _agent;
    public bool _gotShot = false;
    public bool _fleeing = false;

    Vector3 _directionToPlayer;
    Vector3 _fleePosition;

    public GameObject[] _waypoints;
    int _currentMovePoint;

    public GameObject _frontBuilding;
    public bool _arrived = false;
    public FrontBuildingScript _frontS;
    public bool _handleBuilding = true;

    public GameObject _alley;
    public bool _inAlley = false;
    public AlleyScript _alleyS;
    public bool _handleAlley = true;

    public float _time = 0;
    public float _timeSincePlayer = 0;

    public float _freezeTime = 0;
    public Vector3 _lastPosition;
    public Vector3 _curPosition;
    public float _positionTimeCheck = 0;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _transform = transform;
        _animator = GetComponent<Animator>();
        _rbody = GetComponent<Rigidbody>();
        _frontS = FindObjectOfType<FrontBuildingScript>();
        _alleyS = FindObjectOfType<AlleyScript>();
        _player = FindObjectOfType<PlayerShootScript>();
        _manager = FindObjectOfType<MSManagerScript>();
        _playerObject = GameObject.FindWithTag("Player");
        _frontBuilding = GameObject.FindWithTag("FrontBuilding");
        _alley = GameObject.FindWithTag("Alley");
        _agent = GetComponent<NavMeshAgent>();
        _currentMovePoint = Random.Range(0,3);
        _waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        _curPosition = _transform.position;
        _lastPosition = _transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _curPosition = _transform.position;
        _animator.SetBool("Idle", _rbody.velocity.x == 0.0f && _rbody.velocity.z == 0.0f && !_player._hitCRPlaying);
        _animator.SetBool("Forward", (_rbody.velocity.x > 0.0f || _rbody.velocity.z > 0.0f) && !_player._hitCRPlaying);
        
        if(_manager._storyStart)
        {
            _monsterActive = true;
        }

        if(_monsterActive)
        {
            if(_gotShot || _fleeing) //Make him flee from the player to another location
            {
                if(_gotShot)
                {   
                    _gotShot = false;
                    _directionToPlayer = _transform.position - _playerObject.transform.position;
                    _fleePosition = _transform.position + _directionToPlayer;
                    _agent.SetDestination(_fleePosition);
                    print("Fleeing to: " + _agent.destination);
                }

                float distance = Vector3.Distance(_transform.position, _fleePosition);
                if(distance < 2f)
                {
                    _fleeing = false;
                }
            }
            else if(_followingPlayer) // Make him follow the player's position directly
            {
                _agent.SetDestination(_playerObject.transform.position);
                //print("Following Player to: " + _agent.destination);
                _frontS._playerEnter = false;
                _arrived = false;
                _handleAlley = true;
                _alleyS._playerEnterAlley = false;
                _inAlley = false;
                _handleBuilding = true;
            }
            else if(_alleyS._playerEnterAlley || _frontS._playerEnter)
            {
                if(!_handleAlley)
                {
                    _frontS._playerEnter = false;
                    _arrived = false;
                    _handleAlley = true;
                }
                if(!_handleBuilding)
                {
                    _alleyS._playerEnterAlley = false;
                    _inAlley = false;
                    _handleBuilding = true;
                }
                if(_alleyS._playerEnterAlley) // Make him go check the alleys
                {
                    //print("ALLEY");
                    if(!_inAlley)
                    {
                        _agent.SetDestination(_alley.transform.position);
                        float distance = Vector3.Distance(_transform.position, _alley.transform.position);
                        if(distance < 2f)
                        {
                            _inAlley = true;
                            _time = 0;
                        }
                    } 
                    else
                    {
                        _time += Time.deltaTime;
                    }
                    if(_time > 3f)
                    {
                        _time = 0;
                        _alleyS._playerEnterAlley = false;
                        _inAlley = false;
                       // print("DONE WITH ALLEY");
                    }
                }

                if(_frontS._playerEnter) //Make him go check the front of the building
                {
                   // print("IN BUILDING");
                    if(!_arrived)
                    {
                        _agent.SetDestination(_frontBuilding.transform.position);
                        float distance = Vector3.Distance(_transform.position, _frontBuilding.transform.position);
                        if(distance < 2f)
                        {
                            _arrived = true;
                            _time = 0;
                        }
                    } 
                    else
                    {
                        _time += Time.deltaTime;
                    }
                    if(_time > 3f)
                    {
                        _time = 0;
                        _frontS._playerEnter = false;
                        _arrived = false;
                        //print("DONE WITH BUILDING");
                    }
                }
            }
            else // Make him move randomly around the map
            {
                _agent.SetDestination(_waypoints[_currentMovePoint].transform.position);
                float dtp = Vector3.Distance(_transform.position, _waypoints[_currentMovePoint].transform.position);
                if(dtp < 1f)
                {
                    //Prevent Same Point generating twice
                    int newPoint = Random.Range(0,4);
                    while(newPoint == _currentMovePoint)
                    {
                        newPoint = Random.Range(0,4);
                    }
                    _currentMovePoint = newPoint;

                    _agent.SetDestination(_waypoints[_currentMovePoint].transform.position);
                }
            }

            _positionTimeCheck += Time.deltaTime;
            //print("FROZEN for: " + _freezeTime);
            if(_positionTimeCheck > 1)
            {
                float dtp = Vector3.Distance(_lastPosition, _curPosition);
                _positionTimeCheck = 0;
                _lastPosition = _curPosition;
                if(dtp < 0.25f)
                {
                    _freezeTime += 1;
                }
                else
                {
                    _freezeTime = 0;
                }
            }

            if(_freezeTime > 4.0f)
            {
                _agent.enabled = false;
                _agent.enabled = true;
                _agent.ResetPath();
                _agent.Warp(new Vector3(-204f, 69.76448f, -5f));
                _freezeTime = 0;
            }
        }
    }

    //Player following circle
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            _followingPlayer = true;
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            _followingPlayer = false;
        }
    }

    // IEnumerator TestMove()
    // {
    //     //while (true)
    //     //{
    //         if (!_player._hitCRPlaying)
    //         {
    //             yield return new WaitForSeconds(15.0f);
    //             _animator.SetBool("Forward", true);
    //             yield return new WaitForSeconds(1.1f);
    //             _rbody.velocity = new Vector3(0, 0, 2.0f);
    //             yield return new WaitForSeconds(5.0f);
    //             _animator.SetBool("Forward", false);
    //             _rbody.velocity = new Vector3(0, 0, 0);
    //         }
            
    //     //}



    // }

    public IEnumerator PlayScreech()
    {
        _audioSource.clip = _fleeSound;
        _audioSource.Play();
        yield return new WaitForSeconds(_audioSource.clip.length);
    }
   public IEnumerator HitAnim()
    {
        _player._hitCRPlaying = true;
        _rbody.velocity = new Vector3(0, 0, 0);
        _animator.SetBool("Forward", false);
        print("hurt true");
        _animator.SetBool("Hurt", true);
        yield return new WaitForSeconds(1.0f);
        _animator.SetBool("Hurt", false);
        print("hurt false");
        _player._hitCRPlaying = false;
    }
}
