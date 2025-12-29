using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSM
{
    public class Controller : MonoBehaviour
    {
        public State currentState; //Apuntador al estado actual 
        public bool ActiveAI { get; set; }
        private Animator _anim;
        public void Start()
        {
            ActiveAI = true;    //Para activar la IA

        }
        public void Update() //Se ejecutan las accioens del estado actual
        {
            if (!ActiveAI) return; //El parámetro permite que los estados tengan una referencia al controlador, para poder llamar a sus métods
           
            
            currentState.UpdateState(this); 
        }
        public void Transition(State nextState)
        {
            currentState = nextState;
        }

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }
        public void SetAnimation(string animation, bool value)
        {
            AnimatorControllerParameter[] animp = _anim.parameters;
            for (int i = 0; i< animp.Length; i++)
                {
                _anim.SetBool(animp[i].name, false);
            }
         //   Debug.Log(animation);
            _anim.SetBool(animation, value);

        } 
    }
}
     