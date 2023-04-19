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
    public Camera _mainCamera;
    public GameObject _emptyHitObject;
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
        var ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 4)) 
        {
            _hit = hit.transform.gameObject;
        } else
        {
            _hit = _emptyHitObject;
            print("emptyHit");
        }

        if(_hit.gameObject.tag != "Note" && _hit.gameObject.tag != "StoryNote" && _hit.gameObject.tag != "Gun" && _hit.gameObject.tag != "Car")
        {
            _pickupText.enabled = false;
        } 

    }
}
