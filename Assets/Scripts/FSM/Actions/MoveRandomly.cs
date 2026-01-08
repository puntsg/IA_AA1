using UnityEngine;

namespace FSM{

    [CreateAssetMenu(menuName = "FSM/Carnation/Action/MoveAction/RandomMove")]
    public class MoveRandomly : Action
    {
        [SerializeField] float maxSpeed;
        [SerializeField] Vector2 direction;

        MoveRandomly()
        {
            direction = Vector2.zero;
        }
        public override void Act(Controller controller)
        {
            direction.x += Random.Range(-1f, 1f);
            direction.x = Mathf.Clamp(direction.x, -maxSpeed, maxSpeed);
            if (direction.x > maxSpeed)
                direction.x = maxSpeed;
            else if(direction.x < -maxSpeed)
                direction.x = -maxSpeed;


            direction.y += Random.Range(-1f, 1f);
            direction.y = Mathf.Clamp(direction.y, -maxSpeed, maxSpeed);
            if (direction.y > maxSpeed)
                direction.y = maxSpeed;
            else if (direction.y < -maxSpeed)
                direction.y = -maxSpeed;

            controller.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * maxSpeed;
        }
    }
}
