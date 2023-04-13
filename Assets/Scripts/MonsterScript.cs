using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public Animator _animator;
    Rigidbody _rbody;
    PlayerShootScript _player;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rbody = GetComponent<Rigidbody>();
        _player = FindObjectOfType<PlayerShootScript>();
        StartCoroutine(TestMove());
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("Idle", _rbody.velocity.x == 0.0f && _rbody.velocity.z == 0.0f && !_player._hitCRPlaying);
        


    }

    IEnumerator TestMove()
    {
        //while (true)
        //{
            if (!_player._hitCRPlaying)
            {
                yield return new WaitForSeconds(15.0f);
                _animator.SetBool("Forward", true);
                yield return new WaitForSeconds(1.1f);
                _rbody.velocity = new Vector3(0, 0, 2.0f);
                yield return new WaitForSeconds(5.0f);
                _animator.SetBool("Forward", false);
                _rbody.velocity = new Vector3(0, 0, 0);
            }
            
        //}



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
