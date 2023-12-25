using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarAI : MonoBehaviour
{
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float reverseDistance;

    private CarAIController carController;
    private Transform carTransform;
    [SerializeField] private Transform target;
    private float angleToDir;
    private int currentTargetIndex;

    private float forwardAmount;
    private float turnAmount;
    private float distanceToTarget;
    private Vector3 previousPosition;
    private bool solvingStuck = false;
    private float checkStuckTimer = 0;
    private float solvingStuckCD = 0;
    public Placement AICarPlacement { get; private set; }

    private void Awake()
    {
        carController = GetComponent<CarAIController>();
        carTransform = transform;
        currentTargetIndex = 1;
        AICarPlacement = GetComponent<Placement>();
    }

    private void Start()
    {
        SetTargetPosition(TrackCheckPoints.Instance.CheckPointSingleList[currentTargetIndex].transform);
    }

    private void Update()
    {
        if (solvingStuck) return;

        CheckStuck();

        // Debug.Log(Vector3.Distance(target.position, carTransform.position));
        if (Vector3.Distance(target.position, carTransform.position) <= 7f)
        {
            // Debug.Log("Reached destination");
            carController.ReRandomSpeed();
            AICarPlacement.PassACheckPoint();
            currentTargetIndex++;
            if (currentTargetIndex >= TrackCheckPoints.Instance.CheckPointSingleList.Count) currentTargetIndex = 0;
            SetTargetPosition(TrackCheckPoints.Instance.CheckPointSingleList[currentTargetIndex].transform);
        }


        forwardAmount = 0f;
        turnAmount = 0f;

        float reachedTargetDistance = 3f;
        distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget > reachedTargetDistance)
        {
            // Still too far, keep going
            Vector3 dirToMovePosition = (target.position - transform.position).normalized;

            float dot = Vector3.Dot(transform.forward, dirToMovePosition);
            CheckTargetInfrontOrBehind(dot);

            angleToDir = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);

            if (angleToDir > 0)
            {
                turnAmount = 1f;
            }
            else
            {
                turnAmount = -1f;
            }
        }
        else
        {
            // Reached target
            if (carController.GetSpeed() > 15f)
            {
                forwardAmount = -1f;
            }
            else
            {
                forwardAmount = 0f;
            }
            turnAmount = 0f;
        }

        carController.SetInputs(forwardAmount, turnAmount, angleToDir);

        if (Time.frameCount % 5 == 0) previousPosition = carTransform.position;
    }

    private void CheckStuck()
    {
        if (Vector3.Distance(previousPosition, carTransform.position) <= 0.15f)
        {
            checkStuckTimer += Time.deltaTime;
            if (checkStuckTimer >= 10f)
            {
                checkStuckTimer = 0;
                StartCoroutine(SolveStuck(2.5f));
            }
        }
        else checkStuckTimer = 0;

        if (solvingStuckCD > 0) solvingStuckCD -= Time.deltaTime;
    }

    private IEnumerator SolveStuck(float duration)
    {
        if (solvingStuckCD > 0) yield break;
        solvingStuck = true;
        float currentProgres = 0f;
        while (currentProgres < duration)
        {
            currentProgres += Time.deltaTime;
            carController.SetInputs(-1, 0, 0);
            yield return null;
        }
        var closestPoint = TrackCheckPoints.Instance.GetClosestCheckPoint(transform.position);
        SetTargetPosition(closestPoint.transform);
        currentTargetIndex = TrackCheckPoints.Instance.CheckPointSingleList.IndexOf(closestPoint);
        solvingStuck = false;
    }

    private void CheckTargetInfrontOrBehind(float dot)
    {
        if (dot > 0)
        {
            // Target in front
            forwardAmount = 1f;


            float stoppingSpeed = 40f;
            if (distanceToTarget < stoppingDistance && carController.GetSpeed() > stoppingSpeed)
            {
                // Within stopping distance and moving forward too fast
                forwardAmount = -1f;
            }
        }
        else
        {
            // Target behind

            if (distanceToTarget > reverseDistance)
            {
                // Too far to reverse
                forwardAmount = 1f;
            }
            else
            {
                forwardAmount = -1f;
            }
        }
    }

    public void SetTargetPosition(Transform target)
    {
        // if (this.target != target)
        this.target = target;
    }
}
