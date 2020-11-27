using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool useOffsetVaslues;
    public float rotateSpeed;
    public Transform pivot;
    

    private void Start()
    {
        if (!useOffsetVaslues)
            offset = target.position - transform.position;

        //pivot.transform.position = target.transform.position;
       // pivot.transform.parent = target.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //  Get the X position of the mouse & rotate the target
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        pivot.parent.Rotate(0, horizontal, 0);
        
        //  Get the Y position of the mouse & rotate the pivot
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        pivot.Rotate(-vertical, 0, 0);

        //  Move the camera based on the current rotation of the target & the original offset
        float desiredYAngle = target.eulerAngles.y;
        float desiredXAngle = target.eulerAngles.x;
        float desiredZAngle = target.eulerAngles.z;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = target.position - (rotation * offset);

        if (transform.position.y < target.position.y)
            transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);

        
        //transform.LookAt(target);

    }
}
