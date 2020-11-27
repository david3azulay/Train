using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDetails : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "People On Train: 0";
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "People On Train: " + TrainScript.train.passangers;

    }
}
