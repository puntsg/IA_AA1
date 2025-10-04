using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class MovingType{
    public MovementBehaviour movementBehaviour;
    [Range(0.0F, 1.0F)]public float weight = 1f;
}
public class Movement : MonoBehaviour
{
    public List<MovingType> movingBehaviours;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
