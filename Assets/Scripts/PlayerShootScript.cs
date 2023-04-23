using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootScript : MonoBehaviour
{
    public Animator _animator;
    public GameObject _gun;
    public Transform _gunTransform;
    public GameObject _hit;
    public MonsterScript _monster;
    public bool _hitCRPlaying = false;
    AudioSource _playerSource;
    public AudioClip _shootSound;
    public AudioClip _gunPickUp;
    public bool _playPickup = false;
    public Camera _mainCamera;
    public PlayerLookScript _playerLook;
    public GameObject _emptyHitObject;
    
    Invantory _inventory;
    public int _gunInventoryIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        _playerSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _gun.SetActive(false);
        _monster = FindObjectOfType<MonsterScript>();
        _playerLook = FindObjectOfType<PlayerLookScript>();
        _inventory = GetComponent<Invantory>();
    }

    // Update is called once per frame
    void Update()
    {
        //print("gun inventory index: " + _gunInventoryIndex);
        //print("curr selected: " + _inventory.getCurrSelected());

        if (_inventory.getCurrSelected() == _gunInventoryIndex)
        {
            _gun.SetActive(true);
        }
        else
        {
            _gun.SetActive(false);
        }

            if (_playPickup)
        {
            _playPickup = false;
            _playerSource.PlayOneShot(_gunPickUp);
        }
        if (Input.GetMouseButtonDown(0) && _gun.activeSelf)
        {
            StartCoroutine(GunAnimation());
            _playerSource.PlayOneShot(_shootSound);
            print("clicked mouse");

            var ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 8)) 
            {
                _hit = hit.transform.gameObject;
            }
            else
            {
                _hit = _emptyHitObject;
            }

            if(_hit.gameObject.tag == "Monster")
            {
                print("hit monster");
                _monster._gotShot = true;
                _monster._fleeing = true;
                StartCoroutine(_monster.HitAnim());
                StartCoroutine(_monster.PlayScreech());

            }
            //_gun.SetActive(false);
        }

    }

    public IEnumerator GunAnimation()
    {
        float i = .002f;
        while(_gunTransform.localPosition.z > .090f)
        {
            _gunTransform.localPosition = new Vector3(_gunTransform.localPosition.x, _gunTransform.localPosition.y, 
            _gunTransform.localPosition.z - i);
            yield return null;
        }

        while(_gunTransform.localPosition.z < .104f)
        {
            _gunTransform.localPosition = new Vector3(_gunTransform.localPosition.x, _gunTransform.localPosition.y, 
            _gunTransform.localPosition.z + i);
            yield return null;
        }
    }

    /*IEnumerator HitAnim()
    {
        _hitCRPlaying = true;
        _animator.SetBool("Hurt", true);
        yield return new WaitForSeconds(5.0f);
        print("hurt true");
        _animator.SetBool("Hurt", false);
        print("hurt false");
        _hitCRPlaying = false;
    }*/
}
