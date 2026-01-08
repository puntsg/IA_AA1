using UnityEngine;
namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Carnation/Decision/PlayerNotNearby")]
    public class PlayerNOTNearby : Decision
    {
        public override bool Decide(Controller controller)
        {
            bool matched = false;
            foreach (GameObject objectInList in controller.GetComponent<Detector>().nearbyObjects)
                if (objectInList.tag.Equals("Player"))
                    matched = true;

            return !matched;
        }
    }
}