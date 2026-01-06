using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSM
{
    public enum ObjectTypes
    {
        Player,
        NPC
    }
    
    public class Controller : MonoBehaviour
    {
        public State currentState; //Apuntador al estado actual
        
        public ObjectTypes objectType;
        public GameObject currentTarget;
        
        public bool ActiveAI { get; set; }
        private Animator _anim;
        
        public void Start()
        {
            ActiveAI = true;    //Para activar la IA

        }
        public void Update() //Se ejecutan las accioens del estado actual
        {
            if (!ActiveAI) return; //El par�metro permite que los estados tengan una referencia al controlador, para poder llamar a sus m�tods
           
            
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

        public GameObject GetTarget()
        { 
            switch (objectType)
            {
                case ObjectTypes.Player: GetPlayer(); break;
                
                case ObjectTypes.NPC: GetNPC(); break;
            }
            
            return currentTarget;
        }

        private void GetPlayer()
        {
            List<GameObject> detectedObjects = GetComponent<Detector>().nearbyObjects;

            foreach (GameObject ob in detectedObjects)
            {
                if (ob.CompareTag("Player"))
                {
                    currentTarget = ob;
                }
            }
        }

        private void GetNPC()
        {
            List<GameObject> detectedObjects = GetComponent<Detector>().nearbyObjects;

            foreach (GameObject ob in detectedObjects)
            {
                if (ob.CompareTag("NPC"))
                {
                    currentTarget = ob;
                }
            }
        }
    }
}
     