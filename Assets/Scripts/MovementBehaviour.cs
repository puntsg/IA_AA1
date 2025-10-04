
using UnityEngine;

public class MovementBehaviour
{
    public Vector2 targetPos;
    virtual public Vector2 getDirection(Vector2 currentPos) { return Vector2.zero; }
}
public class Seek : MovementBehaviour
{
    public override Vector2 getDirection(Vector2 currentPos) {
        Vector3 DesiredVelocity = targetPos - currentPos;
        DesiredVelocity = DesiredVelocity.normalized;
        DesiredVelocity *= 5f;
       
        Vector2 SteeringForce = (DesiredVelocity -(Vector3.one*5));
        SteeringForce /= 5;
        return SteeringForce * 5;
    } 
}
public class Arrive : MovementBehaviour
{
    public override Vector2 getDirection(Vector2 currentPos) { return new Vector2(0, 0); }
}
public class Pursue : MovementBehaviour
{
    public override Vector2 getDirection(Vector2 currentPos) { return new Vector2(0, 0); }
}
public class Wander : MovementBehaviour
{
    public override Vector2 getDirection(Vector2 currentPos) { return new Vector2(0, 0); }
}