using UnityEngine;

namespace FSM
{
    enum EDistanceCheck { CLOSER, FARTHER}


    [CreateAssetMenu(menuName = "FSM/Carnation/Decision/NearbyPlayer")]
    public class NearbyPlayer : Decision
    {
        [SerializeField] EDistanceCheck condition; 
        [SerializeField] float targetDistance = 2f;
        [SerializeField] float tolerance = .05f;

        public override bool Decide(Controller controller)
        {
            foreach (GameObject objectInList in controller.GetComponent<Detector>().nearbyObjects){
                if (objectInList.tag.Equals("player")){
                    float distance = Vector3.Distance(controller.transform.position, objectInList.transform.position);
                    switch (condition)
                    {
                        case EDistanceCheck.FARTHER:
                            return (distance > tolerance);
                        case EDistanceCheck.CLOSER:
                            return (distance < tolerance);
                    }
                }
            }
            return false;
        }
    }
}
