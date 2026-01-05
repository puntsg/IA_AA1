using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Carnation/Action/InvertMoveAction")]
    public class InvertMoveAction : Action
    {
        public float speed = 100;
        public override void Act(Controller controller)
        {
            Vector2 moveInput = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
                moveInput.y = -1;
            else if (Input.GetKey(KeyCode.S))
                moveInput.y = +1;
            if (Input.GetKey(KeyCode.D))
                moveInput.x = -1;
            else if (Input.GetKey(KeyCode.A))
                moveInput.x = +1;
            moveInput.Normalize();
            controller.GetComponent<Rigidbody2D>().linearVelocity = moveInput * speed * Time.fixedDeltaTime;
        }
    }
}