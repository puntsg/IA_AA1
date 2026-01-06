using UnityEngine;
namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Carnation/Decision/NearbyGold")]
    public class NearbyGold : Decision
    {
        public override bool Decide(Controller controller)
        {
            foreach (GameObject objectInList in controller.GetComponent<Detector>().nearbyObjects)
                if (objectInList.tag.Equals("Gold"))
                    return true;

            return false;
        }
    }
}
