using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnlockScript : MonoBehaviour
{
    AudioSource _playerSource;
    public AudioClip _keyPickUp;
    Invantory _inventory;
    public int _keyInventoryIndex = -1;
    public GameObject _key;
    public bool _hasKey;
    public bool _playPickup = false;

    // Start is called before the first frame update
    void Start()
    {
        _playerSource = GetComponent<AudioSource>();
        _key.SetActive(false);
        _inventory = GetComponent<Invantory>();
    }

    // Update is called once per frame
    void Update()
    {
        //print("key inventory index: " + _keyInventoryIndex);
        //print("curr selected: " + _inventory.getCurrSelected());

        if (_inventory.getCurrSelected() == _keyInventoryIndex)
        {
            _key.SetActive(true);
            _hasKey = true;
        }
        else
        {
            _hasKey = false;
            _key.SetActive(false);
        }

        if (_playPickup)
        {
            _playPickup = false;
            _playerSource.PlayOneShot(_keyPickUp);
        }
    }
}
