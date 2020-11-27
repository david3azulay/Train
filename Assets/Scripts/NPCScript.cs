using UnityEngine;
using Random = System.Random;

public class NPCScript : MonoBehaviour
{
    public static NPCScript npc;

    //  Train Station 1 waypoints
    public Transform[] station1EnterLine1;
    public Transform[] station1EnterLine2;
    public Transform[] station1ExitLine1;
    public Transform[] station1ExitLine2;
    public Transform[] station1RotatePoints;

    //  Train Station 2 waypoints
    public Transform[] station2EnterLine1;
    public Transform[] station2EnterLine2;
    public Transform[] station2ExitLine1;
    public Transform[] station2ExitLine2;
    public Transform[] station2RotatePoints;


    //  Train fields
    public Transform[] trainCompartments;
    public int currentCompartment, currentStationDock, currentLinePosition, currentTrainPosition;
    public bool onTrain, exitingTrain, exitedTrain, waitingForTrain, inTrainPosition;
    public bool standingInLine, inLinePosition;

    //  Target navigation fields
    public float speed;
    public int currentRotationIndex;
    public Transform waypointTarget;
    public bool arrivedWayPoint, targetedLinePosition, initialState, needToRoate, rotationComplete, rotating;

    private void Start()
    {
        npc = this;
        Init();
    }

    public void Init()
    {
        rotating = false;
        rotationComplete = false;
        needToRoate = false;
        exitedTrain = false;
        initialState = true;
        onTrain = false;
        arrivedWayPoint = false;
        waitingForTrain = false;
        inTrainPosition = false;
        waypointTarget = transform;
        targetedLinePosition = false;
        standingInLine = false;
        inLinePosition = false;
        exitingTrain = false;
        currentRotationIndex = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //  Enter train line
        chooseALine();
        goToBeginningOfLine(getTrainEnterLine(currentCompartment));

        if (!onTrain && !waitingForTrain && standingInLine && !inLinePosition)
            progressInLine(getTrainEnterLine(currentCompartment));

        //  Enter the deepest position in train
        enterTrain();
        standInTrainPosition();

        //  NPC rides the train to next station
        if (onTrain && inTrainPosition && TrainScript.train.inMovement)
            moveWithTrain(waypointTarget);

        //  If reached destination
        goBackToTrainEntrance();
        exitTrain();

        //  Progress in exit line
        if (!onTrain && waitingForTrain && exitingTrain && standingInLine)
            progressInLine(getTrainExitLine(currentCompartment), false);

        //  Go to new station train enter line
        goToNewTrainSationEnterLine();
    }

    public Transform[] getTrainEnterLine(int compartment)
    {
        if (currentStationDock == 0)
            return compartment == 0 ? station1EnterLine1 : station1EnterLine2;
        else
            return compartment == 0 ? station2EnterLine1 : station2EnterLine2;
    }

    public Transform[] getTrainExitLine(int compartment)
    {
        if (currentStationDock == 0)
            return compartment == 0 ? station2ExitLine2 : station2ExitLine1;
        else
            return compartment == 0 ? station1ExitLine1 : station1ExitLine2;
    }

    public void chooseALine()
    {
        if (initialState)
        {
            currentCompartment = chooseRandomLine();
            Init();
            initialState = false;
        }
    }

    public int chooseRandomLine()
    {
        Random randomNumber = new Random();
        return randomNumber.Next(0, trainCompartments.Length);
    }

    private void goToBeginningOfLine(Transform[] line)
    {
        if (!onTrain && !waitingForTrain && !standingInLine && !exitingTrain)
        {
            if (!targetedLinePosition)
            {
                if (line[line.Length - 1].gameObject.GetComponent<WayPoint>().checkIfAvailable())
                {
                    currentLinePosition = line.Length - 1;
                    targetedLinePosition = true;
                }
            }
            goTo(line[currentLinePosition]);
            if (arrivedWayPoint)
            {
                standingInLine = true;
            }
        }
    }

    private void progressInLine(Transform[] line, bool enterTrain = true)
    {
        int position = enterTrain ? currentLinePosition - 1 : currentLinePosition + 1;
        if ((enterTrain ? currentLinePosition > 0 : currentLinePosition < 2)
            && line[position].gameObject.GetComponent<WayPoint>().checkIfAvailable())
        {
            goTo(line[position]);
            if (arrivedWayPoint)
            {
                currentLinePosition = (enterTrain ? currentLinePosition - 1 : currentLinePosition + 1);
                if (currentLinePosition == (enterTrain ? 0 : line.Length - 1))
                {
                    inLinePosition = true;
                    waitingForTrain = true;
                    if (!enterTrain)
                    {
                        exitedTrain = true;
                        exitingTrain = false;
                        rotating = true;
                    }
                }
            }
        }
        else
        {
            GetComponent<Animator>().SetFloat("Speed", 0f);
        }
    }


    private void enterTrain()
    {
        if (!onTrain && waitingForTrain && standingInLine && !rotating
            && TrainScript.train.nextStation != currentStationDock
            && !TrainScript.train.arrivedStation[TrainScript.train.nextStation]
            && TrainScript.train.passangersWaitingToExitTrain == 0)
        {
            if(!TrainScript.train.inMovement)
            {
                goTo(trainCompartments[currentCompartment]);
                if (arrivedWayPoint)
                {
                    onTrain = true;
                    waitingForTrain = false;
                }
            }
            else
            {
                transform.LookAt(waypointTarget);
                GetComponent<Animator>().SetFloat("Speed", 0f);
            }
                
        }
    }

    private void standInTrainPosition()
    {
        if (onTrain && !inTrainPosition && !exitingTrain)
        {
            int amountOfPositions = TrainScript.train.getStandingPositionLine(currentCompartment).Length;
            currentTrainPosition = amountOfPositions - TrainScript.train.passangers - 1;
            Transform standingPosition = TrainScript.train.getStandingPositionLine(currentCompartment)[currentTrainPosition].transform;
            goTo(standingPosition);
            if (arrivedWayPoint)
            {
                exitingTrain = true;
                standingInLine = false;
                inLinePosition = false;
                inTrainPosition = true;
                targetedLinePosition = false;
                TrainScript.train.addPassanger();
            }
        }
    }

    public void goBackToTrainEntrance()
    {
        if (onTrain && inTrainPosition && exitingTrain
            && !TrainScript.train.inMovement
            && TrainScript.train.arrivedStation[(currentStationDock + 1) % TrainScript.train.stations.Length])
        {
            if (currentTrainPosition > 0)
            {
                WayPoint trainEntrance = TrainScript.train.getStandingPositionLine(currentCompartment)[currentTrainPosition - 1];
                if (trainEntrance.checkIfAvailable())
                {
                    goTo(trainEntrance.transform);
                    if (arrivedWayPoint)
                    {
                        currentTrainPosition--;
                    }
                }
            }
            else
            {
                inTrainPosition = false;
            }
        }
    }

    private void exitToBeginningOfLine(Transform[] line)
    {

        if (onTrain && exitingTrain && !inTrainPosition && !exitedTrain
            && !TrainScript.train.inMovement
            && TrainScript.train.arrivedStation[(currentStationDock + 1) % TrainScript.train.stations.Length])
        {
            if (!targetedLinePosition)
            {
                currentLinePosition = 0;
                targetedLinePosition = true;
            }
            else
            {
                if (line[currentLinePosition].gameObject.GetComponent<WayPoint>().checkIfAvailable())
                {
                    goTo(line[currentLinePosition]);
                    if (arrivedWayPoint)
                    {
                        standingInLine = true;
                        onTrain = false;
                        waitingForTrain = true;
                        TrainScript.train.removePassanger();
                    }
                }
                else
                {
                    GetComponent<Animator>().SetFloat("Speed", 0f);
                }

            }
        }
    }

    private void exitTrain()
    {
        if (onTrain && exitingTrain
            && currentStationDock == TrainScript.train.nextStation
            && TrainScript.train.arrivedStation[(currentStationDock + 1) % TrainScript.train.stations.Length])
        {
            exitToBeginningOfLine(getTrainExitLine(currentCompartment));
            if (inLinePosition)
            {
                exitedTrain = true;
            }
        }
    }

    public void goToNewTrainSationEnterLine()
    {
        if (!initialState && exitedTrain && !rotationComplete)
        {
            if (!needToRoate)
            {
                currentStationDock = (currentStationDock + 1) % TrainScript.train.stations.Length;
                needToRoate = true;

            }

            Transform[] rotatePoint = currentStationDock == 0 ?
                    station1RotatePoints : station2RotatePoints;
            int rotationPoints = (currentStationDock == 0 ? station1RotatePoints.Length - 1 : station2RotatePoints.Length - 1);
            if (currentRotationIndex <= rotationPoints
                && rotatePoint[currentRotationIndex].gameObject.GetComponent<WayPoint>().checkIfAvailable())
            {

                goTo(rotatePoint[currentRotationIndex]);
                if (arrivedWayPoint)
                {
                    currentRotationIndex++;
                }
            }
            else if (currentRotationIndex > rotationPoints)
            {
                rotationComplete = true;
                //  free the last waypoint targeted
                waypointTarget.gameObject.GetComponent<WayPoint>().setAvailable(true);
                initialState = true;
            }
        }
    }


    private void goTo(Transform target)
    {
        //  Mark the current waypoint as available
        waypointTarget.gameObject.GetComponent<WayPoint>()?.setAvailable(true);
        //  Set the next target waypoint
        waypointTarget = target;
        //  Go to the waypoint
        arrivedWayPoint = false;
        goToWayPoint(waypointTarget);
    }

    private void moveWithTrain(Transform waypointTarget)
    {
        GetComponent<Animator>().SetFloat("Speed", 0f);
        Vector3 position = Vector3.MoveTowards(transform.position, waypointTarget.position, speed);
        GetComponent<Rigidbody>().MovePosition(position);
    }

    private void goToWayPoint(Transform target)
    {
        transform.LookAt(target);
        if (transform.position != target.position)
        {
            Vector3 position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(position);
            GetComponent<Animator>().SetFloat("Speed", 1f);
        }
        else
        {
            GetComponent<Animator>().SetFloat("Speed", 0f);
            arrivedWayPoint = true;
            waypointTarget = target;
            target.gameObject.GetComponent<WayPoint>().setAvailable(false);
            transform.LookAt(Vector3.forward);
        }
    }
}
