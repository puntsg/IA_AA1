using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class MovementBehaviour
{
    public Vector2 targetPosition;
    public Vector2 currentPosition;
    public Vector2 currentDir;
    virtual public Vector2 getDirection(Vector2 currentPos,Vector2 targetPos) { return Vector2.zero; }
}

[Serializable]
public class Seek : MovementBehaviour
{
    public bool seek = true;
    public float minDistanceToObjective;

    public override Vector2 getDirection(Vector2 currentPos, Vector2 targetPos) {
        this.currentPosition = currentPos;
        this.targetPosition = targetPos;
        if (Vector3.Distance(currentPos,targetPos) > minDistanceToObjective) {
            Vector3 DesiredVelocity = targetPos - currentPos;
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
    public override Vector2 getDirection(Vector2 currentPos, Vector2 targetPos) {
        this.currentPosition = currentPos;
        this.targetPosition = targetPos;
        return new Vector2(0, 0); }
}

[Serializable]
public class Pursue : MovementBehaviour
{

    public bool pursue = true;
    public override Vector2 getDirection(Vector2 currentPos, Vector2 targetPos) {
        this.currentPosition = currentPos;
        this.targetPosition = targetPos;
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

    public override Vector2 getDirection(Vector2 currentPos, Vector2 targetPos) {
        this.currentPosition = currentPos;
        this.targetPosition = targetPos;

        float angleDelta = Random.Range(-wanderMaxChange, wanderMaxChange);
        wanderAngle += angleDelta;

        Vector2 forward = currentDir.sqrMagnitude > 0.0001f ? currentDir.normalized : lastForward;

        Vector2 circleCenter = currentPos + forward * wanderOffset;

        Vector2 offset = new Vector2(Mathf.Cos(wanderAngle), Mathf.Sin(wanderAngle)) * wanderRadius;
        Vector2 wanderTarget = circleCenter + offset;

        Vector2 desiredVelocity = (wanderTarget - currentPos).normalized;
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

    public override Vector2 getDirection(Vector2 currentPos, Vector2 targetPos)
    {
        this.currentPosition = currentPos;
        this.targetPosition = targetPos;
        if (Vector3.Distance(currentPos, targetPos) > maxDistanceToObjective || maxDistanceToObjective == -1)
        {
            Vector3 DesiredVelocity = targetPos - currentPos;
            DesiredVelocity = DesiredVelocity.normalized;
            currentDir = -DesiredVelocity;
            return currentDir;
        }
        else
        { return Vector2.zero; }


    }
}

[Serializable]
public class Flocking : MovementBehaviour
{

    public bool pursue = true;
    public override Vector2 getDirection(Vector2 currentPos, Vector2 targetPos)
    {
        this.currentPosition = currentPos;
        this.targetPosition = targetPos;
        return new Vector2(0, 0);
    }
}