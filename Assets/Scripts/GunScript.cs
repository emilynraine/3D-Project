using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    PlayerLookScript _player;
    PlayerShootScript _shooter;
    Invantory _inventory;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerLookScript>();
        _shooter = FindObjectOfType<PlayerShootScript>();
        _inventory = _player.GetComponent<Invantory>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_player._hit.tag == "Gun")
        {
            _player._pickupText.enabled = true;
            if(Input.GetKeyDown(KeyCode.E))
            {
                _inventory.AddItemToInvanntory(this.GetComponent<CollectableObject>());
                _shooter._gunInventoryIndex = _inventory.getNumInInventory() - 1;
                _shooter._playPickup = true;
                _shooter._gun.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
