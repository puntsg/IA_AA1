using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(fileName = "FSM/Carnation/Decision/UpLimit")]
    public class UpLimit : Decision
    {
        public float thereshold = 100;
        public override bool Decide(Controller controller)
        {
            Debug.Log("CheckingUpLimit " + controller.transform.position.y + " - " + thereshold + " : " + (controller.transform.position.y < thereshold));
            return (controller.transform.position.y > thereshold);
        }
    }
}
