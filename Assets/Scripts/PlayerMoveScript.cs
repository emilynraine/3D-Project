using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    Rigidbody _rbody;
    Transform _transform;
    float _scale = 8;
    // Start is called before the first frame update
    void Start()
    {
        _rbody = GetComponent<Rigidbody>();
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(_scale * Input.GetAxis("Horizontal"), 0, _scale * Input.GetAxis("Vertical"));
        _transform.position +=  _transform.rotation * (Time.deltaTime * move);

        if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            _rbody.velocity = Vector3.zero;
        }
    }
}
