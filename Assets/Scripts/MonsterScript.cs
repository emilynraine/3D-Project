using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterScript : MonoBehaviour
{
    AudioSource _audioSource;
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

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _transform = transform;
        _animator = GetComponent<Animator>();
        _rbody = GetComponent<Rigidbody>();
        _player = FindObjectOfType<PlayerShootScript>();
        _manager = FindObjectOfType<MSManagerScript>();
        _playerObject = GameObject.FindWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
        _currentMovePoint = Random.Range(0,3);
        _waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("Idle", _rbody.velocity.x == 0.0f && _rbody.velocity.z == 0.0f && !_player._hitCRPlaying);
        _animator.SetBool("Forward", (_rbody.velocity.x > 0.0f || _rbody.velocity.z > 0.0f) && !_player._hitCRPlaying);
        
        if(_manager._storyStart)
        {
            _monsterActive = true;
        }

        _monsterActive = true;
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
                }

                float distance = Vector3.Distance(_transform.position, _fleePosition);
                if(distance < 2f)
                {
                    _fleeing = false;
                }
            }
            else if(_followingPlayer) //Follow the player's position directly
            {
                _agent.SetDestination(_playerObject.transform.position);
            }
            else //Make him move randomly around the map
            {
                _agent.SetDestination(_waypoints[_currentMovePoint].transform.position);
                float dtp = Vector3.Distance(_transform.position, _waypoints[_currentMovePoint].transform.position);
                if(dtp < .5f)
                {
                    _currentMovePoint = Random.Range(0,3);
                    _agent.SetDestination(_waypoints[_currentMovePoint].transform.position);
                }
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
