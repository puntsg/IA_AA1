using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class MovingType{
    public enum EmovementBehaviour { A,B,C}
    public EmovementBehaviour movement;
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
        Vector3 direction = Vector3.zero;
        foreach (var movingBehaviour in movingBehaviours)
        {
            direction = movingBehaviour.movementBehaviour.getDirection(this.transform.position)*movingBehaviour.weight;
        }
    }
}
