using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMotion : MonoBehaviour
{
    private const float cameraHeight = 1.86f;
    private float _speed;
    private float acceleration;
    private float verticalVelocity;
    private float _angularSpeed;
    private float _rotationAngle;
    private CharacterController _characterController;
    private float _minX, _minZ, _maxX, _maxZ;

    // Start is called before the first frame update
    void Start()
    {
        _speed = 0f;
        _angularSpeed = 1f;
        acceleration = 4f;
        _rotationAngle = 0f;
        verticalVelocity = 0f;
        _characterController = GetComponent<CharacterController>();
        _minX = Terrain.activeTerrain.terrainData.bounds.min.x;
        _minZ = Terrain.activeTerrain.terrainData.bounds.min.z;
        _maxX = Terrain.activeTerrain.terrainData.bounds.min.x + Terrain.activeTerrain.terrainData.size.x;
        _maxZ = Terrain.activeTerrain.terrainData.bounds.min.z + Terrain.activeTerrain.terrainData.size.z;

    }

    // Update is called once per frame
    void Update()
    {
        // get mouse X-coordinate
        float mouse_x = Input.GetAxis("Mouse X");
        Vector3 horizontalVector = Vector3.zero;

        //  Handle keyboard events
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            _speed = 0f;
            acceleration = 0;
            horizontalVector = Vector3.zero;
        }    
        if (Input.GetKey(KeyCode.W))
            _speed = 15f + (acceleration += acceleration < 80 ? 1 + acceleration * 0.01f : 80) * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            _speed = -15f - (acceleration -= acceleration > -80 ? -1 - acceleration * 0.01f : -80) * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            horizontalVector = Vector3.right * (15f + (acceleration += acceleration < 80 ? 1 + acceleration * 0.01f : 80));
        if (Input.GetKey(KeyCode.A))
            horizontalVector = Vector3.left * (15f + (acceleration += acceleration < 80 ? 1 + acceleration * 0.01f : 80));


        // sets sight direction by means of transform.Rotate
        _rotationAngle += mouse_x * _angularSpeed * Time.deltaTime;
        transform.Rotate(0, _rotationAngle, 0);
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
        float point;
        if (transform.position.x <= _minX || transform.position.x >= _maxX || transform.position.z <= _minZ ||
           transform.position.z >= _maxZ) point = cameraHeight +Terrain.activeTerrain.SampleHeight(pos);
        else // update height to terrain height in point (position.x,,position.z)
        {
            point = cameraHeight + Terrain.activeTerrain.SampleHeight(pos) - transform.position.y; // delta in Y direction
        }
        
        //verticalVelocity = _characterController.bounds.center.y <= cameraHeight + Terrain.activeTerrain.SampleHeight(pos)? 0 : verticalVelocity -= cameraHeight * Time.deltaTime;

        // we shall use CharacterController to move and to stop if camera collides with another object
        Vector3 height = point > cameraHeight ? Vector3.down : Vector3.up;
        Vector3 direction = transform.TransformDirection((Vector3.forward * _speed + height * verticalVelocity * 2 + horizontalVector) * Time.deltaTime );
        transform.Translate(direction);
       // _characterController.Move(direction);

        
    }
}