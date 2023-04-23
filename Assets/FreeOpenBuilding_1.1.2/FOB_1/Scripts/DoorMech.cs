using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMech : MonoBehaviour 
{
	PlayerUnlockScript _playerUnlock;
	public Vector3 OpenRotation, CloseRotation;

	public float rotSpeed = 1f;

	public bool doorBool;


	void Start()
	{
		_playerUnlock= FindObjectOfType<PlayerUnlockScript>();
		doorBool = false;
	}
		
	void OnTriggerStay(Collider col)
	{
		print("triggered");
		if(col.gameObject.tag == ("Player") && Input.GetKeyDown(KeyCode.E) && _playerUnlock._hasKey)
		{
			if (!doorBool)
			{
				doorBool = true;
				print("set true");
			}

			else
			{
				doorBool = false;
				print("set false");
			}
        }
	}

	void Update()
	{
		if (doorBool && Input.GetKeyDown(KeyCode.E) && _playerUnlock._hasKey)
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (OpenRotation), rotSpeed * Time.deltaTime);
		else if (!doorBool && Input.GetKeyDown(KeyCode.E) && _playerUnlock._hasKey)
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (CloseRotation), rotSpeed * Time.deltaTime);	
	}

}

