using UnityEngine;
namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Carnation/Decision/FarFromGold")]
    public class FarFromGold : Decision
    {
        public override bool Decide(Controller controller)
        {
            foreach (GameObject objectInList in controller.GetComponent<Detector>().nearbyObjects)
                if (objectInList.tag.Equals("Gold"))
                    return false;

            return true;
        }
    }
}