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
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _gun.SetActive(false);
        _monster = FindObjectOfType<MonsterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
                StartCoroutine(_monster.HitAnim());

            }
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
