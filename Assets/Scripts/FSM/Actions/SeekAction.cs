using UnityEngine;
namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Carnation/Action/MoveAction/seekAction")]
    public class SeekAction : Action
    {
        public float minDistanceToObjective = 10f;

        public float seekSpeed = 5f;


        public override void Act(Controller controller)
        {
            GameObject target = controller.GetTarget();

            Vector2 currentPosition = controller.transform.position;
            Vector2 targetPosition = target.transform.position;

            float distanceToTarget = Vector2.Distance(currentPosition, targetPosition);

            if (distanceToTarget > minDistanceToObjective || minDistanceToObjective == -1)
            {
                Vector2 seekDirection = (currentPosition - targetPosition).normalized;

                controller.transform.position -= (Vector3)(seekDirection * seekSpeed * Time.deltaTime);
            }
        }
    }
}
