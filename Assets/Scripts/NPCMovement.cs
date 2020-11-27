using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPCMovement : MonoBehaviour
{
    [SerializeField]
    public float totalWaitTime;

    // Start is called before the first frame update

    [SerializeField]
    List<WayPoint> patrolPoints;

    Animator animator;
    NavMeshAgent navigateAgent;
    int currentPatrolIndex;
   
    CharacterController characterController;


    void Start()
    {
        navigateAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        if (navigateAgent == null)
            Debug.LogError("No NavMeshAgent or target assigned to " + this.name);
        else
        {
            if (patrolPoints != null && patrolPoints.Count >= 1)
            {
                currentPatrolIndex = 0;
            }
            else
            {
                Debug.LogError("Insufficient patrol points for basic patroling behaviour.");
            }
        }
    }

    private void SetDetination()
    {
        if (patrolPoints != null)
        {
            currentPatrolIndex = NPCScript.npc.currentStationDock;
            Vector3 targetVector = patrolPoints[currentPatrolIndex].transform.position;
            navigateAgent.SetDestination(targetVector);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetDetination();
        //navigateAgent.SetDestination(patrolPoints[currentPatrolIndex].transform.position);
        if (navigateAgent.remainingDistance > 0)
        {
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            animator.SetFloat("Speed", 0f);

            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<NPCScript>().enabled = true;
            NPCScript.npc.initialState = true;
            GetComponent<NPCMovement>().enabled = false;
        }
    }

    private void ChangePatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
    }

}
