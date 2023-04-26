using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontBuildingScript : MonoBehaviour
{
    public bool _playerEnter = false;
    public MonsterScript _monster;
    // Start is called before the first frame update
    void Start()
    {
        _monster = FindObjectOfType<MonsterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _playerEnter = true;
            _monster._handleBuilding = false;
        }
    }
}
