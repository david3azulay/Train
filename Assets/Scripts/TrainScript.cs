using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrainScript : MonoBehaviour
{
    public Transform[] stations;
    public float speed;
    public float waitBeforeLeavingStation;
    public int nextStation;
    private float waitTimer;
    public int passangers;
    public WayPoint[] compartment2StandingPosition;
    public WayPoint[] compartment1StandingPosition;
    public bool[] arrivedStation;
    public bool inMovement;
    public static TrainScript train;
    public int passangersWaitingToExitTrain;

    private void Start()
    {
        transform.position = stations[(nextStation + 1) % stations.Length].position;
        train = this;
        arrivedStation = new bool[stations.Length];
        arrivedStation[(nextStation + 1) % stations.Length] = true;
        arrivedStation[nextStation] = false;
        inMovement = false;
        passangersWaitingToExitTrain = 0;
    }

    public WayPoint[] getStandingPositionLine(int compartment)
    {
        return compartment == 0 ? compartment1StandingPosition : compartment2StandingPosition;
    }

    public int addPassanger()
    {
        return passangers++;
    }

    public int removePassanger()
    {
        passangersWaitingToExitTrain--;
        return passangers--;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (waitForPassangers(waitBeforeLeavingStation) && passangers > 2 && passangersWaitingToExitTrain == 0)
        {
            if (transform.position != stations[nextStation].position)
            {
                if (!inMovement)
                {
                    inMovement = true;
                    arrivedStation[nextStation] = false;
                    arrivedStation[(nextStation + 1) % stations.Length] = false;
                }

                Vector3 position = Vector3.MoveTowards(transform.position, stations[nextStation].position, speed * Time.deltaTime);
                GetComponent<Rigidbody>().MovePosition(position);
            }
            else
            {
                passangersWaitingToExitTrain = passangers;
                arrivedStation[nextStation] = true;
                nextStation = (nextStation + 1) % stations.Length;
                waitTimer = 0f;
                inMovement = false;
            }
        }
    }

    private bool waitForPassangers(float timeToWait)
    {
        waitTimer += Time.deltaTime;
        if (waitTimer >= timeToWait)
            return true;

        return false;
    }



}
