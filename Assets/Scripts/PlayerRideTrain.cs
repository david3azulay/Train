using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRideTrain : MonoBehaviour
{
    public GameObject player;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.gameObject.name}");
        if (other.gameObject.CompareTag("Character"))
        {
            
            player.transform.parent = transform.parent;
            TrainScript.train.addPassanger();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            player.transform.parent = null;
            
        }
    }
}
