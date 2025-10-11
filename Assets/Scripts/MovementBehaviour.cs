
using System;
using UnityEngine;

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
    public override Vector2 getDirection(Vector2 currentPos, Vector2 targetPos) {
        this.currentPosition = currentPos;
        this.targetPosition = targetPos;
        return new Vector2(0, 0); 
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
        if (Vector3.Distance(currentPos, targetPos) < maxDistanceToObjective || maxDistanceToObjective == -1)
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