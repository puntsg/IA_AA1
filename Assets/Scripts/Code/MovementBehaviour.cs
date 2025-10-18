using System;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[Serializable]
public class MovementBehaviour
{
    [Header("Target Params")]
    public Transform targetTransform;
    public Vector2 targetPosition;
    
    [Header("Player Params")]
    public Transform currentTransform;
    public Vector2 currentPosition;
    public Vector2 currentDir;

    public void AssignTransforms(Transform newCurrentTransform,Transform newTargetTransform )
    {
        this.currentTransform = newCurrentTransform;
        this.targetTransform = newTargetTransform; 
    }

    virtual public Vector2 getDirection() { return Vector2.zero; }
}

[Serializable]
public class Seek : MovementBehaviour
{
    public bool seek = true;
    public float minDistanceToObjective;

    public override Vector2 getDirection() {
        this.currentPosition = currentTransform.position;
        this.targetPosition = targetTransform.position;
        if (Vector3.Distance(currentPosition,targetPosition) > minDistanceToObjective) {
            Vector3 DesiredVelocity = targetPosition - currentPosition;
            DesiredVelocity = DesiredVelocity.normalized;
            currentDir = DesiredVelocity;
            return currentDir; 
        }
        else
        { return Vector2.zero; }
    } 
}

[Serializable]
public class Arrive : MovementBehaviour
{
    public bool arrive = true;
    public override Vector2 getDirection() {
        this.currentPosition = currentTransform.position;
        this.targetPosition = targetTransform.position;
        return new Vector2(0, 0); }
}

[Serializable]
public class Pursue : MovementBehaviour
{
    public bool pursue = true;
    public override Vector2 getDirection() {
        this.currentPosition = currentTransform.position;
        this.targetPosition = targetTransform.position;
        return new Vector2(0, 0); 
    }
}

[Serializable]
public class Wander : MovementBehaviour
{
    public bool wander = true;
    [Header("Wander Settings")]
    public float wanderRadius = 1.5f;
    public float wanderOffset = 2.0f;
    public float wanderMaxChange = 0.5f;

    private float wanderAngle = 0f;
    private Vector2 lastForward = Vector2.right; 

    public override Vector2 getDirection() {
        this.currentPosition = currentTransform.position;
        this.targetPosition = targetTransform.position;

        float angleDelta = Random.Range(-wanderMaxChange, wanderMaxChange);
        wanderAngle += angleDelta;

        Vector2 forward = currentDir.sqrMagnitude > 0.0001f ? currentDir.normalized : lastForward;

        Vector2 circleCenter = currentPosition + forward * wanderOffset;

        Vector2 offset = new Vector2(Mathf.Cos(wanderAngle), Mathf.Sin(wanderAngle)) * wanderRadius;
        Vector2 wanderTarget = circleCenter + offset;

        Vector2 desiredVelocity = (wanderTarget - currentPosition).normalized;
        currentDir = desiredVelocity;
        lastForward = desiredVelocity.sqrMagnitude > 0.0001f ? desiredVelocity : lastForward;
        return currentDir;
    }
}

[Serializable]
public class Flee : MovementBehaviour
{
    public bool flee = true;
    public float maxDistanceToObjective;

    public override Vector2 getDirection()
    {
        this.currentPosition = currentTransform.position;
        this.targetPosition = targetTransform.position;
        if (Vector3.Distance(currentPosition, targetPosition) > maxDistanceToObjective || maxDistanceToObjective == -1)
        {
            Vector3 DesiredVelocity = targetPosition - currentPosition;
            DesiredVelocity = DesiredVelocity.normalized;
            currentDir = -DesiredVelocity;
            return currentDir;
        }
        else
        { return Vector2.zero; }
    }
}
//
[Serializable]
public class Flocking : MovementBehaviour
{
    public Vector2  acceleration = Vector2.zero;
    public float r = 2, maxForce = .03f, maxSpeed = 2;

   
    public Vector2 getDirection(Rigidbody2D rigidbody2D)
    {
        this.currentPosition = currentTransform.position;
        this.targetPosition = targetTransform.position;

        rigidbody2D.linearVelocity += acceleration;
        if(rigidbody2D.angularVelocity > maxSpeed)
            rigidbody2D.angularVelocity = maxSpeed;
        acceleration *= 0;
        return rigidbody2D.linearVelocity;
    }
}