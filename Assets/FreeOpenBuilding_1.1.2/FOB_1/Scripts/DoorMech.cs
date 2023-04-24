using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMech : MonoBehaviour 
{
    public PlayerLookScript _playerLook;
    PlayerUnlockScript _playerUnlock;
	public Vector3 OpenRotation, CloseRotation;
	public AudioSource _audioSource;
	public AudioClip _unlockClip;
	public float rotSpeed = 1f;

	public bool _openDoor = false;


	void Start()
	{
		_audioSource = GetComponent<AudioSource>();
		_playerLook = FindObjectOfType<PlayerLookScript>();
		_playerUnlock= FindObjectOfType<PlayerUnlockScript>();
	}
		

	void Update()
	{

        if (_playerLook._hit.tag == "Door" && _playerUnlock._hasKey && _openDoor == false)
        {
            _playerLook._pickupText.text = "Press 'E' to open";
            _playerLook._pickupText.enabled = true;
        }
		else if(_playerLook._hit.tag == "Door" && !_playerUnlock._hasKey && _openDoor == false)
		{

            _playerLook._pickupText.text = "The door is locked.";
            _playerLook._pickupText.enabled = true;
        }
        else if (_playerLook._hit.tag != "Car")
        {
            _playerLook._pickupText.text = "Press 'E' to pick up";
        }

		if (_playerLook._hit.tag == "Door" && Input.GetKeyDown(KeyCode.E) && _playerUnlock._hasKey)
		{
			_openDoor = true;
			_audioSource.PlayOneShot(_unlockClip);
           
        }
		if (_openDoor)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(OpenRotation), rotSpeed * Time.deltaTime);
		}
	}

}

