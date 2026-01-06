using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Carnation/Action/FleeAction")]
    public class FleeAction : Action
    {
        public float maxDistanceToObjective = 10f;
        
        public float fleeSpeed = 5f;
        
        
        public override void Act(Controller controller)
        {
            GameObject target = controller.GetTarget();
            
            Vector2 currentPosition = controller.transform.position;
            Vector2 targetPosition = target.transform.position;
            
            float distanceToTarget = Vector2.Distance(currentPosition, targetPosition);

            if (distanceToTarget < maxDistanceToObjective || maxDistanceToObjective == -1)
            {
                Vector2 fleeDirection = (currentPosition - targetPosition).normalized;
                
                controller.transform.position += (Vector3)(fleeDirection * fleeSpeed * Time.deltaTime);
            }
        }
    }
}
