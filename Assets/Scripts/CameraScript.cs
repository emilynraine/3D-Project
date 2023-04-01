using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    PlayerMoveScript _player;
    Transform _transform;
    float _changeValue = .1f;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerMoveScript>();
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(_player._moving && _player._movement)
        {
            //If the player is going up or down
            if(_transform.localPosition.y <= .5f)
            {
                _changeValue = .0063f;
            }
            else if(_transform.localPosition.y >= .7f)
            {
                _changeValue = -.0063f;
            }

            _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y + _changeValue, _transform.localPosition.z);
        }
    }
}
