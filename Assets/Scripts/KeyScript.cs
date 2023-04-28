using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    PlayerLookScript _player;
    PlayerUnlockScript _playerUnlock;
    Invantory _inventory;
    MSManagerScript _manager;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerLookScript>();
        _playerUnlock = FindObjectOfType<PlayerUnlockScript>();
        _inventory = _player.GetComponent<Invantory>();
        _manager = FindObjectOfType<MSManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player._hit.tag == "Key")
        {
            _player._pickupText.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                _manager._spawnInBuilding = true;
                _inventory.AddItemToInvanntory(this.GetComponent<CollectableObject>());
                _playerUnlock._keyInventoryIndex = _inventory.getNumInInventory() - 1;
                _playerUnlock._playPickup = true;
                _playerUnlock._key.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
