using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarScript : MonoBehaviour
{
    PlayerLookScript _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerLookScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_player._hit.tag == "Car")
        {
            _player._pickupText.text = "Car is locked...";
            _player._pickupText.enabled = true;
        }
        else
        {
            _player._pickupText.text = "Press 'E' to pick up";
        }
    }
}
