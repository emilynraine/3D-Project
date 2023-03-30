using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBoxScript : MonoBehaviour
{
    PlayerMoveScript _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerMoveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision c)
    {
        _player._climbingLadder = false;
    }
}
