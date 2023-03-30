using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{
   public bool _charIsThere = false;
    public PlayerMoveScript _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerMoveScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _charIsThere = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _charIsThere = false;
        }
    }
}
