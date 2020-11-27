using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainFloorScript : MonoBehaviour
{
    public GameObject character;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            character.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            character = null;
        }
    }
}
