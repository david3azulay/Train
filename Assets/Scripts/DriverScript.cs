using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DriverScript : MonoBehaviour
{
    public Transform[] compartment;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = compartment[TrainScript.train.nextStation].transform.position;
        transform.LookAt(TrainScript.train.stations[TrainScript.train.nextStation]);
    }
}
