
using System;
using UnityEngine;

[Serializable]
public class MovementBehaviour
{
    public Vector2 targetPos;
    virtual public Vector2 getDirection(Vector2 currentPos) { return Vector2.zero; }
}

[Serializable]
public class Seek : MovementBehaviour
{
    public float seek = 0;
    public override Vector2 getDirection(Vector2 currentPos) {
        Vector3 DesiredVelocity = targetPos - currentPos;
        DesiredVelocity = DesiredVelocity.normalized;
        DesiredVelocity *= 5f;
       
        Vector2 SteeringForce = (DesiredVelocity -(Vector3.one*5));
        SteeringForce /= 5;
        return SteeringForce * 5;
    } 
}

[Serializable]
public class Arrive : MovementBehaviour
{

    public float arrive = 0;
    public override Vector2 getDirection(Vector2 currentPos) { return new Vector2(0, 0); }
}

[Serializable]
public class Pursue : MovementBehaviour
{

    public float pursue = 0;
    public override Vector2 getDirection(Vector2 currentPos) { return new Vector2(0, 0); }
}

[Serializable]
public class Wander : MovementBehaviour
{

    public float wander = 0;
    public override Vector2 getDirection(Vector2 currentPos) { return new Vector2(0, 0); }
}