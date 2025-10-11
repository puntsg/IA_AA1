using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class MovingType{
    public enum EmovementBehaviour { SEEK,ARRIVE,PURSUE, WANDER, FLEE}
    public EmovementBehaviour movement;
    [SerializeReference] public MovementBehaviour movementBehaviour;
    [Range(0.0F, 1.0F)]public float weight = 1f;
}
public class Movement : MonoBehaviour
{
    [Header("Params")]
    [SerializeField]Vector3 direction;
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField]float speed = 1f;
    [Header("Movements")]
    public List<MovingType> movingBehaviours;
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        direction = Vector3.zero;
        foreach (var movingBehaviour in movingBehaviours)
        {
            Vector3 dirToAdd = movingBehaviour.movementBehaviour.getDirection(this.transform.position, Player.instance.transform.position) *movingBehaviour.weight;
            direction += dirToAdd;
        }
        rigidbody2D.linearVelocity = direction*speed*Time.deltaTime;
    }
}
