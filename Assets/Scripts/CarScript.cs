using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarScript : MonoBehaviour
{
    PlayerLookScript _player;
    MSManagerScript _manager;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerLookScript>();
        _manager = FindObjectOfType<MSManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_player._hit.tag == "Car" && _manager._notesLeft)
        {
            _player._pickupText.text = "The car is locked...";
            _player._pickupText.enabled = true;
        }
        else if (_player._hit.tag == "Car" && !_manager._notesLeft && !_manager._playingDrive) 
        {
            _player._pickupText.text = "Press 'E' to start car";
            _player._pickupText.enabled = true;
        }
        else if (_player._hit.tag != "Door" && _manager._notesLeft)
        {
            _player._pickupText.text = "Press 'E' to pick up";
        }

        if((_player._hit.tag == "Car") && (Input.GetKeyDown(KeyCode.E)) && (!_manager._notesLeft))
        {
            _manager._won = true;
        }
    }
}
