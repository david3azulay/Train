using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMotion : MonoBehaviour
{
    public Animator animator;
    public bool isOpen;
    public GameObject DoorAxis;
    private AudioSource audioSource;
    public AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        animator = DoorAxis.GetComponent<Animator>();
        audioSource = DoorAxis.GetComponent<AudioSource>();
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character") || other.CompareTag("NPC") && !isOpen)
        {
            animator.SetTrigger("Open");
            
            audioSource.PlayDelayed(0.6f);
            isOpen = true;
           
            
        }     
    }

    void OnTriggerExit(Collider other)
    {
        if (isOpen)
        {
            animator.SetTrigger("Close");
            isOpen = false;
            audioSource.PlayOneShot(audioClip);
            
        }
    }
}
