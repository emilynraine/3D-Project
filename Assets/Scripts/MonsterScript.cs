using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public Animator _animator;
    Rigidbody _rbody;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rbody = GetComponent<Rigidbody>();
        StartCoroutine(TestMove());
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("Idle", _rbody.velocity.x == 0.0f && _rbody.velocity.z == 0.0f);
        


    }

    IEnumerator TestMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(15.0f);
            _animator.SetBool("Forward", true);
            yield return new WaitForSeconds(1.1f);
            _rbody.velocity = new Vector3(0, 0, 2.0f);
            yield return new WaitForSeconds(5.0f);
            _animator.SetBool("Forward", false);
            _rbody.velocity = new Vector3(0, 0, 0);
            
        }
    }

}
