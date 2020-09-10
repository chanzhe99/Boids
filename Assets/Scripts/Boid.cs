using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField] List<Boid> BoidsScanList;

    public Vector2 Velocity { get; set; }
    public Vector2 DesiredVelocity { get; set; }
    public Vector2 SteeringForce { get; set; }
    
    public void SetBoidsList(List<Boid> boidList)
    {
        BoidsScanList = boidList;
    }
}
