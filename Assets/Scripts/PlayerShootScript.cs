using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootScript : MonoBehaviour
{
    public Animator _animator;
    public GameObject _gun;
    public Transform _gunTransform;
    GameObject _hit;
    public MonsterScript _monster;
    public bool _hitCRPlaying = false;
    AudioSource _playerSource;
    public AudioClip _shootSound;
    // Start is called before the first frame update
    void Start()
    {
        _playerSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _gun.SetActive(false);
        _monster = FindObjectOfType<MonsterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _gun.activeSelf)
        {
            StartCoroutine(GunAnimation());
            _playerSource.PlayOneShot(_shootSound);
            print("clicked mouse");
            var _shootRay = new Ray(_gunTransform.position, -_gunTransform.forward);

            RaycastHit hit;
            if (Physics.Raycast(_shootRay, out hit, 25))
            {
                _hit = hit.transform.gameObject;
                print("hit an object");
                print(_hit.gameObject.tag);
            }

            if (_hit != null && _hit.gameObject.tag == "Monster" && (!_hitCRPlaying))
            {
                print("hit monster");
                _monster._gotShot = true;
                _monster._fleeing = true;
                StartCoroutine(_monster.HitAnim());

            }
            _gun.SetActive(false);
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
