using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{

    public static WayPoint waypoint;

    [SerializeField]
    protected float radius = 1.0f;

    [SerializeField]
    bool _isTargeted = false;

    [SerializeField]
    bool isAvailable;

    private void Start()
    {
        waypoint = this;
        isAvailable = true;
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void setAvailable(bool value)
    {
        this.isAvailable = value;
    }

    public bool checkIfAvailable()
    {
        return this.isAvailable;
    }
}
