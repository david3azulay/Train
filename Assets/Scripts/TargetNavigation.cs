using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetNavigation : MonoBehaviour
{

    public NavMeshAgent navigatorAgent;
    LineRenderer path;

    // Start is called before the first frame update
    void Start()
    {
        path = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        path.positionCount = navigatorAgent.path.corners.Length;
        path.SetPositions(navigatorAgent.path.corners);
    }
}
