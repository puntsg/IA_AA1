using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Carnation/Decision/Live")]
    public class DecideBeheivor : Decision
    { 
        //public float healthChange;
        public override bool Decide(Controller controller)
        {
           // if(controller._hb.GetHealth()<=healthChange)
            //{
               // return true;
            //}
            return false;
        }
    }
}
