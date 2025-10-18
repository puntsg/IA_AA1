using System;
using System.Collections.Generic;
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
        [Header("Boids (simple)")]
        public float neighborRadius = 3.5f;    
        public float separationRadius = 1.5f;  
        public float separationWeight = 1.5f;
        public float alignmentWeight = 1.0f;
        public float cohesionWeight = 1.0f;

        public override Vector2 getDirection()
        {
            currentPosition = currentTransform.position;

            var mgr = FlockManager.instance;
            if (mgr == null || mgr.flockingObjects == null || mgr.flockingObjects.Count == 0)
                return currentDir; 

            Vector2 sep = Vector2.zero;
            Vector2 ali = Vector2.zero;
            Vector2 coh = Vector2.zero;

            int aliCohCount = 0;
            int sepCount = 0;

            float neigh2 = neighborRadius * neighborRadius;
            float sep2 = separationRadius * separationRadius;

            foreach (var other in mgr.flockingObjects)
            {
                if (other == null || other == this || other.currentTransform == null) continue;

                Vector2 op = (Vector2)other.currentTransform.position;
                Vector2 toOther = op - currentPosition;
                float d2 = toOther.sqrMagnitude;

                if (d2 > 0f && d2 <= neigh2)
                {
                    if (other.currentDir.sqrMagnitude > 0.0001f)
                        ali += other.currentDir.normalized;

                    coh += op;

                    aliCohCount++;
                }

                if (d2 > 0f && d2 < sep2)
                {
                    sep += (currentPosition - op) / Mathf.Sqrt(d2);
                    sepCount++;
                }
            }

            if (aliCohCount > 0)
            {
                ali /= aliCohCount;
                coh = (coh / aliCohCount) - currentPosition; 
            }

            if (sepCount > 0)
                sep /= sepCount;

            Vector2 steer = separationWeight * sep
                          + alignmentWeight * ali
                          + cohesionWeight * coh;

         if (steer.sqrMagnitude > 0.0001f)
            currentDir = steer.normalized;

        return currentDir;
    }
}