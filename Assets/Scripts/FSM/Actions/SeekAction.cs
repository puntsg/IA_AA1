using UnityEngine;
namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Carnation/Action/MoveAction")]
    public class SeekAction : Action
    {
        public override void Act(Controller controller)
        {
            
        }
        /*public bool seek = true;
        public float minDistanceToObjective;
        public override Vector2 getDirection()
        {
            this.currentPosition = currentTransform.position;
            this.targetPosition = targetTransform.position;
            if (Vector3.Distance(currentPosition, targetPosition) > minDistanceToObjective)
            {
                Vector3 DesiredVelocity = targetPosition - currentPosition;
                DesiredVelocity = DesiredVelocity.normalized;
                currentDir = DesiredVelocity;
                return currentDir;
            }
            else
            { return Vector2.zero; }
        }*/
    }
}
