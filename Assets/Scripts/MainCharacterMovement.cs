using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainCharacterMovement : MonoBehaviour
{
    float speed;
    float angularSpeed;
    float hMove, vMove;
    CharacterController cController;
    AudioSource audioSource;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        speed = 50f;
        angularSpeed = 100f;
        cController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

  

    // Update is called once per frame
    void Update()
    {
        
        // if a key was pressed
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            hMove = Input.GetAxis("Horizontal") * angularSpeed * Time.deltaTime;
            vMove = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            animator.SetFloat("Speed", animator.GetFloat("Speed") < 1f ? animator.GetFloat("Speed") + 0.2f : 1f);

            transform.Rotate(0, hMove, 0);
            // pos is the x-z coordinate of a character
            Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 point;
            float height = Terrain.activeTerrain.SampleHeight(pos) - transform.position.y;
            point.y = height;
            Vector3 direction = Vector3.forward * vMove;
            direction.y = point.y;
            
            // TransformDirection transforms coordinates to GLOBALs
            cController.Move(transform.TransformDirection(direction));

            //// sound effect
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }


        }
        else
        {
            animator.SetFloat("Speed", 0.0f);
        }
    }
}