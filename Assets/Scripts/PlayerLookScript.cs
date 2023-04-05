using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLookScript : MonoBehaviour
{
    public GameObject _hit;
    Transform _transform;
    public Transform _headTransform;
    float verticalAngle = 0;
    public LayerMask _layer;
    PlayerMoveScript _player;
    public Text _pickupText;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerMoveScript>();
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(_player._movement)
        {
            float horizontalAngle = _transform.eulerAngles.y + (Input.GetAxis("Mouse X") * 4);
            verticalAngle -= (Input.GetAxis("Mouse Y") * 4);
            verticalAngle = Mathf.Clamp(verticalAngle, -60, 60);

            //_transform.rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
            _transform.rotation = Quaternion.Euler(0, horizontalAngle, 0);
            _headTransform.localRotation = Quaternion.Euler(verticalAngle, 0, 0);
        }

        //Look Ray
        var ray = new Ray(_headTransform.position, _headTransform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 15)) 
        {
            _hit = hit.transform.gameObject;
        }

        if(_hit.gameObject.tag != "Note" && _hit.gameObject.tag != "StoryNote" && _hit.gameObject.tag != "Gun")
        {
            _pickupText.enabled = false;
        }

    }
}
