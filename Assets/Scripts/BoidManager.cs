using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    Vector2 ScreenEdge = new Vector2(9.0f, 5.0f);

    [Header("Target Settings")] [Space(5)]
    [SerializeField] GameObject Target;
    [Range(0.0f, 1.0f)] [SerializeField] float SeekWeight = 1.0f;

    [Header("Obstacle Settings")] [Space(5)]
    [SerializeField] GameObject Obstacle;
    [Range(0.0f, 5.0f)] [SerializeField] float ObstacleRadius = 3.0f;
    [Range(0.0f, 5.0f)] [SerializeField] float ObstacleWeight = 1.0f;

    [Header("Repellant Settings")] [Space(5)]
    [SerializeField] GameObject Repellant;
    [Range(0.0f, 5.0f)] [SerializeField] float RepellantRadius = 2.5f;
    [Range(0.0f, 5.0f)] [SerializeField] float RepellantWeight = 2.0f;
    [SerializeField] float RepellantSpeed = 100.0f;
    [SerializeField] float RepellantForce = 0.1f;
    [SerializeField] float RepellantMass = 10.0f;
    Boid RepellantBoid;

    [Header("Boid Steering Forces Settings")] [Space(5)]
    [SerializeField] float MaxSpeed = 10.0f;
    [SerializeField] float MaxForce = 0.2f;
    [SerializeField] float Mass = 5.0f;

    [Header("Boid Steering Behaviors Settings")] [Space(5)]
    [SerializeField] bool IsScreenWrapping;
    public bool UI_IsScreenWrapping { get { return IsScreenWrapping; } set { IsScreenWrapping = value; } }
    [SerializeField] bool IsSeeking;
    [SerializeField] bool IsAvoiding;
    [SerializeField] bool IsBeingRepelled;
    [SerializeField] bool IsSeparating;
    [SerializeField] bool IsAligning;
    [SerializeField] bool IsCohering;

    public bool UI_IsSeeking { get { return IsSeeking; } set { IsSeeking = value; } }
    public bool UI_IsAvoiding { get { return IsAvoiding; } set { IsAvoiding = value; } }
    public bool UI_IsBeingRepelled { get { return IsBeingRepelled; } set { IsBeingRepelled = value; } }

    [Range(0.0f, 5.0f)] [SerializeField] float SeparationWeight = 1.0f;
    public float UI_SeparationWeight { get { return SeparationWeight; } set { SeparationWeight = value; } }
    [Range(0.0f, 5.0f)] [SerializeField] float AlignmentWeight = 1.0f;
    public float UI_AlignmentWeight { get { return AlignmentWeight; } set { AlignmentWeight = value; } }
    [Range(0.0f, 5.0f)] [SerializeField] float CohesionWeight = 1.0f;
    public float UI_CohesionWeight { get { return CohesionWeight; } set { CohesionWeight = value; } }
    [Range(0.0f, 0.5f)] [SerializeField] float CohesionSmoothTime = 0.25f;
    Vector2 CohesionVelocityRef;

    [Header("Boid Scan Radius Settings")] [Space(5)]
    [Range(0.0f, 1.0f)] [SerializeField] float ScanRadius = 0.75f;
    public float UI_ScanRadius { get { return ScanRadius; } set { ScanRadius = value; } }
    [Range(0.0f, 1.0f)] [SerializeField] float SeparationRadius = 0.5f;
    public float UI_SeparationRadius { get { return SeparationRadius; } set { SeparationRadius = value; } }
    [Range(0.0f, 180.0f)] [SerializeField] float ScanAngle = 120.0f;
    public float UI_ScanAngle { get { return ScanAngle; } set { ScanAngle = value; } }

    [Header("Boid Spawning Settings")] [Space(5)]
    [SerializeField] GameObject BoidPrefab;
    [Range(1.0f, 300.0f)] [SerializeField] int BoidsToSpawn = 100;
    public int UI_BoidsToSpawn { get { return BoidsToSpawn; } set { BoidsToSpawn = value; } }
    [SerializeField] public List<Boid> Boids = new List<Boid>();
    
    bool PressedTarget, PressedObstacle, PressedRepellant;

    void Start()
    {
        if (Target.activeSelf != IsSeeking) Target.SetActive(IsSeeking);
        if (Obstacle.activeSelf != IsAvoiding) Obstacle.SetActive(IsAvoiding);
        if (Repellant.activeSelf != IsBeingRepelled) Repellant.SetActive(IsBeingRepelled);
        
        for (int i = 0; i < BoidsToSpawn; i++)
        {
            Vector2 SpawnPosition = new Vector2(Random.Range(-ScreenEdge.x, ScreenEdge.x), Random.Range(-ScreenEdge.y, ScreenEdge.y));
            Boid newBoid = Instantiate(BoidPrefab, SpawnPosition, Quaternion.identity, transform).GetComponent<Boid>();
            newBoid.name = "Boid " + i;
            Boids.Add(newBoid);
        }

        RepellantBoid = Repellant.GetComponent<Boid>();
    }

    bool MouseOverObject(Transform objectTransform)
    {
        if (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)).x <= objectTransform.position.x + objectTransform.localScale.x &&
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)).x >= objectTransform.position.x - objectTransform.localScale.x &&
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)).y <= objectTransform.position.y + objectTransform.localScale.y &&
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)).y >= objectTransform.position.y - objectTransform.localScale.y)
            return true;
        else
            return false;
    }

    void Update()
    {
        if (Target.activeSelf != IsSeeking) Target.SetActive(IsSeeking);
        if (Obstacle.activeSelf != IsAvoiding) Obstacle.SetActive(IsAvoiding);
        if (Repellant.activeSelf != IsBeingRepelled) Repellant.SetActive(IsBeingRepelled);

        if (Input.GetMouseButtonDown(0))
        {
            if (MouseOverObject(Target.transform)) PressedTarget = true;
            if (MouseOverObject(Obstacle.transform)) PressedObstacle = true;
            if (MouseOverObject(Repellant.transform)) PressedRepellant = true;
        }

        if (Input.GetMouseButton(0))
        {
            if(PressedTarget)
                Target.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, Camera.main.WorldToScreenPoint(Target.transform.position).z);
            if(PressedObstacle)
                Obstacle.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, Camera.main.WorldToScreenPoint(Obstacle.transform.position).z);
            if (PressedRepellant)
                Repellant.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, Camera.main.WorldToScreenPoint(Repellant.transform.position).z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (PressedTarget) PressedTarget = false;
            if (PressedObstacle) PressedObstacle = false;
            if (PressedRepellant) PressedRepellant = false;
        }
    }

    void FixedUpdate()
    {
        foreach (Boid boid in Boids)
        {
            if(IsScreenWrapping) UpdateEdges(boid);
            List<Boid> NearbyBoids = GetNearbyBoids(boid);

            boid.SteeringForce = Vector2.zero;
            if (IsSeeking)
                boid.SteeringForce += Seek(Target.transform.position, boid) * SeekWeight;
            if (IsAvoiding && Vector2.Distance(Obstacle.transform.position, boid.transform.position) <= ObstacleRadius)
                boid.SteeringForce += Avoidance(Obstacle.transform.position, ObstacleRadius, boid) * ObstacleWeight;
            if (IsBeingRepelled && Vector2.Distance(Repellant.transform.position, boid.transform.position) <= RepellantRadius)
                boid.SteeringForce += Avoidance(Repellant.transform.position, RepellantRadius, boid) * RepellantWeight;

            if (NearbyBoids.Count != 0)
            {
                List<Boid> SeparationBoids = GetSeparationBoids(NearbyBoids, boid);

                if(IsSeparating && SeparationBoids.Count != 0)
                    boid.SteeringForce += Separation(SeparationBoids, boid) * SeparationWeight;
                if(IsAligning)
                    boid.SteeringForce += Alignment(NearbyBoids, boid) * AlignmentWeight;
                if(IsCohering)
                    boid.SteeringForce += Cohesion(NearbyBoids, boid) * CohesionWeight;
            }

            boid.SteeringForce = Vector2.ClampMagnitude(boid.SteeringForce, MaxForce) / Mass;
            boid.Velocity = Vector2.ClampMagnitude(boid.Velocity + boid.SteeringForce, MaxSpeed);
            boid.transform.position += (Vector3)boid.Velocity * Time.deltaTime;
            boid.transform.up = boid.Velocity.normalized;

            Debug.DrawRay(boid.transform.position, boid.Velocity.normalized, Color.green); // Draw current velocity vector
        }

        if(Repellant.activeSelf)
        {
            RepellantBoid.SteeringForce = Vector2.zero;
            RepellantBoid.SteeringForce += RepellantSeek(Boids, RepellantBoid);

            RepellantBoid.SteeringForce = Vector2.ClampMagnitude(RepellantBoid.SteeringForce, RepellantForce) / RepellantMass;
            RepellantBoid.Velocity = Vector2.ClampMagnitude(RepellantBoid.Velocity + RepellantBoid.SteeringForce, RepellantSpeed);
            RepellantBoid.transform.position += (Vector3)RepellantBoid.Velocity * Time.deltaTime;
            RepellantBoid.transform.up = RepellantBoid.Velocity.normalized;

            Debug.DrawRay(RepellantBoid.transform.position, RepellantBoid.Velocity.normalized, Color.green); // Draw current velocity vector
        }
    }

    Vector2 Seek(Vector2 targetPosition, Boid thisBoid)
    {
        Vector2 SeekVelocity = Vector2.zero;
        SeekVelocity = (targetPosition - (Vector2)thisBoid.transform.position).normalized * MaxSpeed;
        //Debug.DrawRay(thisBoid.transform.position, SeekVelocity.normalized, Color.magenta);
        return (SeekVelocity - thisBoid.Velocity);
    }

    Vector2 Avoidance(Vector2 avoidancePosition, float avoidanceRadius, Boid thisBoid)
    {
        Vector2 AvoidanceVelocity = Vector2.zero;
        AvoidanceVelocity -= (avoidancePosition - (Vector2)thisBoid.transform.position) / Vector2.Distance(avoidancePosition, thisBoid.transform.position);
        AvoidanceVelocity = AvoidanceVelocity.normalized * MaxSpeed;
        //Debug.DrawRay(thisBoid.transform.position, AvoidanceVelocity.normalized, Color.magenta);
        return (AvoidanceVelocity - thisBoid.Velocity);
    }

    Vector2 Separation(List<Boid> separationBoids, Boid thisBoid)
    {
        Vector2 SeparationVelocity = Vector2.zero;
        foreach (Boid otherBoid in separationBoids)
            SeparationVelocity -= (Vector2)(otherBoid.transform.position - thisBoid.transform.position) / Vector2.Distance(otherBoid.transform.position, thisBoid.transform.position);
        SeparationVelocity /= separationBoids.Count;
        SeparationVelocity = SeparationVelocity.normalized * MaxSpeed;
        //Debug.DrawRay(thisBoid.transform.position, SeparationVelocity.normalized, Color.magenta);
        return (SeparationVelocity - thisBoid.Velocity);
    }

    Vector2 Alignment(List<Boid> nearbyBoids, Boid thisBoid)
    {
        Vector2 AlignmentVelocity = Vector2.zero;
        foreach(Boid otherBoid in nearbyBoids)
            AlignmentVelocity += otherBoid.Velocity;

        AlignmentVelocity /= nearbyBoids.Count;
        AlignmentVelocity = AlignmentVelocity.normalized * MaxSpeed;
        //Debug.DrawRay(thisBoid.transform.position, AlignmentVelocity.normalized, Color.magenta);
        return (AlignmentVelocity - thisBoid.Velocity);
    }

    Vector2 Cohesion(List<Boid> nearbyBoids, Boid thisBoid)
    {
        Vector2 CohesionVelocity = Vector2.zero;
        foreach (Boid otherBoid in nearbyBoids)
            CohesionVelocity += (Vector2)otherBoid.transform.position;

        CohesionVelocity /= nearbyBoids.Count;
        CohesionVelocity -= (Vector2)thisBoid.transform.position;
        CohesionVelocity = CohesionVelocity.normalized * MaxSpeed;
        CohesionVelocity = Vector2.SmoothDamp(thisBoid.transform.up, CohesionVelocity, ref CohesionVelocityRef, CohesionSmoothTime);
        //Debug.DrawRay(thisBoid.transform.position, CohesionVelocity.normalized, Color.magenta);
        return (CohesionVelocity - thisBoid.Velocity);
    }

    Vector2 RepellantSeek(List<Boid> allBoids, Boid repellant)
    {
        Vector2 RepellantVelocity = Vector2.zero;
        foreach (Boid boid in allBoids)
            RepellantVelocity += (Vector2)boid.transform.position;

        RepellantVelocity /= allBoids.Count;
        RepellantVelocity -= (Vector2)repellant.transform.position;
        RepellantVelocity = RepellantVelocity.normalized * RepellantSpeed;
        RepellantVelocity = Vector2.SmoothDamp(repellant.transform.up, RepellantVelocity, ref CohesionVelocityRef, 0.1f);
        //Debug.DrawRay(repellant.transform.position, RepellantVelocity.normalized, Color.magenta);
        return (RepellantVelocity - repellant.Velocity);
    }

    List<Boid> GetNearbyBoids(Boid thisBoid)
    {
        List<Boid> nearbyBoids = new List<Boid>();
        foreach (Boid otherBoid in Boids)
            if (otherBoid != thisBoid)
                if (Vector2.Distance(otherBoid.transform.position, thisBoid.transform.position) <= ScanRadius)
                    if (Vector3.Angle(thisBoid.transform.up, otherBoid.transform.position - thisBoid.transform.position) <= ScanAngle)
                        if (!nearbyBoids.Contains(otherBoid))
                            nearbyBoids.Add(otherBoid);
        return nearbyBoids;
    }

    List<Boid> GetSeparationBoids(List<Boid> nearbyBoids, Boid thisBoid)
    {
        List<Boid> separationBoids = new List<Boid>();
        foreach (Boid otherBoid in nearbyBoids)
            if (Vector2.Distance(otherBoid.transform.position, thisBoid.transform.position) <= ScanRadius * SeparationRadius)
                if (!separationBoids.Contains(otherBoid))
                    separationBoids.Add(otherBoid);
        return separationBoids;
    }

    void UpdateEdges(Boid boid)
    {
        if (boid.transform.position.x > ScreenEdge.x)
            boid.transform.position = new Vector2(-ScreenEdge.x, boid.transform.position.y);
        else if (boid.transform.position.x < -ScreenEdge.x)
            boid.transform.position = new Vector2(ScreenEdge.x, boid.transform.position.y);

        if (boid.transform.position.y > ScreenEdge.y)
            boid.transform.position = new Vector2(boid.transform.position.x, -ScreenEdge.y);
        else if (boid.transform.position.y < -ScreenEdge.y)
            boid.transform.position = new Vector2(boid.transform.position.x, ScreenEdge.y);
    }

    public void SpawnBoids()
    {
        foreach (Boid boid in Boids)
            Destroy(boid.gameObject);
        Boids.Clear();
        for (int i = 0; i < BoidsToSpawn; i++)
        {
            Vector2 SpawnPosition = new Vector2(Random.Range(-ScreenEdge.x, ScreenEdge.x), Random.Range(-ScreenEdge.y, ScreenEdge.y));
            Boid newBoid = Instantiate(BoidPrefab, SpawnPosition, Quaternion.identity, transform).GetComponent<Boid>();
            newBoid.name = "Boid " + i;
            Boids.Add(newBoid);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
        if (Obstacle.activeSelf) Gizmos.DrawWireSphere(Obstacle.transform.position, ObstacleRadius);
        if (Repellant.activeSelf) Gizmos.DrawWireSphere(Repellant.transform.position, RepellantRadius);

        foreach (Boid boid in Boids)
        {
            Gizmos.color = Color.yellow;
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
            Gizmos.DrawWireSphere(boid.transform.position, ScanRadius);

            Gizmos.color = Color.red;
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
            Gizmos.DrawWireSphere(boid.transform.position, ScanRadius * SeparationRadius);

            Gizmos.color = Color.cyan;
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
            Gizmos.DrawRay(boid.transform.position, Quaternion.AngleAxis(ScanAngle, boid.transform.forward) * boid.transform.up);
            Gizmos.DrawRay(boid.transform.position, Quaternion.AngleAxis(-ScanAngle, boid.transform.forward) * boid.transform.up);
        }
    }
    
}
