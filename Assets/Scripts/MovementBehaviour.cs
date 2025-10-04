using UnityEngine;

public class MovementBehaviour
{
    virtual public Vector2 getDirection() { return Vector2.zero; }
}
public class Seek : MovementBehaviour
{
    public override Vector2 getDirection() { return new Vector2(0, 0); } 
}
