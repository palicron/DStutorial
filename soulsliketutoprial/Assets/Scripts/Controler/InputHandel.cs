using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class InputHandel : MonoBehaviour
    {
      
        private float horizontal;
        private float vertical;
        private bool b_input;
        private bool a_input;
        private bool x_input;
        private bool y_input;
        private bool rb_input;
        private float rt_input;
        private bool lb_input;
        private float lt_input;


        StateManager states;
        CamaraManager camaraManager;

        private float delta;
        // Use this for initialization
        void Start()
        {
            states = this.GetComponent<StateManager>();
            states.Init();

            camaraManager = CamaraManager.singleton;
            camaraManager.Init(this.transform);

        }

        private void FixedUpdate()
        {

            delta = Time.fixedDeltaTime;
            getInput();
            UpdateStates();
            states.FixedTick(delta);
            camaraManager.Tick(delta);
        }

        private void Update()
        {
            delta = Time.deltaTime;
            states.Tick(delta);

        }

        private void getInput()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
            b_input = Input.GetButton("b_input");
        }

        private void UpdateStates()
        {
            states.horizontal = horizontal;
            states.vertical = vertical;

            Vector3 v = vertical * camaraManager.transform.forward;
            Vector3 h = horizontal * camaraManager.transform.right;

            states.moveDir = (v + h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);
            if (b_input)
            {
                states.run = (states.moveAmount > 0);

            }
            else
            {
                states.run = false;
            }


        }
    }
}
