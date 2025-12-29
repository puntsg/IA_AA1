using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Carnation/Action/IdleAction")]
    public class IdleAction : Action
    {
        public override void Act(Controller controller)
        {
            controller.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            Debug.Log("Hi, i'm in idle");
        }
     }
}
