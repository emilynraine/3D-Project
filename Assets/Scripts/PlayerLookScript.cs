using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookScript : MonoBehaviour
{
    Transform _transform;
    float verticalAngle = 0;
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalAngle = _transform.eulerAngles.y + (Input.GetAxis("Mouse X") * 4);
            verticalAngle -= (Input.GetAxis("Mouse Y") * 4);
            verticalAngle = Mathf.Clamp(verticalAngle, -60, 60);

            _transform.rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
    }
}
