using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Carnation/Actions")]
    public class AnimAct : Action
    {
  
  
        public string animationName;
        public override void Act(Controller controller)
        {
            controller.SetAnimation(animationName, true);
        }

    }
}
