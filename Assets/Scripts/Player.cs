using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] Vector2 moveInput;
    public void OnMove(InputValue value){moveInput = value.Get<Vector2>();}

    [Header("Other")]
    [SerializeField]float speed = 1.0f;
    Rigidbody2D rigidbody;

    void Start()
    {
       rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigidbody.linearVelocity = moveInput*speed*Time.deltaTime;
    }
}
