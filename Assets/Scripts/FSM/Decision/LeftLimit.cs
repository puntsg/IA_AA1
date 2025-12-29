using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Carnation/Decision/LeftLimit")]
    public class LeftLimit : Decision
    {
        public float thereshold = -1;
        public override bool Decide(Controller controller)
        {
            Debug.Log("CheckingLeftLimit " + controller.transform.position.x + " - " + thereshold + " : " + (controller.transform.position.x < thereshold));
            return (controller.transform.position.x < thereshold);
        }
    }
}
